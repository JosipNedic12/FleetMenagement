using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FleetManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop pre-existing table created outside of migrations
            migrationBuilder.Sql("DROP TABLE IF EXISTS fleet.vehicle_document CASCADE;");

            migrationBuilder.CreateTable(
                name: "vehicle_document",
                schema: "fleet",
                columns: table => new
                {
                    vehicle_document_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vehicle_id = table.Column<int>(type: "integer", nullable: false),
                    document_id = table.Column<int>(type: "integer", nullable: false),
                    document_type_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_document", x => x.vehicle_document_id);
                    table.ForeignKey(
                        name: "FK_vehicle_document_document_document_id",
                        column: x => x.document_id,
                        principalSchema: "fleet",
                        principalTable: "document",
                        principalColumn: "document_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vehicle_document_vehicle_vehicle_id",
                        column: x => x.vehicle_id,
                        principalSchema: "fleet",
                        principalTable: "vehicle",
                        principalColumn: "vehicle_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_document_document_id",
                schema: "fleet",
                table: "vehicle_document",
                column: "document_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_document_vehicle_id",
                schema: "fleet",
                table: "vehicle_document",
                column: "vehicle_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vehicle_document",
                schema: "fleet");
        }
    }
}
