using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCarpetApp.Migrations
{
    /// <inheritdoc />
    public partial class ReceivedType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Receiveds",
                type: "int",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 3. CarpetTenantId sütununu kaldır
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Receiveds");
        }
    }
}
