using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class AAAAA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_tbl_category_Category_ID",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "Category_ID",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_tbl_category_Category_ID",
                table: "Books",
                column: "Category_ID",
                principalTable: "tbl_category",
                principalColumn: "Category_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_tbl_category_Category_ID",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "Category_ID",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_tbl_category_Category_ID",
                table: "Books",
                column: "Category_ID",
                principalTable: "tbl_category",
                principalColumn: "Category_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
