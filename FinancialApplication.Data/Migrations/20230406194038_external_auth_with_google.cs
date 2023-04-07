using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialApplication.Data.Migrations
{
    public partial class external_auth_with_google : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ExternalAuthInWithGoogle",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalAuthInWithGoogle",
                table: "AspNetUsers");
        }
    }
}
