using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RioScaffolding.OpenSpacePlanner.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(
                        type: "datetimeoffset",
                        nullable: true,
                        defaultValueSql: "getutcdate()"
                    ),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LastUpdatedAt = table.Column<DateTimeOffset>(
                        type: "datetimeoffset",
                        nullable: true,
                        defaultValueSql: "getutcdate()"
                    ),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id).Annotation("SqlServer:Clustered", false);
                }
            );

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(
                        type: "datetimeoffset",
                        nullable: true,
                        defaultValueSql: "getutcdate()"
                    ),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LastUpdatedAt = table.Column<DateTimeOffset>(
                        type: "datetimeoffset",
                        nullable: true,
                        defaultValueSql: "getutcdate()"
                    ),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id).Annotation("SqlServer:Clustered", true);
                }
            );

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(
                        type: "datetimeoffset",
                        nullable: true,
                        defaultValueSql: "getutcdate()"
                    ),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LastUpdatedAt = table.Column<DateTimeOffset>(
                        type: "datetimeoffset",
                        nullable: true,
                        defaultValueSql: "getutcdate()"
                    ),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id).Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Rooms_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "TimeSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    FromTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    ToTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(
                        type: "datetimeoffset",
                        nullable: true,
                        defaultValueSql: "getutcdate()"
                    ),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LastUpdatedAt = table.Column<DateTimeOffset>(
                        type: "datetimeoffset",
                        nullable: true,
                        defaultValueSql: "getutcdate()"
                    ),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlots", x => x.Id).Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_TimeSlots_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TimeSlotId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(
                        type: "datetimeoffset",
                        nullable: true,
                        defaultValueSql: "getutcdate()"
                    ),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LastUpdatedAt = table.Column<DateTimeOffset>(
                        type: "datetimeoffset",
                        nullable: true,
                        defaultValueSql: "getutcdate()"
                    ),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id).Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Topics_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id"
                    );
                    table.ForeignKey(
                        name: "FK_Topics_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_Topics_TimeSlots_TimeSlotId",
                        column: x => x.TimeSlotId,
                        principalTable: "TimeSlots",
                        principalColumn: "Id"
                    );
                }
            );

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "Id", "CreatedBy", "DeletedAt", "DeletedBy", "Identifier", "LastUpdatedBy", "Name" },
                values: new object[]
                {
                    new Guid("1049efd6-22ab-422a-9bdd-d09a19223f55"),
                    null,
                    null,
                    null,
                    "default",
                    null,
                    "Default Tenant",
                }
            );

            migrationBuilder.CreateIndex(name: "IX_Rooms_SessionId", table: "Rooms", column: "SessionId");

            migrationBuilder
                .CreateIndex(
                    name: "IX_Rooms_TenantId_Id",
                    table: "Rooms",
                    columns: new[] { "TenantId", "Id" },
                    unique: true
                )
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder
                .CreateIndex(
                    name: "IX_Sessions_TenantId_Id",
                    table: "Sessions",
                    columns: new[] { "TenantId", "Id" },
                    unique: true
                )
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Identifier",
                table: "Tenants",
                column: "Identifier",
                unique: true
            );

            migrationBuilder.CreateIndex(name: "IX_TimeSlots_SessionId", table: "TimeSlots", column: "SessionId");

            migrationBuilder
                .CreateIndex(
                    name: "IX_TimeSlots_TenantId_Id",
                    table: "TimeSlots",
                    columns: new[] { "TenantId", "Id" },
                    unique: true
                )
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(name: "IX_Topics_RoomId", table: "Topics", column: "RoomId");

            migrationBuilder.CreateIndex(name: "IX_Topics_SessionId", table: "Topics", column: "SessionId");

            migrationBuilder
                .CreateIndex(
                    name: "IX_Topics_TenantId_Id",
                    table: "Topics",
                    columns: new[] { "TenantId", "Id" },
                    unique: true
                )
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(name: "IX_Topics_TimeSlotId", table: "Topics", column: "TimeSlotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Tenants");

            migrationBuilder.DropTable(name: "Topics");

            migrationBuilder.DropTable(name: "Rooms");

            migrationBuilder.DropTable(name: "TimeSlots");

            migrationBuilder.DropTable(name: "Sessions");
        }
    }
}
