using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColIsCcAddColNoteTblEsignStatusSignerHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCc",
                table: "EsignStatusSignerHistory");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "EsignStatusSignerHistory",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "EsignStatusSignerHistory");

            migrationBuilder.AddColumn<bool>(
                name: "IsCc",
                table: "EsignStatusSignerHistory",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
