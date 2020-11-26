namespace Miniblog.Core
{
    public class BlogSettings
    {
        public int CommentsCloseAfterDays { get; set; } = 10;

        public PostListView ListView { get; set; } = PostListView.FullPosts;

        public string Owner { get; set; } = "Sergi Gisbert";

        public int PostsPerPage { get; set; } = 4;
    }
}
