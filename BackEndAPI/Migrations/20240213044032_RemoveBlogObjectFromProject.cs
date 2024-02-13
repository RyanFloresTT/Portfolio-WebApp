using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEndAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBlogObjectFromProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Projects_ProjectId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_ProjectId",
                table: "Blogs");

            migrationBuilder.AddColumn<string>(
                name: "BlogPostIds",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlogPostIds",
                table: "Projects");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_ProjectId",
                table: "Blogs",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Projects_ProjectId",
                table: "Blogs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
