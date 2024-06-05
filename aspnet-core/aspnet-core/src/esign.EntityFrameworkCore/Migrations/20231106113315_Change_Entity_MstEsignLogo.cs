using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEntityMstEsignLogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LogoUrl",
                table: "MstEsignLogo",
                newName: "LogoMinUrl");

            migrationBuilder.AddColumn<string>(
                name: "LogoMaxUrl",
                table: "MstEsignLogo",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoMaxUrl",
                table: "MstEsignLogo");

            migrationBuilder.RenameColumn(
                name: "LogoMinUrl",
                table: "MstEsignLogo",
                newName: "LogoUrl");
        }
    }
}
