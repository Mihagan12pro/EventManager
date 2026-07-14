using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePendingOutbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "OutboxPendingMessages");
            //migrationBuilder.RenameTable(
            //    name: "OutboxPendingMessages",
            //    newName: "PendingBookingMessage");

            //migrationBuilder.RenameIndex(
            //    name: "IX_OutboxPendingMessages_BookingId",
            //    table: "PendingBookingMessage",
            //    newName: "IX_PendingBookingMessage_BookingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutboxPendingMessages",
                columns: table => new
                {
                    BookingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutboxPendingMessages_BookingId",
                table: "OutboxPendingMessages",
                column: "BookingId",
                unique: true);
            //migrationBuilder.RenameTable(
            //    name: "PendingBookingMessage",
            //    newName: "OutboxPendingMessages");

            //migrationBuilder.RenameIndex(
            //    name: "IX_PendingBookingMessage_BookingId",
            //    table: "OutboxPendingMessages",
            //    newName: "IX_OutboxPendingMessages_BookingId");
        }
    }
}
