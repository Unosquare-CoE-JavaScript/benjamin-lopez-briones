using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class Many2ManyRel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Authors_Fluent_AuthorAuthor_ID",
                table: "Fluent_BookAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Books_AuthorID",
                table: "Fluent_BookAuthor");

            migrationBuilder.DropIndex(
                name: "IX_Fluent_BookAuthor_Fluent_AuthorAuthor_ID",
                table: "Fluent_BookAuthor");

            migrationBuilder.DropColumn(
                name: "Fluent_AuthorAuthor_ID",
                table: "Fluent_BookAuthor");

            migrationBuilder.AddForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Authors_AuthorID",
                table: "Fluent_BookAuthor",
                column: "AuthorID",
                principalTable: "Fluent_Authors",
                principalColumn: "Author_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Books_BookID",
                table: "Fluent_BookAuthor",
                column: "BookID",
                principalTable: "Fluent_Books",
                principalColumn: "Book_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Authors_AuthorID",
                table: "Fluent_BookAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Books_BookID",
                table: "Fluent_BookAuthor");

            migrationBuilder.AddColumn<int>(
                name: "Fluent_AuthorAuthor_ID",
                table: "Fluent_BookAuthor",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_BookAuthor_Fluent_AuthorAuthor_ID",
                table: "Fluent_BookAuthor",
                column: "Fluent_AuthorAuthor_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Authors_Fluent_AuthorAuthor_ID",
                table: "Fluent_BookAuthor",
                column: "Fluent_AuthorAuthor_ID",
                principalTable: "Fluent_Authors",
                principalColumn: "Author_ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fluent_BookAuthor_Fluent_Books_AuthorID",
                table: "Fluent_BookAuthor",
                column: "AuthorID",
                principalTable: "Fluent_Books",
                principalColumn: "Book_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
