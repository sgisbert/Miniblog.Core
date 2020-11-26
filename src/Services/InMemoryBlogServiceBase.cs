namespace Miniblog.Core.Services
{
    using Microsoft.AspNetCore.Http;

    using Miniblog.Core.Models;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class InMemoryBlogServiceBase : IBlogService
    {
        protected InMemoryBlogServiceBase(IHttpContextAccessor contextAccessor) => this.ContextAccessor = contextAccessor;

        protected List<Post> Cache { get; } = new List<Post>();

        protected IHttpContextAccessor ContextAccessor { get; }

        public abstract Task DeletePost(Post post);

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

        public IAsyncEnumerable<KeyValuePair<string, int>> GetGroupedCategories()
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

        public IAsyncEnumerable<KeyValuePair<string, Tuple<int, int, int>>> GetGroupedDates()
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

        public IAsyncEnumerable<Post> GetPostsByDate(int year, int month)
        {
            var isAdmin = this.IsAdmin();
            var initDate = new DateTime(year, month, 1);
            var endDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            var posts = from p in this.Cache
                        where p.PubDate >= initDate && p.PubDate <= endDate && (p.IsPublished || isAdmin)
                        select p;

            return posts.ToAsyncEnumerable();
        }

        public async Task<int> GetPostsCountAsync()
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
    }
}
