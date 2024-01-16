namespace PortfolioWeb.Models
{
    public class BlogPost {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public string Summary { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key to Project (nullable)
        public int? ProjectId { get; set; }
        public Project Project { get; set; }

        // Tags for filtering
        public List<string> Tags { get; set; } = new List<string>();
    }

}
