namespace PortfolioWeb.Models
{
    public class Project {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string Summary { get; set; }
        public string RepoLink { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for associated blog posts
        public List<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();

        // Tags for filtering
        public List<string> Tags { get; set; } = new List<string>();
    }

}
