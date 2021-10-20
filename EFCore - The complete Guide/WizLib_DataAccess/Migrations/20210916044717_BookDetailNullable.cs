using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class BookDetailNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookDetail_BookDetailID",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookDetailID",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "BookDetailID",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookDetailID",
                table: "Books",
                column: "BookDetailID",
                unique: true,
                filter: "[BookDetailID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookDetail_BookDetailID",
                table: "Books",
                column: "BookDetailID",
                principalTable: "BookDetail",
                principalColumn: "BookDetailID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookDetail_BookDetailID",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookDetailID",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "BookDetailID",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookDetailID",
                table: "Books",
                column: "BookDetailID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookDetail_BookDetailID",
                table: "Books",
                column: "BookDetailID",
                principalTable: "BookDetail",
                principalColumn: "BookDetailID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
