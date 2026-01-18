using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjeAyuDeneme.Migrations
{
    /// <inheritdoc />
    public partial class AddOnayDurumuToFaaliyetler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OnayDurumu",
                table: "Sektorler",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OnayDurumu",
                table: "Reklamlar",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OnayDurumu",
                table: "KulturSanatlar",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OnayDurumu",
                table: "Fuarlar",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OnayDurumu",
                table: "Agirlamalar",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnayDurumu",
                table: "Sektorler");

            migrationBuilder.DropColumn(
                name: "OnayDurumu",
                table: "Reklamlar");

            migrationBuilder.DropColumn(
                name: "OnayDurumu",
                table: "KulturSanatlar");

            migrationBuilder.DropColumn(
                name: "OnayDurumu",
                table: "Fuarlar");

            migrationBuilder.DropColumn(
                name: "OnayDurumu",
                table: "Agirlamalar");
        }
    }
}
