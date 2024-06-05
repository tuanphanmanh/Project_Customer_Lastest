using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class EsignDocumentListRemoveIsEncrypted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEncrypted",
                table: "EsignDocumentListTemp");

            migrationBuilder.DropColumn(
                name: "IsEncrypted",
                table: "EsignDocumentList");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEncrypted",
                table: "EsignDocumentListTemp",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEncrypted",
                table: "EsignDocumentList",
                type: "bit",
                nullable: true);
        }
    }
}
