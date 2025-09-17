using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace koninkrijk.Server.Migrations
{
    /// <inheritdoc />
    public partial class migratetries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tries",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "PlayerProvince",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProvinceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tries = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerProvince", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerProvince_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerProvince_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProvince_PlayerId",
                table: "PlayerProvince",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProvince_ProvinceId",
                table: "PlayerProvince",
                column: "ProvinceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerProvince");

            migrationBuilder.AddColumn<int>(
                name: "Tries",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
