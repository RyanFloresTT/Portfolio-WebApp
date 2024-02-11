using BackEndAPI.DTOs;
using BackEndAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndAPI.Endpoints
{
    public static class TagEndpoints
    {
        public static void MapTagEndpoints(WebApplication app)
        {
            app.MapGet("/tags", GetTags).AllowAnonymous();
            app.MapGet("/tags/{id}", GetTagById).AllowAnonymous();
            app.MapPost("/tags", CreateTag).RequireAuthorization();
            app.MapPut("/tags/{id}", UpdateTag).RequireAuthorization();
            app.MapDelete("/tags/{id}", DeleteTag).RequireAuthorization();
        }

        private static async Task<IResult> GetTags(ApplicationDbContext db)
        {
            var tags = await db.Tags.ToListAsync();
            return tags != null ? Results.Ok(tags) : Results.NotFound($"No Tags Found.");
        }

        private static async Task<IResult> GetTagById(int id, ApplicationDbContext db)
        {
            var tag = await db.Tags.SingleOrDefaultAsync(b => b.Id == id);

            return tag != null ? Results.Ok(tag) : Results.NotFound($"Tag with ID {id} not found.");
        }

        private static async Task<IResult> CreateTag(ApplicationDbContext db, TagDTO dto)
        {
            var tag = new Tag { Name = dto.Name };
            db.Tags.Add(tag);
            await db.SaveChangesAsync();
            return Results.Created($"/tags/{tag.Id}", tag);
        }

        private static async Task<IResult> UpdateTag(int id, ApplicationDbContext db, TagDTO tagDto)
        {
            var tag = await db.Tags.SingleAsync(b => b.Id == id);
            if (tag == null)
            {
                return Results.NotFound($"Tag with ID {id} not found.");
            }

            tag.Name = tagDto.Name;
            await db.SaveChangesAsync();
            return Results.Ok(tag);
        }

        private static async Task<IResult> DeleteTag(int id, ApplicationDbContext db)
        {
            var tag = await db.Tags.FindAsync(id);
            if (tag == null)
            {
                return Results.NotFound($"Tag with ID {id} not found.");
            }

            db.Tags.Remove(tag);
            await db.SaveChangesAsync();
            return Results.Ok($"Tag with ID {id} deleted.");
        }
    }
}
