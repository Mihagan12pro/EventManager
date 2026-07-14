using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class RenameConfirmedBookingsMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfirmedBookingsMessages",
                table: "ConfirmedBookingsMessages");

            migrationBuilder.RenameTable(
                name: "ConfirmedBookingsMessages",
                newName: "OutboxConfirmedBookingsMessages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxConfirmedBookingsMessages",
                table: "OutboxConfirmedBookingsMessages",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxConfirmedBookingsMessages",
                table: "OutboxConfirmedBookingsMessages");

            migrationBuilder.RenameTable(
                name: "OutboxConfirmedBookingsMessages",
                newName: "ConfirmedBookingsMessages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfirmedBookingsMessages",
                table: "ConfirmedBookingsMessages",
                column: "Id");
        }
    }
}
