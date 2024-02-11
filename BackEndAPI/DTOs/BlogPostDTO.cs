using System.ComponentModel.DataAnnotations;

namespace BackEndAPI.DTOs
{
    public class BlogPostDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public string Summary { get; set; }
        public int? ProjectId { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();
    }
}
