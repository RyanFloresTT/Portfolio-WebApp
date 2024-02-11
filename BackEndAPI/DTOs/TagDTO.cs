using System.ComponentModel.DataAnnotations;

namespace BackEndAPI.DTOs
{
    public class TagDTO
    {

        [Required]
        public string Name { get; set; }
    }
}
