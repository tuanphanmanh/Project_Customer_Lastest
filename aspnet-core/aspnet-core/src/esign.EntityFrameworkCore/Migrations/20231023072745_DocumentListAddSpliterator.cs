using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class DocumentListAddSpliterator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EncryptedUserPass",
                table: "EsignDocumentListTemp");

            migrationBuilder.DropColumn(
                name: "IsUserPassInput",
                table: "EsignDocumentListTemp");

            migrationBuilder.DropColumn(
                name: "EncryptedUserPass",
                table: "EsignDocumentList");

            migrationBuilder.DropColumn(
                name: "IsUserPassInput",
                table: "EsignDocumentList");

            migrationBuilder.RenameColumn(
                name: "SecretKey",
                table: "EsignDocumentListTemp",
                newName: "DocumentSmallPart");

            migrationBuilder.RenameColumn(
                name: "SecretKey",
                table: "EsignDocumentList",
                newName: "DocumentSmallPart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumentSmallPart",
                table: "EsignDocumentListTemp",
                newName: "SecretKey");

            migrationBuilder.RenameColumn(
                name: "DocumentSmallPart",
                table: "EsignDocumentList",
                newName: "SecretKey");

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
                name: "EncryptedUserPass",
                table: "EsignDocumentList",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsUserPassInput",
                table: "EsignDocumentList",
                type: "bit",
                nullable: true);
        }
    }
}
