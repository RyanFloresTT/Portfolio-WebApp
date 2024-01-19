using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBlogProjectModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlogPostId",
                table: "Tag",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Tag",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_BlogPostId",
                table: "Tag",
                column: "BlogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ProjectId",
                table: "Tag",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_BlogPost_BlogPostId",
                table: "Tag",
                column: "BlogPostId",
                principalTable: "BlogPost",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Project_ProjectId",
                table: "Tag",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_BlogPost_BlogPostId",
                table: "Tag");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Project_ProjectId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_BlogPostId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_ProjectId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "BlogPostId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Tag");
        }
    }
}
