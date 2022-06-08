﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ElRegistratura.Migrations
{
    public partial class jn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "Break",
                table: "Schedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Break",
                table: "Schedules");
        }
    }
}
