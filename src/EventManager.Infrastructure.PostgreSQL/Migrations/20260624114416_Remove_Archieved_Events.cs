using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManager.Infrastructure.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Archieved_Events : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("ArchivedEvents");
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ArchivedEvents_Events_EventId",
            //    table: "ArchivedEvents");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_ArchivedEvents",
            //    table: "ArchivedEvents");

            //migrationBuilder.RenameTable(
            //    name: "ArchivedEvents",
            //    newName: "ArchivedEventEntity");

            //migrationBuilder.RenameIndex(
            //    name: "IX_ArchivedEvents_EventId",
            //    table: "ArchivedEventEntity",
            //    newName: "IX_ArchivedEventEntity_EventId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_ArchivedEventEntity",
            //    table: "ArchivedEventEntity",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ArchivedEventEntity_Events_EventId",
            //    table: "ArchivedEventEntity",
            //    column: "EventId",
            //    principalTable: "Events",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchivedEventEntity_Events_EventId",
                table: "ArchivedEventEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArchivedEventEntity",
                table: "ArchivedEventEntity");

            migrationBuilder.RenameTable(
                name: "ArchivedEventEntity",
                newName: "ArchivedEvents");

            migrationBuilder.RenameIndex(
                name: "IX_ArchivedEventEntity_EventId",
                table: "ArchivedEvents",
                newName: "IX_ArchivedEvents_EventId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArchivedEvents",
                table: "ArchivedEvents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArchivedEvents_Events_EventId",
                table: "ArchivedEvents",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
