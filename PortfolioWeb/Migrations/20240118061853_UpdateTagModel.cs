using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioWeb.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTagModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "TagIds",
                table: "Project",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TagIds",
                table: "BlogPost",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagIds",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "TagIds",
                table: "BlogPost");

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
    }
}
