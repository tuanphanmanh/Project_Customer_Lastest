using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class EsignDocumentListAddIsEncrypted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentSmallPart",
                table: "EsignDocumentListTemp");

            migrationBuilder.DropColumn(
                name: "DocumentSmallPart",
                table: "EsignDocumentList");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEncrypted",
                table: "EsignDocumentListTemp");

            migrationBuilder.DropColumn(
                name: "IsEncrypted",
                table: "EsignDocumentList");

            migrationBuilder.AddColumn<byte[]>(
                name: "DocumentSmallPart",
                table: "EsignDocumentListTemp",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "DocumentSmallPart",
                table: "EsignDocumentList",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
