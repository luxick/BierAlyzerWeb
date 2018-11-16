using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BierAlyzer.Api.Migrations
{
    public partial class DrinkOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Drink",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drink_OwnerId",
                table: "Drink",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drink_User_OwnerId",
                table: "Drink",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drink_User_OwnerId",
                table: "Drink");

            migrationBuilder.DropIndex(
                name: "IX_Drink_OwnerId",
                table: "Drink");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Drink");
        }
    }
}
