using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialApplication.Data.Migrations
{
    public partial class picture_publicId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureId",
                table: "AspNetUsers");
        }
    }
}
