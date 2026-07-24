using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceProjesi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class YorumTablosuGuncel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yorumlar_Musteriler_MusteriId",
                table: "Yorumlar");

            migrationBuilder.DropColumn(
                name: "Onaylandi",
                table: "Yorumlar");

            migrationBuilder.RenameColumn(
                name: "Metin",
                table: "Yorumlar",
                newName: "Icerik");

            migrationBuilder.AlterColumn<int>(
                name: "MusteriId",
                table: "Yorumlar",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Yorumlar",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Yorumlar_UserId",
                table: "Yorumlar",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Yorumlar_AspNetUsers_UserId",
                table: "Yorumlar",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Yorumlar_Musteriler_MusteriId",
                table: "Yorumlar",
                column: "MusteriId",
                principalTable: "Musteriler",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Yorumlar_AspNetUsers_UserId",
                table: "Yorumlar");

            migrationBuilder.DropForeignKey(
                name: "FK_Yorumlar_Musteriler_MusteriId",
                table: "Yorumlar");

            migrationBuilder.DropIndex(
                name: "IX_Yorumlar_UserId",
                table: "Yorumlar");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Yorumlar");

            migrationBuilder.RenameColumn(
                name: "Icerik",
                table: "Yorumlar",
                newName: "Metin");

            migrationBuilder.AlterColumn<int>(
                name: "MusteriId",
                table: "Yorumlar",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Onaylandi",
                table: "Yorumlar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Yorumlar_Musteriler_MusteriId",
                table: "Yorumlar",
                column: "MusteriId",
                principalTable: "Musteriler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
