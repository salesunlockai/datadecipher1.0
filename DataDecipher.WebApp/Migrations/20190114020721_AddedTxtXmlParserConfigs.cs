using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataDecipher.WebApp.Migrations
{
    public partial class AddedTxtXmlParserConfigs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CsvParserConfigs_AspNetUsers_CreatedById",
                table: "CsvParserConfigs");

            migrationBuilder.DropTable(
                name: "ParserCsvFiles");

            migrationBuilder.AddColumn<string>(
                name: "TxtParserConfigID",
                table: "SharedMethods",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XmlParserConfigID",
                table: "SharedMethods",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TxtParserConfigID",
                table: "MethodDataSources",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XmlParserConfigID",
                table: "MethodDataSources",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "CsvParserConfigs",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateTable(
                name: "TxtParserConfigs",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    RecordMarkerStart = table.Column<string>(nullable: false),
                    RecordMarkerEnd = table.Column<string>(nullable: false),
                    HeaderMarkerStart = table.Column<string>(nullable: false),
                    HeaderMarkerEnd = table.Column<string>(nullable: false),
                    TableMarkerStart = table.Column<string>(nullable: true),
                    TableMarkerEnd = table.Column<string>(nullable: true),
                    HeaderFields = table.Column<string>(nullable: true),
                    TableFields = table.Column<string>(nullable: true),
                    Delimiter = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TxtParserConfigs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TxtParserConfigs_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "XmlParserConfigs",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    ParentTag = table.Column<string>(nullable: false),
                    HeaderFields = table.Column<string>(nullable: false),
                    TableFields = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XmlParserConfigs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_XmlParserConfigs_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedMethods_TxtParserConfigID",
                table: "SharedMethods",
                column: "TxtParserConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_SharedMethods_XmlParserConfigID",
                table: "SharedMethods",
                column: "XmlParserConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_MethodDataSources_TxtParserConfigID",
                table: "MethodDataSources",
                column: "TxtParserConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_MethodDataSources_XmlParserConfigID",
                table: "MethodDataSources",
                column: "XmlParserConfigID");

            migrationBuilder.CreateIndex(
                name: "IX_TxtParserConfigs_CreatedById",
                table: "TxtParserConfigs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_XmlParserConfigs_CreatedById",
                table: "XmlParserConfigs",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_CsvParserConfigs_AspNetUsers_CreatedById",
                table: "CsvParserConfigs",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MethodDataSources_TxtParserConfigs_TxtParserConfigID",
                table: "MethodDataSources",
                column: "TxtParserConfigID",
                principalTable: "TxtParserConfigs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MethodDataSources_XmlParserConfigs_XmlParserConfigID",
                table: "MethodDataSources",
                column: "XmlParserConfigID",
                principalTable: "XmlParserConfigs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SharedMethods_TxtParserConfigs_TxtParserConfigID",
                table: "SharedMethods",
                column: "TxtParserConfigID",
                principalTable: "TxtParserConfigs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SharedMethods_XmlParserConfigs_XmlParserConfigID",
                table: "SharedMethods",
                column: "XmlParserConfigID",
                principalTable: "XmlParserConfigs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CsvParserConfigs_AspNetUsers_CreatedById",
                table: "CsvParserConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_MethodDataSources_TxtParserConfigs_TxtParserConfigID",
                table: "MethodDataSources");

            migrationBuilder.DropForeignKey(
                name: "FK_MethodDataSources_XmlParserConfigs_XmlParserConfigID",
                table: "MethodDataSources");

            migrationBuilder.DropForeignKey(
                name: "FK_SharedMethods_TxtParserConfigs_TxtParserConfigID",
                table: "SharedMethods");

            migrationBuilder.DropForeignKey(
                name: "FK_SharedMethods_XmlParserConfigs_XmlParserConfigID",
                table: "SharedMethods");

            migrationBuilder.DropTable(
                name: "TxtParserConfigs");

            migrationBuilder.DropTable(
                name: "XmlParserConfigs");

            migrationBuilder.DropIndex(
                name: "IX_SharedMethods_TxtParserConfigID",
                table: "SharedMethods");

            migrationBuilder.DropIndex(
                name: "IX_SharedMethods_XmlParserConfigID",
                table: "SharedMethods");

            migrationBuilder.DropIndex(
                name: "IX_MethodDataSources_TxtParserConfigID",
                table: "MethodDataSources");

            migrationBuilder.DropIndex(
                name: "IX_MethodDataSources_XmlParserConfigID",
                table: "MethodDataSources");

            migrationBuilder.DropColumn(
                name: "TxtParserConfigID",
                table: "SharedMethods");

            migrationBuilder.DropColumn(
                name: "XmlParserConfigID",
                table: "SharedMethods");

            migrationBuilder.DropColumn(
                name: "TxtParserConfigID",
                table: "MethodDataSources");

            migrationBuilder.DropColumn(
                name: "XmlParserConfigID",
                table: "MethodDataSources");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "CsvParserConfigs",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ParserCsvFiles",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Delimiter = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    RequiredHeader = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParserCsvFiles", x => x.ID);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CsvParserConfigs_AspNetUsers_CreatedById",
                table: "CsvParserConfigs",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
