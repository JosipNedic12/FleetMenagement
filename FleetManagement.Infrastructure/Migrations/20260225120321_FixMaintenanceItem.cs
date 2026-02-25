using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FleetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixMaintenanceItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dc_maintenance_type",
                schema: "fleet",
                columns: table => new
                {
                    maintenance_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dc_maintenance_type", x => x.maintenance_type_id);
                });

            migrationBuilder.CreateTable(
                name: "vendor",
                schema: "fleet",
                columns: table => new
                {
                    vendor_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    contact_person = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendor", x => x.vendor_id);
                });

            migrationBuilder.CreateTable(
                name: "maintenance_order",
                schema: "fleet",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vehicle_id = table.Column<int>(type: "integer", nullable: false),
                    vendor_id = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    reported_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    scheduled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    closed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    odometer_km = table.Column<int>(type: "integer", nullable: true),
                    total_cost = table.Column<decimal>(type: "numeric", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    cancel_reason = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true),
                    modified_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintenance_order", x => x.order_id);
                    table.ForeignKey(
                        name: "FK_maintenance_order_vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalSchema: "fleet",
                        principalTable: "vehicle",
                        principalColumn: "vehicle_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_maintenance_order_vendor_vendor_id",
                        column: x => x.vendor_id,
                        principalSchema: "fleet",
                        principalTable: "vendor",
                        principalColumn: "vendor_id");
                });

            migrationBuilder.CreateTable(
                name: "maintenance_item",
                schema: "fleet",
                columns: table => new
                {
                    item_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    maintenance_type_id = table.Column<int>(type: "integer", nullable: false),
                    parts_cost = table.Column<decimal>(type: "numeric", nullable: false),
                    labor_cost = table.Column<decimal>(type: "numeric", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maintenance_item", x => x.item_id);
                    table.ForeignKey(
                        name: "FK_maintenance_item_dc_maintenance_type_maintenance_type_id",
                        column: x => x.maintenance_type_id,
                        principalSchema: "fleet",
                        principalTable: "dc_maintenance_type",
                        principalColumn: "maintenance_type_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_maintenance_item_maintenance_order_order_id",
                        column: x => x.order_id,
                        principalSchema: "fleet",
                        principalTable: "maintenance_order",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_maintenance_item_maintenance_type_id",
                schema: "fleet",
                table: "maintenance_item",
                column: "maintenance_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_maintenance_item_order_id",
                schema: "fleet",
                table: "maintenance_item",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_maintenance_order_vehicle_id",
                schema: "fleet",
                table: "maintenance_order",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_maintenance_order_vendor_id",
                schema: "fleet",
                table: "maintenance_order",
                column: "vendor_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "maintenance_item",
                schema: "fleet");

            migrationBuilder.DropTable(
                name: "dc_maintenance_type",
                schema: "fleet");

            migrationBuilder.DropTable(
                name: "maintenance_order",
                schema: "fleet");

            migrationBuilder.DropTable(
                name: "vendor",
                schema: "fleet");
        }
    }
}
