using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace koninkrijk.Server.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentFeedback",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CurrentWord",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "CurrentFeedback",
                table: "PlayerProvince",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentWord",
                table: "PlayerProvince",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCaptureTry",
                table: "PlayerProvince",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentFeedback",
                table: "PlayerProvince");

            migrationBuilder.DropColumn(
                name: "CurrentWord",
                table: "PlayerProvince");

            migrationBuilder.DropColumn(
                name: "LastCaptureTry",
                table: "PlayerProvince");

            migrationBuilder.AddColumn<string>(
                name: "CurrentFeedback",
                table: "Players",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentWord",
                table: "Players",
                type: "TEXT",
                nullable: true);
        }
    }
}
