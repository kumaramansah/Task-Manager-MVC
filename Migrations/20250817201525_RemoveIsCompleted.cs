﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_Manager.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsCompleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "TaskItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "TaskItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
