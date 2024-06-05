using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColTypeEsignRequestAddColAddCcToEsignRequestRemoveIsCcEsignSignerList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCc",
                table: "EsignSignerList");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "EsignRequest",
                type: "decimal(15,3)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ROI",
                table: "EsignRequest",
                type: "decimal(3,3)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddCC",
                table: "EsignRequest",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddCC",
                table: "EsignRequest");

            migrationBuilder.AddColumn<bool>(
                name: "IsCc",
                table: "EsignSignerList",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<long>(
                name: "TotalCost",
                table: "EsignRequest",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ROI",
                table: "EsignRequest",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,3)",
                oldNullable: true);
        }
    }
}
