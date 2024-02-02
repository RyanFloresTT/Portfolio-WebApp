using BackEndAPI.Models;

namespace BackEndAPI.DTOs
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? RepoLink { get; set; }
        public List<int> TagIds { get; set; } = new();
        public List<int> AssociatedBlogPostIds { get; set; } = new();
    }
}
