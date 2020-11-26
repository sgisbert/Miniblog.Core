namespace Miniblog.Core.Services
{
    using Miniblog.Core.Models;

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBlogService
    {
        Task DeletePost(Post post);

        IAsyncEnumerable<string> GetCategories();

        IAsyncEnumerable<KeyValuePair<string,int>> GetGroupedCategories();

        IAsyncEnumerable<KeyValuePair<string, Tuple<int, int, int>>> GetGroupedDates();

        Task<Post?> GetPostById(string id);

        Task<Post?> GetPostBySlug(string slug);

        Task<int> GetPostsCountAsync();

        IAsyncEnumerable<Post> GetPosts();

        IAsyncEnumerable<Post> GetPosts(int count, int skip = 0);

        IAsyncEnumerable<Post> GetPostsByCategory(string category);

        IAsyncEnumerable<Post> GetPostsByDate(int year, int month);

        Task<string> SaveFile(byte[] bytes, string fileName, string? suffix = null);

        Task SavePost(Post post);
    }
}
