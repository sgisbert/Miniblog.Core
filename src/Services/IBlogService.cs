namespace Miniblog.Core.Services
{
    using Miniblog.Core.Models;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBlogService
    {
        Task DeletePost(Post post);

        IAsyncEnumerable<string> GetCategories();

        IAsyncEnumerable<KeyValuePair<string,int>> GetGroupedCategories();

        Task<Post?> GetPostById(string id);

        Task<Post?> GetPostBySlug(string slug);

        Task<int> GetPostsCountAsync();

        IAsyncEnumerable<Post> GetPosts();

        IAsyncEnumerable<Post> GetPosts(int count, int skip = 0);

        IAsyncEnumerable<Post> GetPostsByCategory(string category);

        Task<string> SaveFile(byte[] bytes, string fileName, string? suffix = null);

        Task SavePost(Post post);
    }
}
