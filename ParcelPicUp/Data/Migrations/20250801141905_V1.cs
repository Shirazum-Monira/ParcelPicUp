using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParcelPicUp.Data.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parcel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DeliveryAgentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReceiverName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parcel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parcel_AspNetUsers_DeliveryAgentId",
                        column: x => x.DeliveryAgentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parcel_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parcel_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parcel_DeliveryAgentId",
                table: "Parcel",
                column: "DeliveryAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcel_ReceiverId",
                table: "Parcel",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Parcel_SenderId",
                table: "Parcel",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parcel");
        }
    }
}
