using System.ComponentModel.DataAnnotations;

namespace BackEndAPI.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<BlogPost>? Blogs { get; set; } = new List<BlogPost>();
        public List<Project>? Projects { get; set; } = new List<Project>();
    }

}
