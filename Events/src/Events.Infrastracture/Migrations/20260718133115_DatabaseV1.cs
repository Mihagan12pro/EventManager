using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Events.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS "Events" (
                    "Id" uuid NOT NULL,
                    "StartAt" timestamp with time zone NOT NULL,
                    "EndAt" timestamp with time zone NOT NULL,
                    "Title" text NOT NULL,
                    "Description" text NOT NULL,
                    "AvailableSeats" integer NOT NULL,
                    "TotalSeats" integer NOT NULL,
                    CONSTRAINT "PK_Events" PRIMARY KEY ("Id")
                );
            """);

            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS "InboxCancelledMessages" (
                    "Id" uuid NOT NULL,
                    "EventId" uuid NOT NULL,
                    "UserId" uuid,
                    "BookingId" uuid NOT NULL,
                    "OccurredAt" timestamp with time zone NOT NULL,
                    CONSTRAINT "PK_InboxCancelledMessages" PRIMARY KEY ("Id")
                );
            """);

            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS "InboxPendingMessages" (
                    "Id" uuid NOT NULL,
                    "EventId" uuid NOT NULL,
                    "UserId" uuid,
                    "BookingId" uuid NOT NULL,
                    "OccurredAt" timestamp with time zone NOT NULL,
                    CONSTRAINT "PK_InboxPendingMessages" PRIMARY KEY ("Id")
                );
            """);

            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS "OutboxConfirmedBookingsMessages" (
                    "Id" uuid NOT NULL,
                    "EventId" uuid NOT NULL,
                    "UserId" uuid,
                    "BookingId" uuid NOT NULL,
                    "OccurredAt" timestamp with time zone NOT NULL,
                    CONSTRAINT "PK_OutboxConfirmedBookingsMessages" PRIMARY KEY ("Id")
                );
            """);

            migrationBuilder.Sql("""
                CREATE UNIQUE INDEX IF NOT EXISTS "IX_InboxCancelledMessages_BookingId"
                ON "InboxCancelledMessages" ("BookingId");
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "InboxCancelledMessages");

            migrationBuilder.DropTable(
                name: "InboxPendingMessages");

            migrationBuilder.DropTable(
                name: "OutboxConfirmedBookingsMessages");
        }
    }
}
