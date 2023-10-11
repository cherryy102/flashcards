using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flashcards.Infrastructure.Persistence.Flashcards.Migrations
{
    public partial class addrepeat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "next_repeat_date",
                schema: "flashcards",
                table: "cards",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "stage",
                schema: "flashcards",
                table: "cards",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "next_repeat_date",
                schema: "flashcards",
                table: "cards");

            migrationBuilder.DropColumn(
                name: "stage",
                schema: "flashcards",
                table: "cards");
        }
    }
}
