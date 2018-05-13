using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BierAlyzerWeb.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drink",
                columns: table => new
                {
                    DrinkId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    CreatedString = table.Column<string>(nullable: true),
                    ModifiedString = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Percentage = table.Column<double>(nullable: false),
                    Visible = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drink", x => x.DrinkId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    CreatedString = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Hash = table.Column<string>(nullable: true),
                    LastLoginString = table.Column<string>(nullable: true),
                    Mail = table.Column<string>(nullable: true),
                    ModifiedString = table.Column<string>(nullable: true),
                    Origin = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    CreatedString = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EndString = table.Column<string>(nullable: true),
                    ModifiedString = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OwnerId = table.Column<Guid>(nullable: false),
                    StartString = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Event_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrinkEntry",
                columns: table => new
                {
                    EntryId = table.Column<Guid>(nullable: false),
                    DrinkId = table.Column<Guid>(nullable: false),
                    EventId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrinkEntry", x => x.EntryId);
                    table.ForeignKey(
                        name: "FK_DrinkEntry_Drink_DrinkId",
                        column: x => x.DrinkId,
                        principalTable: "Drink",
                        principalColumn: "DrinkId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrinkEntry_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrinkEntry_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserEvent",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    EventId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEvent", x => new { x.UserId, x.EventId });
                    table.UniqueConstraint("AK_UserEvent_EventId_UserId", x => new { x.EventId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserEvent_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEvent_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrinkEntry_DrinkId",
                table: "DrinkEntry",
                column: "DrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkEntry_EventId",
                table: "DrinkEntry",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkEntry_UserId",
                table: "DrinkEntry",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_OwnerId",
                table: "Event",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrinkEntry");

            migrationBuilder.DropTable(
                name: "UserEvent");

            migrationBuilder.DropTable(
                name: "Drink");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
