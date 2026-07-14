using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class InboxPendingMessages_WithUid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"InboxPendingMessages\";");

            migrationBuilder.CreateTable(
                name: "InboxPendingMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    BookingId = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "InboxPendingMessages");

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
