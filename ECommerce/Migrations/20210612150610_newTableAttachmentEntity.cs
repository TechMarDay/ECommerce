using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Migrations
{
    public partial class newTableAttachmentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttachmentId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Attachments_AttachmentId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Products_AttachmentId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "Products");
        }
    }
}
