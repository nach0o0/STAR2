using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Planning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanningEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CostObjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlannedHours = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    PlanningPeriodStart = table.Column<DateTime>(type: "date", nullable: false),
                    PlanningPeriodEnd = table.Column<DateTime>(type: "date", nullable: false),
                    PlannerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningEntries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanningEntries_CostObjectId",
                table: "PlanningEntries",
                column: "CostObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningEntries_EmployeeGroupId",
                table: "PlanningEntries",
                column: "EmployeeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanningEntries_EmployeeId",
                table: "PlanningEntries",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanningEntries");
        }
    }
}
