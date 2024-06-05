using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace esign.Migrations
{
    /// <inheritdoc />
    public partial class AddESignTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ESiDocument",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DocumentPath = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SystemId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RefId = table.Column<long>(type: "bigint", nullable: true),
                    RefType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESiDocument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESiDocumentFile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<long>(type: "bigint", nullable: true),
                    OriginalFileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    OriginalServerFileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ServerFileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ServerFileNamePreSign = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FileSize = table.Column<float>(type: "real", nullable: true),
                    DocumentPath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    IsLock = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    RefId = table.Column<long>(type: "bigint", nullable: true),
                    SystemId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESiDocumentFile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESiDocumentFileImage",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentFileId = table.Column<long>(type: "bigint", nullable: true),
                    ServerFileName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    FileSeq = table.Column<long>(type: "bigint", nullable: true),
                    ImageW = table.Column<long>(type: "bigint", nullable: true),
                    ImageH = table.Column<long>(type: "bigint", nullable: true),
                    DocumentId = table.Column<long>(type: "bigint", nullable: true),
                    RefId = table.Column<long>(type: "bigint", nullable: true),
                    SystemId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESiDocumentFileImage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESiDocumentFileImageSign",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentFileImageId = table.Column<long>(type: "bigint", nullable: true),
                    DocumentUserId = table.Column<long>(type: "bigint", nullable: true),
                    EmailSigner = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RefId = table.Column<long>(type: "bigint", nullable: true),
                    SystemId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PositionX = table.Column<long>(type: "bigint", nullable: true),
                    PositionY = table.Column<long>(type: "bigint", nullable: true),
                    PositionW = table.Column<long>(type: "bigint", nullable: true),
                    PositionH = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESiDocumentFileImageSign", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESiDocumentFileImageSignLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentFileImageId = table.Column<long>(type: "bigint", nullable: true),
                    PositionX = table.Column<long>(type: "bigint", nullable: true),
                    PositionY = table.Column<long>(type: "bigint", nullable: true),
                    PositionW = table.Column<long>(type: "bigint", nullable: true),
                    PositionH = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESiDocumentFileImageSignLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESiDocumentFileLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentFileId = table.Column<long>(type: "bigint", nullable: true),
                    DocumentUserLogId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedByName = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedByUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESiDocumentFileLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESiDocumentForward",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForwardUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ForwardUserId = table.Column<long>(type: "bigint", nullable: true),
                    ForwardName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESiDocumentForward", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESiDocumentUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedByUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Seq = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeadLineDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SystemId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RefId = table.Column<long>(type: "bigint", nullable: true),
                    SignatureString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ForwardByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ForwardByUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ForwardDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDigitalSign = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESiDocumentUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESiDocumentUserLog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApprovedByUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RejectNote = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DocumentUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESiDocumentUserLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESiEmailAd",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SNo = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmailAlias = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SamAccountName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Division = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Manager = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Office = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AccountExpiryTime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccountStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CommonName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LogonName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESiEmailAd", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESiEmailTemplate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Bcc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESiEmailTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESiNotification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedByUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApprovedByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentId = table.Column<long>(type: "bigint", nullable: true),
                    SenderByUserId = table.Column<long>(type: "bigint", nullable: true),
                    SenderByUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SenderByName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsRead = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESiNotification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESiSignature",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SignaturePath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESiSignature", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ESiSystem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ResultUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ForwardUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RequestInfoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESiSystem", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ESiDocument");

            migrationBuilder.DropTable(
                name: "ESiDocumentFile");

            migrationBuilder.DropTable(
                name: "ESiDocumentFileImage");

            migrationBuilder.DropTable(
                name: "ESiDocumentFileImageSign");

            migrationBuilder.DropTable(
                name: "ESiDocumentFileImageSignLog");

            migrationBuilder.DropTable(
                name: "ESiDocumentFileLog");

            migrationBuilder.DropTable(
                name: "ESiDocumentForward");

            migrationBuilder.DropTable(
                name: "ESiDocumentUser");

            migrationBuilder.DropTable(
                name: "ESiDocumentUserLog");

            migrationBuilder.DropTable(
                name: "ESiEmailAd");

            migrationBuilder.DropTable(
                name: "ESiEmailTemplate");

            migrationBuilder.DropTable(
                name: "ESiNotification");

            migrationBuilder.DropTable(
                name: "ESiSignature");

            migrationBuilder.DropTable(
                name: "ESiSystem");
        }
    }
}
