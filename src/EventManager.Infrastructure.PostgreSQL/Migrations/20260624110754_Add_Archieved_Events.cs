using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManager.Infrastructure.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class Add_Archieved_Events : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArchivedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivedEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchivedEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchivedEvents_EventId",
                table: "ArchivedEvents",
                column: "EventId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchivedEvents");
        }
    }
}
