using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialApplication.Data.Migrations
{
    public partial class column_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecievePushNotification",
                table: "AspNetUsers",
                newName: "ReceivePushNotification");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceivePushNotification",
                table: "AspNetUsers",
                newName: "RecievePushNotification");
        }
    }
}
