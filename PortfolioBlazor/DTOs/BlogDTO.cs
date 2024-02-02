namespace PortfolioBlazor.DTOs
{
    public class BlogDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<int> TagIds { get; set; } = new();
        public int? AssociatedProjectId { get; set; }
    }
}
