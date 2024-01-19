using System.ComponentModel.DataAnnotations;

namespace PortfolioWeb.Models
{
    public class BlogPost {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public string Summary { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key to Project (nullable)
        public int? ProjectId { get; set; }
        public Project? Project { get; set; }

        // Tags for filtering
        public List<int>? TagIds { get; set; } = new();
        public List<Tag>? Tags { get; set; } = new();
    }

}
