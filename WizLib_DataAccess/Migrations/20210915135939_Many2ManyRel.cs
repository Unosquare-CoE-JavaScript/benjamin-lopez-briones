using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class Many2ManyRel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fluent_BookAuthor",
                columns: table => new
                {
                    BookID = table.Column<int>(type: "int", nullable: false),
                    AuthorID = table.Column<int>(type: "int", nullable: false),
                    Fluent_AuthorAuthor_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fluent_BookAuthor", x => new { x.BookID, x.AuthorID });
                    table.ForeignKey(
                        name: "FK_Fluent_BookAuthor_Fluent_Authors_Fluent_AuthorAuthor_ID",
                        column: x => x.Fluent_AuthorAuthor_ID,
                        principalTable: "Fluent_Authors",
                        principalColumn: "Author_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fluent_BookAuthor_Fluent_Books_AuthorID",
                        column: x => x.AuthorID,
                        principalTable: "Fluent_Books",
                        principalColumn: "Book_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_BookAuthor_AuthorID",
                table: "Fluent_BookAuthor",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_BookAuthor_Fluent_AuthorAuthor_ID",
                table: "Fluent_BookAuthor",
                column: "Fluent_AuthorAuthor_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fluent_BookAuthor");
        }
    }
}
