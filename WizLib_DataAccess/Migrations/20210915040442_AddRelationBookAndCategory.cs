using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class AddRelationBookAndCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category_ID",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Books_Category_ID",
                table: "Books",
                column: "Category_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Categories_Category_ID",
                table: "Books",
                column: "Category_ID",
                principalTable: "Categories",
                principalColumn: "Category_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Categories_Category_ID",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_Category_ID",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Category_ID",
                table: "Books");
        }
    }
}
