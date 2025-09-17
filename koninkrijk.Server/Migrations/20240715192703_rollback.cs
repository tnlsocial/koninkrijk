using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace koninkrijk.Server.Migrations
{
    /// <inheritdoc />
    public partial class rollback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentFeedback",
                table: "PlayerProvince",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentFeedback",
                table: "PlayerProvince");
        }
    }
}
