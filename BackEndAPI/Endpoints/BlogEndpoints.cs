using BackEndAPI.DTOs;
using BackEndAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndAPI.Endpoints
{
    public static class BlogEndpoints
    {
        public static void MapBlogEndpoints(WebApplication app)
        {
            app.MapGet("/blogs", GetBlogs).AllowAnonymous();
            app.MapGet("/blogs/{id}", GetBlogById).AllowAnonymous();
            app.MapPost("/blogs", CreateBlog).RequireAuthorization();
            app.MapPut("/blogs/{id}", UpdateBlog).RequireAuthorization();
            app.MapDelete("/blogs/{id}", DeleteBlog).RequireAuthorization();
        }

        private static async Task<IResult> GetBlogs(ApplicationDbContext db)
        {
            var blogPosts = await db.Blogs.Include(b => b.Tags).ToListAsync();
            return Results.Ok(blogPosts);
        }

        private static async Task<IResult> GetBlogById(int id, ApplicationDbContext db)
        {
            var blogPost = await db.Blogs.SingleOrDefaultAsync(b => b.Id == id);
            return Results.Ok(blogPost);
        }

        private static async Task<IResult> CreateBlog(BlogPostDTO dto, ApplicationDbContext db)
        {
            var tags = await db.Tags.Where(t => dto.TagIds.Contains(t.Id)).ToListAsync();
            var project = await db.Projects.Where(p => dto.ProjectId == p.Id).ToListAsync();
            var blogPost = new BlogPost
            {
                Title = dto.Title,
                Body = dto.Body,
                Summary = dto.Summary,
                ProjectId = dto.ProjectId,
                Project = project[0],
                Tags = tags,
            };

            db.Blogs.Add(blogPost);
            await db.SaveChangesAsync();
            return Results.Created($"/blogs/{blogPost.Id}", blogPost);
        }


        private static async Task<IResult> UpdateBlog(int id, BlogPostDTO dto, ApplicationDbContext db)
        {
            if (dto.ProjectId.HasValue && !await db.Projects.AnyAsync(p => p.Id == dto.ProjectId.Value))
            {
                return Results.NotFound($"Project with ID {dto.ProjectId} not found.");
            }

            var blogPost = await db.Blogs.SingleAsync(b => b.Id == id);

            if (blogPost == null)
            {
                return Results.NotFound($"Project with ID {id} not found.");
            }

            blogPost.Title = dto.Title;
            blogPost.Body = dto.Body;
            blogPost.Summary = dto.Summary;
            blogPost.ProjectId = dto.ProjectId;


            if (dto.TagIds.Any())
            {
                var existingTagIds = await db.Tags.Where(t => dto.TagIds.Contains(t.Id)).Select(t => t.Id).ToListAsync();
                if (dto.TagIds.Count != existingTagIds.Count)
                {
                    return Results.NotFound("One or more tags not found.");
                }

                blogPost.Tags?.Clear();
                var tagsToAdd = await db.Tags.Where(t => existingTagIds.Contains(t.Id)).ToListAsync();
                foreach (var tag in tagsToAdd)
                {
                    blogPost.Tags.Add(tag);
                }
            }


            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        private static async Task<IResult> DeleteBlog(int id, ApplicationDbContext db)
        {
            var blog = await db.Blogs.FindAsync(id);
            if (blog == null)
            {
                return Results.NotFound($"Project with ID {id} not found.");
            }

            db.Blogs.Remove(blog);
            await db.SaveChangesAsync();
            return Results.Ok($"Project with ID {id} has been deleted.");
        }
    }
}
