using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class MstEsignUserImageAddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImgHeight",
                table: "MstEsignUserImage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImgName",
                table: "MstEsignUserImage",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImgWidth",
                table: "MstEsignUserImage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgHeight",
                table: "MstEsignUserImage");

            migrationBuilder.DropColumn(
                name: "ImgName",
                table: "MstEsignUserImage");

            migrationBuilder.DropColumn(
                name: "ImgWidth",
                table: "MstEsignUserImage");
        }
    }
}
