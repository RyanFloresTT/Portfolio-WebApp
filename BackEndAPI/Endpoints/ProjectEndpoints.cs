using BackEndAPI.DTOs;
using BackEndAPI.Middleware;
using BackEndAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndAPI.Endpoints
{
    public static class ProjectEndpoints
    {
        public static void MapProjectEndpoints(WebApplication app)
        {
            app.MapGet("/projects", GetProjects).AllowAnonymous();
            app.MapGet("/projects/{id}", GetProjectById).AllowAnonymous();
            app.MapPost("/projects", CreateProject).RequireAuthorization();
            app.MapPut("/projects/{id}", UpdateProject).RequireAuthorization();
            app.MapDelete("/projects/{id}", DeleteProject).RequireAuthorization();
        }

        private static async Task<IResult> GetProjects(ApplicationDbContext db)
        {
            var projectDtos = await db.Projects
                .Include(p => p.Tags)
                .Select(p => new ProjectDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Summary = p.Summary,
                    Body = p.Body,
                    CreatedOn = p.CreatedOn,
                    RepoLink = p.RepoLink,
                    Tags = p.Tags.Select(t => new TagDTO { Id = t.Id, Name = t.Name }).ToList(),
                    AssociatedBlogPosts = p.AssociatedBlogPosts.Select(t => new BlogDTO { Id = t.Id, AssociatedProjectId = t.Id }).ToList(),
                })
                .ToListAsync();
            return Results.Ok(projectDtos);
        }

        private static async Task<IResult> GetProjectById(int id, ApplicationDbContext db)
        {
            var projectDto = await db.Projects
                .Where(p => p.Id == id)
                .Include(p => p.Tags)
                .Include(p => p.AssociatedBlogPosts)
                    .ThenInclude(blog => blog.Tags) 
                .Select(p => new ProjectDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Summary = p.Summary,
                    Body = p.Body,
                    CreatedOn = p.CreatedOn,
                    RepoLink = p.RepoLink,
                    Tags = p.Tags.Select(t => new TagDTO
                    {
                        Id = t.Id,
                        Name = t.Name
                    }).ToList(),
                    AssociatedBlogPosts = p.AssociatedBlogPosts.Select(bp => new BlogDTO
                    {
                        Id = bp.Id,
                        Title = bp.Title,
                        Summary = bp.Summary,
                        Body = bp.Body,
                        CreatedOn = bp.CreatedOn,
                        Tags = bp.Tags.Select(t => new TagDTO
                        {
                            Id = t.Id,
                            Name = t.Name
                        }).ToList(),
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return projectDto != null ? Results.Ok(projectDto) : Results.NotFound();
        }


        // Create a new project with associated tags and blog posts
        private static async Task<IResult> CreateProject(ProjectDTO projectDto, ApplicationDbContext db)
        {
            var project = new Project
            {
                Title = projectDto.Title,
                Summary = projectDto.Summary,
                Body = projectDto.Body,
                CreatedOn = projectDto.CreatedOn,
                RepoLink = projectDto.RepoLink,
            };

            // Handle Tags
            foreach (var tagDto in projectDto.Tags)
            {
                var tag = await db.Tags.FirstOrDefaultAsync(t => t.Name == tagDto.Name) ?? new Tag { Name = tagDto.Name };
                project.Tags.Add(tag);
            }

            // Handle Associated Blog Posts
            // This assumes blog posts are identified by ID and already exist
            foreach (var blogId in projectDto.AssociatedBlogPosts)
            {
                var blog = await db.Blogs.FindAsync(blogId);
                if (blog != null)
                {
                    project.AssociatedBlogPosts.Add(blog);
                }
            }

            db.Projects.Add(project);
            await db.SaveChangesAsync();
            return Results.Created($"/projects/{project.Id}", project);
        }

        // Update an existing project
        private static async Task<IResult> UpdateProject(int id, ProjectDTO projectDto, ApplicationDbContext db)
        {
            var project = await db.Projects
                .Include(p => p.Tags)
                .Include(p => p.AssociatedBlogPosts)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return Results.NotFound($"Project with ID {id} not found.");
            }

            project.Title = projectDto.Title;
            project.Summary = projectDto.Summary;
            project.Body = projectDto.Body;
            project.RepoLink = projectDto.RepoLink;

            project.Tags.Clear();
            foreach (var tagDto in projectDto.Tags)
            {
                var tag = await db.Tags.FirstOrDefaultAsync(t => t.Name == tagDto.Name) ?? new Tag { Name = tagDto.Name };
                project.Tags.Add(tag);
            }

            project.AssociatedBlogPosts.Clear();
            foreach (var blogId in projectDto.AssociatedBlogPosts)
            {
                var blog = await db.Blogs.FindAsync(blogId);
                if (blog != null)
                {
                    project.AssociatedBlogPosts.Add(blog);
                }
            }

            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        private static async Task<IResult> DeleteProject(int id, ApplicationDbContext db)
        {
            var project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return Results.NotFound($"Project with ID {id} not found.");
            }

            db.Projects.Remove(project);
            await db.SaveChangesAsync();
            return Results.Ok($"Project with ID {id} has been deleted.");
        }

    }

}
