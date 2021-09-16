using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class AddedOneToOneRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fluent_BookDetails_Books_Book_ID",
                table: "Fluent_BookDetails");

            migrationBuilder.DropIndex(
                name: "IX_Fluent_BookDetails_Book_ID",
                table: "Fluent_BookDetails");

            migrationBuilder.DropColumn(
                name: "Book_ID",
                table: "Fluent_BookDetails");

            migrationBuilder.AddColumn<int>(
                name: "BookDetailID",
                table: "Fluent_Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_Books_BookDetailID",
                table: "Fluent_Books",
                column: "BookDetailID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fluent_Books_Fluent_BookDetails_BookDetailID",
                table: "Fluent_Books",
                column: "BookDetailID",
                principalTable: "Fluent_BookDetails",
                principalColumn: "BookDetail_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fluent_Books_Fluent_BookDetails_BookDetailID",
                table: "Fluent_Books");

            migrationBuilder.DropIndex(
                name: "IX_Fluent_Books_BookDetailID",
                table: "Fluent_Books");

            migrationBuilder.DropColumn(
                name: "BookDetailID",
                table: "Fluent_Books");

            migrationBuilder.AddColumn<int>(
                name: "Book_ID",
                table: "Fluent_BookDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fluent_BookDetails_Book_ID",
                table: "Fluent_BookDetails",
                column: "Book_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Fluent_BookDetails_Books_Book_ID",
                table: "Fluent_BookDetails",
                column: "Book_ID",
                principalTable: "Books",
                principalColumn: "Book_ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
