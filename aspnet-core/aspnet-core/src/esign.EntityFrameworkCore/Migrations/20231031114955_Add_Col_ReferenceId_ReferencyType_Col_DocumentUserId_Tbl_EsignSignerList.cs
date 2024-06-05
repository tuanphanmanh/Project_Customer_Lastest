using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class AddColReferenceIdReferencyTypeColDocumentUserIdTblEsignSignerList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DocumentUserId",
                table: "EsignSignerList",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReferenceId",
                table: "EsignRequest",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceType",
                table: "EsignRequest",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentUserId",
                table: "EsignSignerList");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "EsignRequest");

            migrationBuilder.DropColumn(
                name: "ReferenceType",
                table: "EsignRequest");
        }
    }
}
