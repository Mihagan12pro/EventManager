using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Postgre.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS "Users" (
                    "Id" uuid NOT NULL,
                    "Login" character varying(256) NOT NULL,
                    "Role" text NOT NULL,
                    "HashedPassword" text NOT NULL,
                    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
                );
            """);

            migrationBuilder.Sql("""
                CREATE UNIQUE INDEX IF NOT EXISTS "IX_Users_Login"
                ON "Users" ("Login");
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
