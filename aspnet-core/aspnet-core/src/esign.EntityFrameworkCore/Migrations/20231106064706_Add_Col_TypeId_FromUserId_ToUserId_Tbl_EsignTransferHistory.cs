using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class AddColTypeIdFromUserIdToUserIdTblEsignTransferHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FromUserId",
                table: "EsignTransferSignerHistory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ToUserId",
                table: "EsignTransferSignerHistory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "EsignTransferSignerHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "EsignTransferSignerHistory");

            migrationBuilder.DropColumn(
                name: "ToUserId",
                table: "EsignTransferSignerHistory");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "EsignTransferSignerHistory");
        }
    }
}
