namespace BackEndAPI.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Blog>? Blogs { get; set; } = new List<Blog>();
        public List<Project>? Projects { get; set; } = new List<Project>();
    }

}
