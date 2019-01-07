using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataDecipher.WebApp.Migrations
{
    public partial class AddedCsvParserConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CsvParserConfigID",
                table: "SharedMethods",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CsvParserConfigID",
                table: "MethodDataSources",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CsvParserConfigs",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    Delimiter = table.Column<string>(nullable: false),
                    RequiredHeader = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CsvParserConfigs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CsvParserConfigs_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedMethods_CsvParserConfigID",
                table: "SharedMethods",
                column: "CsvParserConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_MethodDataSources_CsvParserConfigID",
                table: "MethodDataSources",
                column: "CsvParserConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_CsvParserConfigs_CreatedById",
                table: "CsvParserConfigs",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_MethodDataSources_CsvParserConfigs_CsvParserConfigID",
                table: "MethodDataSources",
                column: "CsvParserConfigID",
                principalTable: "CsvParserConfigs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SharedMethods_CsvParserConfigs_CsvParserConfigID",
                table: "SharedMethods",
                column: "CsvParserConfigID",
                principalTable: "CsvParserConfigs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MethodDataSources_CsvParserConfigs_CsvParserConfigID",
                table: "MethodDataSources");

            migrationBuilder.DropForeignKey(
                name: "FK_SharedMethods_CsvParserConfigs_CsvParserConfigID",
                table: "SharedMethods");

            migrationBuilder.DropTable(
                name: "CsvParserConfigs");

            migrationBuilder.DropIndex(
                name: "IX_SharedMethods_CsvParserConfigID",
                table: "SharedMethods");

            migrationBuilder.DropIndex(
                name: "IX_MethodDataSources_CsvParserConfigID",
                table: "MethodDataSources");

            migrationBuilder.DropColumn(
                name: "CsvParserConfigID",
                table: "SharedMethods");

            migrationBuilder.DropColumn(
                name: "CsvParserConfigID",
                table: "MethodDataSources");
        }
    }
}
