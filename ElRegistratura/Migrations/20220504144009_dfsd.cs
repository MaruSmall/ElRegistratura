using Microsoft.EntityFrameworkCore.Migrations;

namespace ElRegistratura.Migrations
{
    public partial class dfsd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sex_SexId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "FIO",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SexId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sex_SexId",
                table: "AspNetUsers",
                column: "SexId",
                principalTable: "Sex",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sex_SexId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FIO",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "SexId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sex_SexId",
                table: "AspNetUsers",
                column: "SexId",
                principalTable: "Sex",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
