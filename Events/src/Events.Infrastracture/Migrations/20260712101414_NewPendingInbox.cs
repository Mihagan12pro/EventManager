using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class NewPendingInbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_PendingBooking",
            //    table: "PendingBooking");

            //migrationBuilder.RenameTable(
            //    name: "PendingBooking",
            //    newName: "InboxPendingMessages");

            //migrationBuilder.RenameIndex(
            //    name: "IX_PendingBooking_BookingId",
            //    table: "InboxPendingMessages",
            //    newName: "IX_InboxPendingMessages_BookingId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_InboxPendingMessages",
            //    table: "InboxPendingMessages",
              //  column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_InboxPendingMessages",
            //    table: "InboxPendingMessages");

            //migrationBuilder.RenameTable(
            //    name: "InboxPendingMessages",
            //    newName: "PendingBooking");

            //migrationBuilder.RenameIndex(
            //    name: "IX_InboxPendingMessages_BookingId",
            //    table: "PendingBooking",
            //    newName: "IX_PendingBooking_BookingId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_PendingBooking",
            //    table: "PendingBooking",
            //    column: "Id");
        }
    }
}
