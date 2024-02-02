using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEndAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDTOs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Projects_AssociatedProjectId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_AssociatedProjectId",
                table: "Blogs");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Blogs",
                type: "INTEGER",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Projects_ProjectId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_ProjectId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Blogs");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_AssociatedProjectId",
                table: "Blogs",
                column: "AssociatedProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Projects_AssociatedProjectId",
                table: "Blogs",
                column: "AssociatedProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
