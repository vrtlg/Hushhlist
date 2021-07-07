using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hushhlist.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HostEvents",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HostId = table.Column<string>(nullable: true),
                    EventTitle = table.Column<string>(maxLength: 255, nullable: false),
                    EventDate = table.Column<DateTime>(nullable: true),
                    EventDescription = table.Column<string>(maxLength: 450, nullable: true),
                    Location = table.Column<string>(nullable: true),
                    EventPassword = table.Column<string>(maxLength: 100, nullable: false),
                    MaxReservable = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostEvents", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientId = table.Column<string>(nullable: false),
                    SenderId = table.Column<string>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: false),
                    GiftId = table.Column<int>(nullable: true),
                    EventId = table.Column<int>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    Read = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "Gifts",
                columns: table => new
                {
                    GiftId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(maxLength: 255, nullable: false),
                    ItemDescription = table.Column<string>(maxLength: 450, nullable: true),
                    URL = table.Column<string>(maxLength: 200, nullable: true),
                    IsAgeRestricted = table.Column<bool>(nullable: false),
                    GuestId = table.Column<string>(nullable: true),
                    IsPriority = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    HostEventEventId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gifts", x => x.GiftId);
                    table.ForeignKey(
                        name: "FK_Gifts_HostEvents_HostEventEventId",
                        column: x => x.HostEventEventId,
                        principalTable: "HostEvents",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    GuestId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 255, nullable: false),
                    LastName = table.Column<string>(maxLength: 255, nullable: false),
                    Username = table.Column<string>(maxLength: 50, nullable: false),
                    LoginMethod = table.Column<string>(maxLength: 100, nullable: false),
                    IsOver18 = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    EventId = table.Column<int>(nullable: false),
                    HostEventEventId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.GuestId);
                    table.ForeignKey(
                        name: "FK_Guests_HostEvents_HostEventEventId",
                        column: x => x.HostEventEventId,
                        principalTable: "HostEvents",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_HostEventEventId",
                table: "Gifts",
                column: "HostEventEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Guests_HostEventEventId",
                table: "Guests",
                column: "HostEventEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gifts");

            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "HostEvents");
        }
    }
}
