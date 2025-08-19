using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CostObject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HierarchyDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EmployeeGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequiredBookingLevelId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HierarchyDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Labels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EmployeeGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HierarchyLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Depth = table.Column<int>(type: "integer", nullable: false),
                    HierarchyDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HierarchyLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HierarchyLevels_HierarchyDefinitions_HierarchyDefinitionId",
                        column: x => x.HierarchyDefinitionId,
                        principalTable: "HierarchyDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CostObjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EmployeeGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentCostObjectId = table.Column<Guid>(type: "uuid", nullable: true),
                    HierarchyLevelId = table.Column<Guid>(type: "uuid", nullable: false),
                    LabelId = table.Column<Guid>(type: "uuid", nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "date", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "date", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostObjects_CostObjects_ParentCostObjectId",
                        column: x => x.ParentCostObjectId,
                        principalTable: "CostObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CostObjects_HierarchyLevels_HierarchyLevelId",
                        column: x => x.HierarchyLevelId,
                        principalTable: "HierarchyLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CostObjects_Labels_LabelId",
                        column: x => x.LabelId,
                        principalTable: "Labels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CostObjectRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CostObjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterEmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    RejectionReason = table.Column<string>(type: "text", nullable: true),
                    ApproverEmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReassignmentCostObjectId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostObjectRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostObjectRequests_CostObjects_CostObjectId",
                        column: x => x.CostObjectId,
                        principalTable: "CostObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CostObjectRequests_CostObjectId",
                table: "CostObjectRequests",
                column: "CostObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CostObjects_HierarchyLevelId",
                table: "CostObjects",
                column: "HierarchyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_CostObjects_LabelId",
                table: "CostObjects",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_CostObjects_ParentCostObjectId",
                table: "CostObjects",
                column: "ParentCostObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_HierarchyDefinitions_EmployeeGroupId_Name",
                table: "HierarchyDefinitions",
                columns: new[] { "EmployeeGroupId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HierarchyLevels_HierarchyDefinitionId",
                table: "HierarchyLevels",
                column: "HierarchyDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Labels_EmployeeGroupId_Name",
                table: "Labels",
                columns: new[] { "EmployeeGroupId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostObjectRequests");

            migrationBuilder.DropTable(
                name: "CostObjects");

            migrationBuilder.DropTable(
                name: "HierarchyLevels");

            migrationBuilder.DropTable(
                name: "Labels");

            migrationBuilder.DropTable(
                name: "HierarchyDefinitions");
        }
    }
}
