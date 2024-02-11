﻿using System.ComponentModel.DataAnnotations;

namespace BackEndAPI.Models
{   
    public class Project
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Summary { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? RepoLink { get; set; }
        public List<Tag> Tags { get; set; } = new();
        public List<BlogPost> AssociatedBlogPosts { get; set; } = new();
    }

}
