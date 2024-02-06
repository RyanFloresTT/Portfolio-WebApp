using BackEndAPI.DTOs;
using BackEndAPI.Middleware;
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
            var blogDtos = await db.Blogs
                .Include(b => b.Tags)
                .Select(b => new BlogDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    Summary = b.Summary,
                    Body = b.Body,
                    CreatedOn = b.CreatedOn,
                    Tags = b.Tags.Select(t => new TagDTO { Id = t.Id, Name = t.Name }).ToList(),
                    AssociatedProjectId = b.AssociatedProjectId
                })
                .ToListAsync();
            return Results.Ok(blogDtos);
        }
        private static async Task<IResult> GetBlogById(int id, ApplicationDbContext db)
        {
            var blogDto = await db.Blogs
                .Where(b => b.Id == id)
                .Include(b => b.Tags)
                .Select(b => new BlogDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    Summary = b.Summary,
                    Body = b.Body,
                    CreatedOn = b.CreatedOn,
                    Tags = b.Tags.Select(t => new TagDTO { Id = t.Id, Name = t.Name }).ToList(),
                    AssociatedProjectId = b.AssociatedProjectId
                })
                .FirstOrDefaultAsync();

            return blogDto != null ? Results.Ok(blogDto) : Results.NotFound();
        }

        private static async Task<IResult> CreateBlog(BlogDTO blogDto, ApplicationDbContext db)
        {
            var blog = new Blog
            {
                Title = blogDto.Title,
                Summary = blogDto.Summary,
                Body = blogDto.Body,
                CreatedOn = blogDto.CreatedOn,
                AssociatedProjectId = blogDto.AssociatedProjectId
            };

            if (blogDto.Tags != null)
            {
                foreach (var tagDto in blogDto.Tags)
                {
                    Tag tag;
                    if (tagDto.Id > 0)
                    {
                        tag = await db.Tags.FindAsync(tagDto.Id);
                    }
                    else
                    {
                        tag = await db.Tags.FirstOrDefaultAsync(t => t.Name == tagDto.Name);
                        if (tag == null)
                        {
                            tag = new Tag { Name = tagDto.Name };
                            db.Tags.Add(tag);
                        }
                    }

                    blog.Tags.Add(tag);
                }
            }

            db.Blogs.Add(blog);
            await db.SaveChangesAsync();
            return Results.Created($"/blogs/{blog.Id}", blog); 
        }



        private static async Task<IResult> UpdateBlog(int id, BlogDTO blogDto, ApplicationDbContext db)
        {
            var blog = await db.Blogs.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null) return Results.NotFound();

            blog.Title = blogDto.Title;
            blog.Summary = blogDto.Summary;
            blog.Body = blogDto.Body;
            blog.AssociatedProjectId = blogDto.AssociatedProjectId;

            blog.Tags.Clear();
            if (blogDto.Tags != null)
            {
                foreach (var tagDto in blogDto.Tags)
                {
                    Tag tag;
                    if (tagDto.Id > 0)
                    {
                        tag = await db.Tags.FindAsync(tagDto.Id);
                    }
                    else
                    {
                        tag = await db.Tags.FirstOrDefaultAsync(t => t.Name == tagDto.Name);
                        if (tag == null)
                        {
                            tag = new Tag { Name = tagDto.Name };
                            db.Tags.Add(tag);
                        }
                    }
                    blog.Tags.Add(tag);
                }
            }

            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        private static async Task<IResult> DeleteBlog(int id, ApplicationDbContext db)
        {
            var blog = await db.Blogs.FindAsync(id);
            if (blog == null) return Results.NotFound();

            db.Blogs.Remove(blog);
            await db.SaveChangesAsync();
            return Results.Ok();
        }
    }

}
