using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Vue.Backend.Sut.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Blogs",
            columns: table => new
            {
                BlogId = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Url = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Blogs", x => x.BlogId);
            });

        migrationBuilder.CreateTable(
            name: "Posts",
            columns: table => new
            {
                PostId = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Title = table.Column<string>(type: "text", nullable: false),
                Content = table.Column<string>(type: "text", nullable: false),
                BlogId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Posts", x => x.PostId);
                table.ForeignKey(
                    name: "FK_Posts_Blogs_BlogId",
                    column: x => x.BlogId,
                    principalTable: "Blogs",
                    principalColumn: "BlogId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Posts_BlogId",
            table: "Posts",
            column: "BlogId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Posts");

        migrationBuilder.DropTable(
            name: "Blogs");
    }
}
