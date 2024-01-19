using System.ComponentModel.DataAnnotations;

namespace PortfolioWeb.Models
{
    public class Project {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public string Summary { get; set; }
        [Required]
        public string RepoLink { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for associated blog posts
        public List<BlogPost> BlogPosts { get; set; } = new();

        // Tags for filtering
        public List<int>? TagIds { get; set; } = new();
        public List<Tag>? Tags { get; set; } = new();
    }

}
