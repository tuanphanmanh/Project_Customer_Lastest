using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class InsertColumnEsignVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsForceUpdate",
                table: "EsignVersionApp",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlConfig",
                table: "EsignVersionApp",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForceUpdate",
                table: "EsignVersionApp");

            migrationBuilder.DropColumn(
                name: "UrlConfig",
                table: "EsignVersionApp");
        }
    }
}
