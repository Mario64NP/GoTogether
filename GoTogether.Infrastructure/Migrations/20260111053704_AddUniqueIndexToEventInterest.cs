using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoTogether.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToEventInterest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventInterests_EventId",
                table: "EventInterests");

            migrationBuilder.CreateIndex(
                name: "IX_EventInterests_EventId_UserId",
                table: "EventInterests",
                columns: new[] { "EventId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventInterests_EventId_UserId",
                table: "EventInterests");

            migrationBuilder.CreateIndex(
                name: "IX_EventInterests_EventId",
                table: "EventInterests",
                column: "EventId");
        }
    }
}
