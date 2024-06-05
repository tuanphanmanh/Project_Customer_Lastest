using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class DocumentListAddMore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "EncryptedUserPass",
                table: "EsignDocumentListTemp",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUserPassInput",
                table: "EsignDocumentListTemp",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "SecretKey",
                table: "EsignDocumentListTemp",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "EncryptedUserPass",
                table: "EsignDocumentList",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUserPassInput",
                table: "EsignDocumentList",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "SecretKey",
                table: "EsignDocumentList",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EncryptedUserPass",
                table: "EsignDocumentListTemp");

            migrationBuilder.DropColumn(
                name: "IsUserPassInput",
                table: "EsignDocumentListTemp");

            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "EsignDocumentListTemp");

            migrationBuilder.DropColumn(
                name: "EncryptedUserPass",
                table: "EsignDocumentList");

            migrationBuilder.DropColumn(
                name: "IsUserPassInput",
                table: "EsignDocumentList");

            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "EsignDocumentList");
        }
    }
}
