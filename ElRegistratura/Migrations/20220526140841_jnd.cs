using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ElRegistratura.Migrations
{
    public partial class jnd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Break",
                table: "Schedules",
                newName: "BreakStart");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "BreakFinish",
                table: "Schedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakFinish",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "BreakStart",
                table: "Schedules",
                newName: "Break");
        }
    }
}
