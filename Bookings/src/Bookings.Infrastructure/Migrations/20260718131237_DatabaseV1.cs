using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookings.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS "Bookings" (
                    "Id" uuid NOT NULL,
                    "EventId" uuid,
                    "UserId" uuid,
                    "CreatedAt" timestamp with time zone NOT NULL,
                    "Status" integer NOT NULL,
                    "ProcessedAt" timestamp with time zone,
                    CONSTRAINT "PK_Bookings" PRIMARY KEY ("Id")
                );
            """);

            migrationBuilder.Sql("""
                CREATE INDEX IF NOT EXISTS "IX_Bookings_EventId"
                ON "Bookings" ("EventId");
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");
        }
    }
}
