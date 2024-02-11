using BackEndAPI.DTOs;
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
            var projects = await db.Projects.ToListAsync();
            return Results.Ok(projects);
        }

        private static async Task<IResult> GetProjectById(int id, ApplicationDbContext db)
        {
            var project = await db.Projects.SingleOrDefaultAsync(x => x.Id == id);
            return Results.Ok(project);
        }


        // Create a new project with associated tags and blog posts
        private static async Task<IResult> CreateProject(ProjectDTO dto, ApplicationDbContext db)
        {
            var project = new Project
            {
                Title = dto.Title,
                Summary = dto.Summary,
                Body = dto.Body,
                RepoLink = dto.RepoLink,
            };

            // Handle Tags
            if (dto.TagIds.Count > 0)
            {
                var tags = await db.Tags.Where(t => dto.TagIds.Contains(t.Id)).ToListAsync();
                foreach (var tag in tags)
                {
                    project.Tags.Add(tag);
                }
            }

            // Handle Associated Blog Posts
            // This assumes blog posts are identified by ID and already exist
            if (dto.BlogPostIds.Count > 0)
            {
                var blogs = await db.Blogs.Where(t => dto.BlogPostIds.Contains(t.Id)).ToListAsync();
                foreach (var blog in blogs)
                {
                    project.AssociatedBlogPosts.Add(blog);
                }
            }

            db.Projects.Add(project);
            await db.SaveChangesAsync();
            return Results.Created($"/projects/{project.Id}", project);
        }

        // Update an existing project
        private static async Task<IResult> UpdateProject(int id, ProjectDTO dto, ApplicationDbContext db)
        {
            var project = await db.Projects.SingleAsync(t => t.Id == id);

            if (project == null)
            {
                return Results.NotFound($"Project with ID {id} not found.");
            }

            project.Title = dto.Title;
            project.Summary = dto.Summary;
            project.Body = dto.Body;
            project.RepoLink = dto.RepoLink;

            project.Tags?.Clear();

            if (dto.TagIds.Count > 0)
            {
                var tags = await db.Tags.Where(t => dto.TagIds.Contains(t.Id)).ToListAsync();
                foreach (var tag in tags)
                {
                    project.Tags.Add(tag);
                }
            }

            project.AssociatedBlogPosts?.Clear();
            if (dto.BlogPostIds.Count > 0)
            {
                var blogs = await db.Blogs.Where(t => dto.BlogPostIds.Contains(t.Id)).ToListAsync();
                foreach (var blog in blogs)
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
            return Results.NoContent();
        }
    }
}
