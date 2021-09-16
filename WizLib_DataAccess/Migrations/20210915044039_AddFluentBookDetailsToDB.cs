using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class AddFluentBookDetailsToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthor_Authors_AuthorID",
                table: "BookAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthor_Books_BookID",
                table: "BookAuthor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookAuthor",
                table: "BookAuthor");

            migrationBuilder.RenameTable(
                name: "BookAuthor",
                newName: "BookAuthors");

            migrationBuilder.RenameIndex(
                name: "IX_BookAuthor_AuthorID",
                table: "BookAuthors",
                newName: "IX_BookAuthors_AuthorID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookAuthors",
                table: "BookAuthors",
                columns: new[] { "BookID", "AuthorID" });

            migrationBuilder.CreateTable(
                name: "Fluent_BookDetails",
                columns: table => new
                {
                    BookDetail_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfChapters = table.Column<int>(type: "int", nullable: false),
                    NumberOfPages = table.Column<int>(type: "int", nullable: false),
                    Weigth = table.Column<double>(type: "float", nullable: false),
                    Book_ID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fluent_BookDetails", x => x.BookDetail_ID);
                    table.ForeignKey(
                        name: "FK_Fluent_BookDetails_Books_Book_ID",
                        column: x => x.Book_ID,
                        principalTable: "Books",
                        principalColumn: "Book_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_BookDetails_Book_ID",
                table: "Fluent_BookDetails",
                column: "Book_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthors_Authors_AuthorID",
                table: "BookAuthors",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "Author_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthors_Books_BookID",
                table: "BookAuthors",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "Book_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthors_Authors_AuthorID",
                table: "BookAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthors_Books_BookID",
                table: "BookAuthors");

            migrationBuilder.DropTable(
                name: "Fluent_BookDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookAuthors",
                table: "BookAuthors");

            migrationBuilder.RenameTable(
                name: "BookAuthors",
                newName: "BookAuthor");

            migrationBuilder.RenameIndex(
                name: "IX_BookAuthors_AuthorID",
                table: "BookAuthor",
                newName: "IX_BookAuthor_AuthorID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookAuthor",
                table: "BookAuthor",
                columns: new[] { "BookID", "AuthorID" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthor_Authors_AuthorID",
                table: "BookAuthor",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "Author_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthor_Books_BookID",
                table: "BookAuthor",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "Book_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
