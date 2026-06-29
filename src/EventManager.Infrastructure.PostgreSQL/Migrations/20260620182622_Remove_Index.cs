using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManager.Infrastructure.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Bookings_UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");
        }
    }
}
