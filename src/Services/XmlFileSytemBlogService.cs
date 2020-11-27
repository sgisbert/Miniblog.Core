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
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class XmlFileSytemBlogService : XmlBlogServiceBase
    {
        private const string FILES = "files";

        private const string POSTS = "Posts";

        private readonly IHttpContextAccessor contextAccessor;

        private readonly string folder;

        [SuppressMessage(
                "Usage",
                "SecurityIntelliSenseCS:MS Security rules violation",
                Justification = "Path not derived from user input.")]
        public XmlFileSytemBlogService(IWebHostEnvironment env, IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            if (env is null)
            {
                throw new ArgumentNullException(nameof(env));
            }

            this.folder = Path.Combine(env.WebRootPath, POSTS);
            this.contextAccessor = contextAccessor;

            this.InitializeAsync().ConfigureAwait(true);
        }

        public override Task DeletePost(Post post)
        {
            if (post is null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            var filePath = this.GetFilePath(post);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if (this.Cache.Contains(post))
            {
                this.Cache.Remove(post);
            }

            return Task.CompletedTask;
        }

        [SuppressMessage(
            "Usage",
            "SecurityIntelliSenseCS:MS Security rules violation",
            Justification = "Path not derived from user input.")]
        protected string GetFilePath(Post post) => Path.Combine(this.folder, $"{post.ID}.xml");

        [SuppressMessage(
            "Usage",
            "SecurityIntelliSenseCS:MS Security rules violation",
            Justification = "Caller must review file name.")]
        public override async Task<string> SaveFile(byte[] bytes, string fileName, string? suffix = null)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            suffix = CleanFromInvalidChars(suffix ?? DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture));

            var ext = Path.GetExtension(fileName);
            var name = CleanFromInvalidChars(Path.GetFileNameWithoutExtension(fileName));

            var fileNameWithSuffix = $"{name}_{suffix}{ext}";

            var absolute = Path.Combine(this.folder, FILES, fileNameWithSuffix);
            var dir = Path.GetDirectoryName(absolute);

            Directory.CreateDirectory(dir);
            using (var writer = new FileStream(absolute, FileMode.CreateNew))
            {
                await writer.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
            }

            return $"/{POSTS}/{FILES}/{fileNameWithSuffix}";
        }

        public override async Task SavePost(Post post)
        {
            if (post is null)
            {
                throw new ArgumentNullException(nameof(post));
            }

            var filePath = this.GetFilePath(post);
            post.LastModified = DateTime.UtcNow;

            var doc = new XDocument(
                            new XElement("post",
                                new XElement("title", post.Title),
                                new XElement("slug", post.Slug),
                                new XElement("pubDate", FormatDateTime(post.PubDate)),
                                new XElement("lastModified", FormatDateTime(post.LastModified)),
                                new XElement("excerpt", post.Excerpt),
                                new XElement("content", post.Content),
                                new XElement("ispublished", post.IsPublished),
                                new XElement("categories", string.Empty),
                                new XElement("comments", string.Empty)
                            ));

            var categories = doc.XPathSelectElement("post/categories");
            foreach (var category in post.Categories)
            {
                categories.Add(new XElement("category", category));
            }

            var comments = doc.XPathSelectElement("post/comments");
            foreach (var comment in post.Comments)
            {
                comments.Add(
                    new XElement("comment",
                        new XElement("author", comment.Author),
                        new XElement("email", comment.Email),
                        new XElement("date", FormatDateTime(comment.PubDate)),
                        new XElement("content", comment.Content),
                        new XAttribute("isAdmin", comment.IsAdmin),
                        new XAttribute("id", comment.ID)
                    ));
            }

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                await doc.SaveAsync(fs, SaveOptions.None, CancellationToken.None).ConfigureAwait(false);
            }

            if (!this.Cache.Contains(post))
            {
                this.Cache.Add(post);
                this.SortCache();
            }
        }

        public override async Task LoadPostsAsync()
        {
            if (!Directory.Exists(this.folder))
            {
                Directory.CreateDirectory(this.folder);
            }

            Stopwatch globalTimer = new Stopwatch();
            globalTimer.Start();

            // Add to ConcurrentBag concurrently
            ConcurrentBag<Post> posts = new ConcurrentBag<Post>();
            List<Task> RunningTasks = new List<Task>();

            // Can this be done in parallel to speed it up?
            foreach (var file in Directory.EnumerateFiles(this.folder, "*.xml", SearchOption.TopDirectoryOnly))
            {
                RunningTasks.Add(LoadPost(posts, file));
            }
            // Wait until all te posts are loaded
            await Task.WhenAll(RunningTasks).ConfigureAwait(true);
            this.Cache = posts.ToList();
            Console.WriteLine($"[ALL POSTS LOADED]: {globalTimer.Elapsed}");
        }
    }
}
