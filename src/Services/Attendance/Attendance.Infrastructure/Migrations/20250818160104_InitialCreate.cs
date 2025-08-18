using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Abbreviation = table.Column<string>(type: "text", nullable: false),
                    IsPresent = table.Column<bool>(type: "boolean", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeWorkModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkModelId = table.Column<Guid>(type: "uuid", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorkModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PublicHolidays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EmployeeGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicHolidays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EmployeeGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    MondayHours = table.Column<decimal>(type: "numeric(4,2)", nullable: false),
                    TuesdayHours = table.Column<decimal>(type: "numeric(4,2)", nullable: false),
                    WednesdayHours = table.Column<decimal>(type: "numeric(4,2)", nullable: false),
                    ThursdayHours = table.Column<decimal>(type: "numeric(4,2)", nullable: false),
                    FridayHours = table.Column<decimal>(type: "numeric(4,2)", nullable: false),
                    SaturdayHours = table.Column<decimal>(type: "numeric(4,2)", nullable: false),
                    SundayHours = table.Column<decimal>(type: "numeric(4,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    AttendanceTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceEntries_AttendanceTypes_AttendanceTypeId",
                        column: x => x.AttendanceTypeId,
                        principalTable: "AttendanceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceEntries_AttendanceTypeId",
                table: "AttendanceEntries",
                column: "AttendanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceEntries_EmployeeId_Date",
                table: "AttendanceEntries",
                columns: new[] { "EmployeeId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkModels_EmployeeId",
                table: "EmployeeWorkModels",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkModels_EmployeeId_ValidFrom_ValidTo",
                table: "EmployeeWorkModels",
                columns: new[] { "EmployeeId", "ValidFrom", "ValidTo" });

            migrationBuilder.CreateIndex(
                name: "IX_PublicHolidays_EmployeeGroupId_Date",
                table: "PublicHolidays",
                columns: new[] { "EmployeeGroupId", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceEntries");

            migrationBuilder.DropTable(
                name: "EmployeeWorkModels");

            migrationBuilder.DropTable(
                name: "PublicHolidays");

            migrationBuilder.DropTable(
                name: "WorkModels");

            migrationBuilder.DropTable(
                name: "AttendanceTypes");
        }
    }
}
