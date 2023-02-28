using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parent_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.id);
                    table.ForeignKey(
                        name: "FK_Category_Category_parent_id",
                        column: x => x.parent_id,
                        principalTable: "Category",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PostStatus",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    name = table.Column<string>(type: "nvarchar(64)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostStatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Sex",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    name = table.Column<string>(type: "nvarchar(64)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sex", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(128)", nullable: false),
                    display_name = table.Column<string>(type: "nvarchar(128)", nullable: false),
                    sex_id = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_Sex_sex_id",
                        column: x => x.sex_id,
                        principalTable: "Sex",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    author_id = table.Column<string>(type: "varchar(128)", nullable: false),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    published_date = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    status_id = table.Column<byte>(type: "tinyint", nullable: true),
                    title = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.id);
                    table.ForeignKey(
                        name: "FK_Post_PostStatus_status_id",
                        column: x => x.status_id,
                        principalTable: "PostStatus",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Post_Post_parent_id",
                        column: x => x.parent_id,
                        principalTable: "Post",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Post_User_author_id",
                        column: x => x.author_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    author_id = table.Column<string>(type: "varchar(128)", nullable: false),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    posted_date = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    post_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.id);
                    table.ForeignKey(
                        name: "FK_Comment_Comment_parent_id",
                        column: x => x.parent_id,
                        principalTable: "Comment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Comment_Post_post_id",
                        column: x => x.post_id,
                        principalTable: "Post",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Comment_User_author_id",
                        column: x => x.author_id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostCategory",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false),
                    post_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategory", x => new { x.category_id, x.post_id });
                    table.ForeignKey(
                        name: "FK_PostCategory_Category_category_id",
                        column: x => x.category_id,
                        principalTable: "Category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostCategory_Post_post_id",
                        column: x => x.post_id,
                        principalTable: "Post",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "int", nullable: false),
                    post_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => new { x.tag_id, x.post_id });
                    table.ForeignKey(
                        name: "FK_PostTag_Post_post_id",
                        column: x => x.post_id,
                        principalTable: "Post",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTag_Tag_tag_id",
                        column: x => x.tag_id,
                        principalTable: "Tag",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_parent_id",
                table: "Category",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_author_id",
                table: "Comment",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_parent_id",
                table: "Comment",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_post_id",
                table: "Comment",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_Post_author_id",
                table: "Post",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_Post_parent_id",
                table: "Post",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_Post_status_id",
                table: "Post",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostCategory_post_id",
                table: "PostCategory",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_post_id",
                table: "PostTag",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_sex_id",
                table: "User",
                column: "sex_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "PostCategory");

            migrationBuilder.DropTable(
                name: "PostTag");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "PostStatus");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Sex");
        }
    }
}
