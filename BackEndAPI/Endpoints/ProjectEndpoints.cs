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
                    TagIds = p.Tags.Select(t => t.Id).ToList()
                })
                .ToListAsync();
            return Results.Ok(projectDtos);
        }


        private static async Task<IResult> GetProjectById(int id, ApplicationDbContext db)
        {
            var projectDto = await db.Projects
                .Where(p => p.Id == id)
                .Include(p => p.Tags)
                .Select(p => new ProjectDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Summary = p.Summary,
                    Body = p.Body,
                    CreatedOn = p.CreatedOn,
                    RepoLink = p.RepoLink,
                    TagIds = p.Tags.Select(t => t.Id).ToList()
                })
                .FirstOrDefaultAsync();

            return projectDto != null ? Results.Ok(projectDto) : Results.NotFound();
        }



        private static async Task<IResult> CreateProject(ProjectDTO projectDto, ApplicationDbContext db)
        {
            var project = new Project
            {
                Title = projectDto.Title,
                Summary = projectDto.Summary,
                Body = projectDto.Body,
                CreatedOn = projectDto.CreatedOn,
                RepoLink = projectDto.RepoLink
            };
            db.Projects.Add(project);
            await db.SaveChangesAsync();
            return Results.Created($"/projects/{project.Id}", project);
        }



        private static async Task<IResult> UpdateProject(int id, ProjectDTO projectDTO, ApplicationDbContext db)
        {
            var project = await db.Projects.Include(p => p.Tags).FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return Results.NotFound($"Project with ID {id} not found.");
            }
            project.Title = projectDTO.Title;
            project.Summary = projectDTO.Summary;
            project.Body = projectDTO.Body;
            project.RepoLink = projectDTO.RepoLink;
            if (projectDTO.TagIds != null)
            {
                project.Tags.Clear();
                foreach (var tagId in projectDTO.TagIds)
                {
                    var tag = await db.Tags.FindAsync(tagId);
                    if (tag != null)
                    {
                        project.Tags.Add(tag);
                    }
                }
            }

            await db.SaveChangesAsync();
            return Results.NoContent();
        }



        private static async Task<IResult> DeleteProject(int id, ApplicationDbContext db)
        {
            var project = await db.Projects.FindAsync(id);
            if (project == null) return Results.NotFound();

            db.Projects.Remove(project);
            await db.SaveChangesAsync();
            return Results.Ok();
        }
    }

}
