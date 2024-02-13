using System.ComponentModel.DataAnnotations;

namespace BackEndAPI.DTOs
{
    public class TagDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
