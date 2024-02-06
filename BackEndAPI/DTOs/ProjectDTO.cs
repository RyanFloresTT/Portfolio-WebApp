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
        public List<TagDTO> Tags { get; set; } = new();
        public List<BlogDTO> AssociatedBlogPosts { get; set; } = new();
    }
}
