using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeManager.IdP.Migrations
{
    public partial class deleteRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshTokens",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshTokens",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
