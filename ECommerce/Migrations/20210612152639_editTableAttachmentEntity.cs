using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class editTableAttachmentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Attachments_AttachmentId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_AttachmentId",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Attachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_ProductId",
                table: "Attachments",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Products_ProductId",
                table: "Attachments",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Products_ProductId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_ProductId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Attachments");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Products_AttachmentId",
                table: "Products",
                column: "AttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Attachments_AttachmentId",
                table: "Products",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
