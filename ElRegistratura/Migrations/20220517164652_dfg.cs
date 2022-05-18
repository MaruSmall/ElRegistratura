using Microsoft.EntityFrameworkCore.Migrations;

namespace ElRegistratura.Migrations
{
    public partial class dfg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FIOAndClinicName",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CabinetNameAndClinicName",
                table: "Cabinets",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FIOAndClinicName",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "CabinetNameAndClinicName",
                table: "Cabinets");
        }
    }
}
