using Microsoft.EntityFrameworkCore.Migrations;

namespace DistancePrinterControl.Database.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Printers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrinterUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Printers",
                columns: new[] { "Id", "PrinterUrl" },
                values: new object[] { 1, "http://192.168.1.125:5000" });

            migrationBuilder.InsertData(
                table: "Printers",
                columns: new[] { "Id", "PrinterUrl" },
                values: new object[] { 2, "http://192.168.1.125:5000" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Printers");
        }
    }
}
