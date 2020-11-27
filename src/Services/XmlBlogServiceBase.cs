namespace Miniblog.Core.Services
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    using Miniblog.Core.Models;

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public abstract class XmlBlogServiceBase : IBlogService
    {
        protected XmlBlogServiceBase(IHttpContextAccessor contextAccessor) => this.ContextAccessor = contextAccessor;

        protected List<Post> Cache { get; set; } = new List<Post>();

        protected IHttpContextAccessor ContextAccessor { get; }

        public abstract Task DeletePost(Post post);

        public virtual void RefreshCache()
        {
            this.Cache.Clear();
            this.InitializeAsync().ConfigureAwait(true);
        }

        public virtual async Task InitializeAsync()
        {
            await this.LoadPostsAsync().ConfigureAwait(true);
            this.SortCache();
        }

        public abstract Task LoadPostsAsync();

        [SuppressMessage(
            "Globalization",
            "CA1308:Normalize strings to uppercase",
            Justification = "Consumer preference.")]
        public virtual IAsyncEnumerable<string> GetCategories()
        {
            var isAdmin = this.IsAdmin();

            var categories = this.Cache
                .Where(p => p.IsPublished || isAdmin)
                .SelectMany(post => post.Categories)
                .Select(cat => cat.ToLowerInvariant())
                .Distinct()
                .ToAsyncEnumerable();

            return categories;
        }

        public virtual IAsyncEnumerable<KeyValuePair<string, int>> GetGroupedCategories()
        {
            var isAdmin = this.IsAdmin();

            var categories = this.Cache
                .Where(p => p.IsPublished || isAdmin)
                .SelectMany(post => post.Categories)
                .GroupBy(c => c)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToDictionary(cat => cat.Category, cat => cat.Count)
                .ToAsyncEnumerable();

            return categories;
        }

        public virtual IAsyncEnumerable<KeyValuePair<string, Tuple<int, int, int>>> GetGroupedDates()
        {
            var isAdmin = this.IsAdmin();

            return this.Cache
                .Where(p => p.IsPublished || isAdmin)
                .Select(post => post.PubDate)
                .GroupBy(date => new { date.Month, date.Year })
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderByDescending(d => d.Date.Year).ThenByDescending(d => d.Date.Month)
                .ToDictionary(
                    keySelector: date => new DateTime(date.Date.Year, date.Date.Month, 1).ToString("MMMM yyyy"),
                    elementSelector: date => new Tuple<int, int, int>(date.Date.Month, date.Date.Year, date.Count))
                .ToAsyncEnumerable();
        }

        public virtual Task<Post?> GetPostById(string id)
        {
            var isAdmin = this.IsAdmin();
            var post = this.Cache.FirstOrDefault(p => p.ID.Equals(id, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(
                post is null || !post.IsVisible() || !isAdmin
                ? null
                : post);
        }

        public virtual Task<Post?> GetPostBySlug(string slug)
        {
            var isAdmin = this.IsAdmin();
            var post = this.Cache.FirstOrDefault(p => p.Slug.Equals(slug, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(
                post is null || !post.IsVisible() || !isAdmin
                ? null
                : post);
        }

        public virtual Task<Post?> GetNextPost(string id)
        {
            var isAdmin = this.IsAdmin();
            var post = this.Cache.FirstOrDefault(p => p.ID.Equals(id, StringComparison.OrdinalIgnoreCase));

            var index = this.Cache.IndexOf(post);
            return Task.FromResult(
                index == this.Cache.Count - 1
                ? null
                : this.Cache.ElementAt(index + 1));
        }

        public virtual Task<Post?> GetPreviousPost(string id)
        {
            var isAdmin = this.IsAdmin();
            var post = this.Cache.FirstOrDefault(p => p.ID.Equals(id, StringComparison.OrdinalIgnoreCase));

            var index = this.Cache.IndexOf(post);
            return Task.FromResult(
                index == 0
                ? null
                : this.Cache.ElementAt(index - 1));
        }

        /// <remarks>Overload for getPosts method to retrieve all posts.</remarks>
        public virtual IAsyncEnumerable<Post> GetPosts()
        {
            var isAdmin = this.IsAdmin();
            return this.Cache.Where(p => p.IsVisible() || isAdmin).ToAsyncEnumerable();
        }

        public virtual IAsyncEnumerable<Post> GetPosts(int count, int skip = 0)
        {
            var isAdmin = this.IsAdmin();

            var posts = this.Cache
                .Where(p => p.IsVisible() || isAdmin)
                .Skip(skip)
                .Take(count)
                .ToAsyncEnumerable();

            return posts;
        }

        public virtual IAsyncEnumerable<Post> GetPostsByCategory(string category)
        {
            var isAdmin = this.IsAdmin();

            var posts = from p in this.Cache
                        where p.IsVisible() || isAdmin
                        where p.Categories.Contains(category, StringComparer.OrdinalIgnoreCase)
                        select p;

            return posts.ToAsyncEnumerable();
        }

        public virtual IAsyncEnumerable<Post> GetPostsByDate(int year, int month)
        {
            var isAdmin = this.IsAdmin();
            var initDate = new DateTime(year, month, 1);
            var endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var posts = from p in this.Cache
                        where p.PubDate >= initDate && p.PubDate <= endDate && (p.IsPublished || isAdmin)
                        select p;

            return posts.ToAsyncEnumerable();
        }

        public virtual async Task<int> GetPostsCountAsync()
        {
            var isAdmin = this.IsAdmin();

            var posts = this.Cache
                .Where(p => p.IsVisible() || isAdmin)
                .ToAsyncEnumerable();

            return await posts.CountAsync();
        }

        public abstract Task<string> SaveFile(byte[] bytes, string fileName, string? suffix = null);

        public abstract Task SavePost(Post post);

        protected bool IsAdmin() => this.ContextAccessor.HttpContext?.User?.Identity.IsAuthenticated ?? false;

        protected void SortCache() => this.Cache.Sort((p1, p2) => p2.PubDate.CompareTo(p1.PubDate));

        #region Private methods

        protected static string CleanFromInvalidChars(string input)
        {
            // ToDo: what we are doing here if we switch the blog from windows to unix system or
            // vice versa? we should remove all invalid chars for both systems

            var regexSearch = Regex.Escape(new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()));
            var r = new Regex($"[{regexSearch}]");
            return r.Replace(input, string.Empty);
        }

        protected static string FormatDateTime(DateTime dateTime)
        {
            const string UTC = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";

            return dateTime.Kind == DateTimeKind.Utc
                ? dateTime.ToString(UTC, CultureInfo.InvariantCulture)
                : dateTime.ToUniversalTime().ToString(UTC, CultureInfo.InvariantCulture);
        }

        protected static void LoadCategories(Post post, XElement doc)
        {
            var categories = doc.Element("categories");
            if (categories is null)
            {
                return;
            }

            post.Categories.Clear();
            categories.Elements("category").Select(node => node.Value).ToList().ForEach(post.Categories.Add);
        }

        protected static void LoadComments(Post post, XElement doc)
        {
            var comments = doc.Element("comments");

            if (comments is null)
            {
                return;
            }

            foreach (var node in comments.Elements("comment"))
            {
                var comment = new Comment
                {
                    ID = ReadAttribute(node, "id"),
                    Author = ReadValue(node, "author"),
                    Email = ReadValue(node, "email"),
                    IsAdmin = bool.Parse(ReadAttribute(node, "isAdmin", "false")),
                    Content = ReadValue(node, "content"),
                    PubDate = DateTime.Parse(ReadValue(node, "date", "2000-01-01"),
                        CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                };

                post.Comments.Add(comment);
            }
        }

        protected static string ReadAttribute(XElement element, XName name, string defaultValue = "") =>
            element.Attribute(name) is null ? defaultValue : element.Attribute(name)?.Value ?? defaultValue;

        protected static string ReadValue(XElement doc, XName name, string defaultValue = "") =>
            doc.Element(name) is null ? defaultValue : doc.Element(name)?.Value ?? defaultValue;

        [SuppressMessage(
            "Globalization",
            "CA1308:Normalize strings to uppercase",
            Justification = "The slug should be lower case.")]
        protected static async Task LoadPost(ConcurrentBag<Post> cb, string file)
        {
            await Task.Run(() =>
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                var doc = XElement.Load(file);

                var post = new Post
                {
                    ID = Path.GetFileNameWithoutExtension(file),
                    Title = ReadValue(doc, "title"),
                    Excerpt = ReadValue(doc, "excerpt"),
                    Content = ReadValue(doc, "content"),
                    Slug = ReadValue(doc, "slug").ToLowerInvariant(),
                    PubDate = DateTime.Parse(ReadValue(doc, "pubDate"), CultureInfo.InvariantCulture,
                        DateTimeStyles.AdjustToUniversal),
                    LastModified = DateTime.Parse(
                        ReadValue(
                            doc,
                            "lastModified",
                            DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                        CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                    IsPublished = bool.Parse(ReadValue(doc, "ispublished", "true")),
                };

                LoadCategories(post, doc);
                LoadComments(post, doc);
                cb.Add(post);
                Console.WriteLine($"[POST LOADED]: {post.Title} - {timer.Elapsed}");
            }).ConfigureAwait(true);
        }

        #endregion
    }
}
