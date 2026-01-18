using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjeAyuDeneme.Migrations
{
    /// <inheritdoc />
    public partial class AddGenelButceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenelButceler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Yil = table.Column<int>(type: "int", nullable: false),
                    Tertip = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BaslangicTutari = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenelButceler", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenelButceler");
        }
    }
}
