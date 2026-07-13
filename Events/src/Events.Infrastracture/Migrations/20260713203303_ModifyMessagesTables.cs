using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Infrastracture.Postgre.Migrations
{
    /// <inheritdoc />
    public partial class ModifyMessagesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "InboxPendingMessages",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "InboxCancelledMessages",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ConfirmedBookingsMessages",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "InboxPendingMessages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "InboxCancelledMessages");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ConfirmedBookingsMessages");
        }
    }
}
