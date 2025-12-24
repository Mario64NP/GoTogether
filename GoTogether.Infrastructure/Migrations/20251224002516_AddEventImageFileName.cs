using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoTogether.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEventImageFileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "Events",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "Events");
        }
    }
}
