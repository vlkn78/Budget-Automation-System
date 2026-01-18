using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjeAyuDeneme.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAllActivityModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agirlamalar_AspNetUsers_UserId",
                table: "Agirlamalar");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Ofisler_OfisId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Fuarlar_AspNetUsers_UserId",
                table: "Fuarlar");

            migrationBuilder.DropForeignKey(
                name: "FK_KulturSanatlar_AspNetUsers_UserId",
                table: "KulturSanatlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Mahsuplar_Ofisler_OfisId",
                table: "Mahsuplar");

            migrationBuilder.DropForeignKey(
                name: "FK_Odenekler_AspNetUsers_UserId",
                table: "Odenekler");

            migrationBuilder.DropForeignKey(
                name: "FK_Odenekler_Ofisler_OfisId",
                table: "Odenekler");

            migrationBuilder.DropForeignKey(
                name: "FK_Reklamlar_AspNetUsers_UserId",
                table: "Reklamlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Sektorler_AspNetUsers_UserId",
                table: "Sektorler");

            migrationBuilder.DropColumn(
                name: "Ofis",
                table: "Sektorler");

            migrationBuilder.DropColumn(
                name: "Ofis",
                table: "Reklamlar");

            migrationBuilder.DropColumn(
                name: "Ofis",
                table: "KulturSanatlar");

            migrationBuilder.DropColumn(
                name: "Ofis",
                table: "Fuarlar");

            migrationBuilder.DropColumn(
                name: "Ofis",
                table: "Agirlamalar");

            migrationBuilder.AddColumn<int>(
                name: "OfisId",
                table: "Sektorler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OfisId",
                table: "Reklamlar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OfisId",
                table: "KulturSanatlar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "StandM2",
                table: "Fuarlar",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "KatilimSayisi",
                table: "Fuarlar",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "OfisId",
                table: "Fuarlar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Adet",
                table: "Agirlamalar",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "OfisId",
                table: "Agirlamalar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sektorler_OfisId",
                table: "Sektorler",
                column: "OfisId");

            migrationBuilder.CreateIndex(
                name: "IX_Reklamlar_OfisId",
                table: "Reklamlar",
                column: "OfisId");

            migrationBuilder.CreateIndex(
                name: "IX_KulturSanatlar_OfisId",
                table: "KulturSanatlar",
                column: "OfisId");

            migrationBuilder.CreateIndex(
                name: "IX_Fuarlar_OfisId",
                table: "Fuarlar",
                column: "OfisId");

            migrationBuilder.CreateIndex(
                name: "IX_Agirlamalar_OfisId",
                table: "Agirlamalar",
                column: "OfisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agirlamalar_AspNetUsers_UserId",
                table: "Agirlamalar",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Agirlamalar_Ofisler_OfisId",
                table: "Agirlamalar",
                column: "OfisId",
                principalTable: "Ofisler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Ofisler_OfisId",
                table: "AspNetUsers",
                column: "OfisId",
                principalTable: "Ofisler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fuarlar_AspNetUsers_UserId",
                table: "Fuarlar",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fuarlar_Ofisler_OfisId",
                table: "Fuarlar",
                column: "OfisId",
                principalTable: "Ofisler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KulturSanatlar_AspNetUsers_UserId",
                table: "KulturSanatlar",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_KulturSanatlar_Ofisler_OfisId",
                table: "KulturSanatlar",
                column: "OfisId",
                principalTable: "Ofisler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mahsuplar_Ofisler_OfisId",
                table: "Mahsuplar",
                column: "OfisId",
                principalTable: "Ofisler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Odenekler_AspNetUsers_UserId",
                table: "Odenekler",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Odenekler_Ofisler_OfisId",
                table: "Odenekler",
                column: "OfisId",
                principalTable: "Ofisler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reklamlar_AspNetUsers_UserId",
                table: "Reklamlar",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reklamlar_Ofisler_OfisId",
                table: "Reklamlar",
                column: "OfisId",
                principalTable: "Ofisler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sektorler_AspNetUsers_UserId",
                table: "Sektorler",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sektorler_Ofisler_OfisId",
                table: "Sektorler",
                column: "OfisId",
                principalTable: "Ofisler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agirlamalar_AspNetUsers_UserId",
                table: "Agirlamalar");

            migrationBuilder.DropForeignKey(
                name: "FK_Agirlamalar_Ofisler_OfisId",
                table: "Agirlamalar");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Ofisler_OfisId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Fuarlar_AspNetUsers_UserId",
                table: "Fuarlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Fuarlar_Ofisler_OfisId",
                table: "Fuarlar");

            migrationBuilder.DropForeignKey(
                name: "FK_KulturSanatlar_AspNetUsers_UserId",
                table: "KulturSanatlar");

            migrationBuilder.DropForeignKey(
                name: "FK_KulturSanatlar_Ofisler_OfisId",
                table: "KulturSanatlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Mahsuplar_Ofisler_OfisId",
                table: "Mahsuplar");

            migrationBuilder.DropForeignKey(
                name: "FK_Odenekler_AspNetUsers_UserId",
                table: "Odenekler");

            migrationBuilder.DropForeignKey(
                name: "FK_Odenekler_Ofisler_OfisId",
                table: "Odenekler");

            migrationBuilder.DropForeignKey(
                name: "FK_Reklamlar_AspNetUsers_UserId",
                table: "Reklamlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Reklamlar_Ofisler_OfisId",
                table: "Reklamlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Sektorler_AspNetUsers_UserId",
                table: "Sektorler");

            migrationBuilder.DropForeignKey(
                name: "FK_Sektorler_Ofisler_OfisId",
                table: "Sektorler");

            migrationBuilder.DropIndex(
                name: "IX_Sektorler_OfisId",
                table: "Sektorler");

            migrationBuilder.DropIndex(
                name: "IX_Reklamlar_OfisId",
                table: "Reklamlar");

            migrationBuilder.DropIndex(
                name: "IX_KulturSanatlar_OfisId",
                table: "KulturSanatlar");

            migrationBuilder.DropIndex(
                name: "IX_Fuarlar_OfisId",
                table: "Fuarlar");

            migrationBuilder.DropIndex(
                name: "IX_Agirlamalar_OfisId",
                table: "Agirlamalar");

            migrationBuilder.DropColumn(
                name: "OfisId",
                table: "Sektorler");

            migrationBuilder.DropColumn(
                name: "OfisId",
                table: "Reklamlar");

            migrationBuilder.DropColumn(
                name: "OfisId",
                table: "KulturSanatlar");

            migrationBuilder.DropColumn(
                name: "OfisId",
                table: "Fuarlar");

            migrationBuilder.DropColumn(
                name: "OfisId",
                table: "Agirlamalar");

            migrationBuilder.AddColumn<string>(
                name: "Ofis",
                table: "Sektorler",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ofis",
                table: "Reklamlar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ofis",
                table: "KulturSanatlar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "StandM2",
                table: "Fuarlar",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "KatilimSayisi",
                table: "Fuarlar",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Ofis",
                table: "Fuarlar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Adet",
                table: "Agirlamalar",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Ofis",
                table: "Agirlamalar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Agirlamalar_AspNetUsers_UserId",
                table: "Agirlamalar",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Ofisler_OfisId",
                table: "AspNetUsers",
                column: "OfisId",
                principalTable: "Ofisler",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fuarlar_AspNetUsers_UserId",
                table: "Fuarlar",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KulturSanatlar_AspNetUsers_UserId",
                table: "KulturSanatlar",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mahsuplar_Ofisler_OfisId",
                table: "Mahsuplar",
                column: "OfisId",
                principalTable: "Ofisler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Odenekler_AspNetUsers_UserId",
                table: "Odenekler",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Odenekler_Ofisler_OfisId",
                table: "Odenekler",
                column: "OfisId",
                principalTable: "Ofisler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reklamlar_AspNetUsers_UserId",
                table: "Reklamlar",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sektorler_AspNetUsers_UserId",
                table: "Sektorler",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
