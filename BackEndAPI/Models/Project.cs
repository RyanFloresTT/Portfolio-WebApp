namespace BackEndAPI.Models
{   
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? RepoLink { get; set; }
        public List<Tag> Tags { get; set; } = new();
        public List<Blog>? AssociatedBlogPosts { get; set; } = new();
    }

}
