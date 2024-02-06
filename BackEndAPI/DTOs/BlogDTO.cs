namespace BackEndAPI.DTOs
{
    public class BlogDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<TagDTO> Tags { get; set; } = new();
        public int? AssociatedProjectId { get; set; }
    }
}
