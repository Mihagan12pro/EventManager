using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class NewInbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_InboxPendingMessages_BookingId",
            //    table: "InboxPendingMessages");

            migrationBuilder.CreateTable(
                name: "InboxCancelledMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxCancelledMessages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InboxCancelledMessages_BookingId",
                table: "InboxCancelledMessages",
                column: "BookingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboxCancelledMessages");

            //migrationBuilder.CreateIndex(
            //    name: "IX_InboxPendingMessages_BookingId",
            //    table: "InboxPendingMessages",
            //    column: "BookingId",
            //    unique: true);
        }
    }
}
