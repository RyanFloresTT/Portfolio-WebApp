using System.ComponentModel.DataAnnotations;

namespace BackEndAPI.DTOs
{
    public class ProjectDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Summary { get; set; }
        [Required]
        public string Body { get; set; }
        public string? RepoLink { get; set; }
        public List<int> TagIds { get; set; } = new();
        public List<int> BlogPostIds { get; set; } = new();
    }
}
