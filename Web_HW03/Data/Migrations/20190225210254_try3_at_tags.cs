using Microsoft.EntityFrameworkCore.Migrations;

namespace Web_HW03.Data.Migrations
{
    public partial class try3_at_tags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_Tags_TagId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_BlogPosts_BlogPostId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_BlogPostId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_TagId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "BlogPostId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "BlogPosts");

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => new { x.PostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PostTag_BlogPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagId",
                table: "PostTag",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostTag");

            migrationBuilder.AddColumn<int>(
                name: "BlogPostId",
                table: "Tags",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "BlogPosts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_BlogPostId",
                table: "Tags",
                column: "BlogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_TagId",
                table: "BlogPosts",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_Tags_TagId",
                table: "BlogPosts",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_BlogPosts_BlogPostId",
                table: "Tags",
                column: "BlogPostId",
                principalTable: "BlogPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
