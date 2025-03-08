using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCarpetApp.Migrations
{
    /// <inheritdoc />
    public partial class RenameReceiveDateProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename PurchaseDate to PickupDate
            migrationBuilder.RenameColumn(
                name: "PurchaseDate",
                table: "Receiveds",
                newName: "PickupDate");

            // Rename ReceivedDate to DeliveryDate
            migrationBuilder.RenameColumn(
                name: "ReceivedDate",
                table: "Receiveds",
                newName: "DeliveryDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rename PickupDate back to PurchaseDate
            migrationBuilder.RenameColumn(
                name: "PickupDate",
                table: "Receiveds",
                newName: "PurchaseDate");

            // Rename DeliveryDate back to ReceivedDate
            migrationBuilder.RenameColumn(
                name: "DeliveryDate",
                table: "Receiveds",
                newName: "ReceivedDate");
        }
    }
}
