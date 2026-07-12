using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Infrastracture.Postgre.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMessagesFieldsType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "InboxPendingMessages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
             name: "InboxPendingMessages",
             columns: table => new
             {
                 Id = table.Column<string>(type: "text", nullable: false),
                 EventId = table.Column<string>(type: "text", nullable: false),
                 BookingId = table.Column<string>(type: "text", nullable: false),
                 OccurredAt = table.Column<string>(type: "text", nullable: false)
             },
             constraints: table =>
             {
                 table.PrimaryKey("PK_InboxPendingMessages", x => x.Id);
             });

            migrationBuilder.CreateIndex(
                name: "IX_InboxPendingMessages_BookingId",
                table: "InboxPendingMessages",
                column: "BookingId",
                unique: true);
        }
    }
}
