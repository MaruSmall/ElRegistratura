using Microsoft.EntityFrameworkCore.Migrations;

namespace ElRegistratura.Migrations
{
    public partial class tel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "AspNetUsers");
        }
    }
}
