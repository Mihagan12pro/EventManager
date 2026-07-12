using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Remove_InboxMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"PendingBookingMessage\";");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PendingBookingMessage",
                columns: table => new
                {
                    BookingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_PendingBookingMessage_BookingId",
                table: "PendingBookingMessage",
                column: "BookingId",
                unique: true);
        }
    }
}
