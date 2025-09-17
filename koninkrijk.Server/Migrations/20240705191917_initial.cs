using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace koninkrijk.Server.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ProvinceSize = table.Column<int>(type: "INTEGER", nullable: false),
                    LastCapture = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CurrentPlayer = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    LastCaptureTry = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tries = table.Column<int>(type: "INTEGER", nullable: false),
                    ApiKey = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentWord = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentFeedback = table.Column<string>(type: "TEXT", nullable: true),
                    ProvinceId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "Id", "CurrentPlayer", "Description", "LastCapture", "Name", "ProvinceSize" },
                values: new object[,]
                {
                    { 1, null, "The province of Groningen", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Groningen", 4 },
                    { 2, null, "The province of Fryslân", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fryslân", 5 },
                    { 3, null, "The province of Drenthe", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Drenthe", 4 },
                    { 4, null, "The province of Overijssel", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Overijssel", 6 },
                    { 5, null, "The province of Flevoland", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Flevoland", 3 },
                    { 6, null, "The province of Gelderland", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gelderland", 9 },
                    { 7, null, "The province of Utrecht", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Utrecht", 2 },
                    { 8, null, "The province of Noord-Holland", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Noord-Holland", 6 },
                    { 9, null, "The province of Zuid-Holland", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zuid-Holland", 7 },
                    { 10, null, "The province of Zeeland", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zeeland", 3 },
                    { 11, null, "The province of Noord-Brabant", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Noord-Brabant", 8 },
                    { 12, null, "The province of Limburg", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Limburg", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_ProvinceId",
                table: "Players",
                column: "ProvinceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Provinces");
        }
    }
}
