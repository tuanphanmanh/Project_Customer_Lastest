using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class multiaffiliateref : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReferenceId",
                table: "EsignPosition",
                newName: "AffiliateReferenceId");

            migrationBuilder.RenameColumn(
                name: "ReferenceId",
                table: "EsignDocumentList",
                newName: "AffiliateReferenceId");

            migrationBuilder.RenameColumn(
                name: "ReferenceId",
                table: "EsignActivityHistory",
                newName: "AffiliateReferenceId");

            migrationBuilder.AddColumn<long>(
                name: "AffiliateReferenceId",
                table: "EsignSignerList",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Affiliate",
                table: "EsignRequest",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AffiliateReferenceId",
                table: "EsignRequest",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AffiliateReferenceId",
                table: "EsignSignerList");

            migrationBuilder.DropColumn(
                name: "Affiliate",
                table: "EsignRequest");

            migrationBuilder.DropColumn(
                name: "AffiliateReferenceId",
                table: "EsignRequest");

            migrationBuilder.RenameColumn(
                name: "AffiliateReferenceId",
                table: "EsignPosition",
                newName: "ReferenceId");

            migrationBuilder.RenameColumn(
                name: "AffiliateReferenceId",
                table: "EsignDocumentList",
                newName: "ReferenceId");

            migrationBuilder.RenameColumn(
                name: "AffiliateReferenceId",
                table: "EsignActivityHistory",
                newName: "ReferenceId");
        }
    }
}
