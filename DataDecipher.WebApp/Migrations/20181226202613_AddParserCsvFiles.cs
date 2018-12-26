using Microsoft.EntityFrameworkCore.Migrations;

namespace DataDecipher.WebApp.Migrations
{
    public partial class AddParserCsvFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParserCsvFiles",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Details = table.Column<string>(nullable: true),
                    Delimiter = table.Column<string>(nullable: false),
                    RequiredHeader = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParserCsvFiles", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParserCsvFiles");
        }
    }
}
