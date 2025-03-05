using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCarpetApp.Migrations
{
    /// <inheritdoc />
    public partial class UserTenantMappingDeleteTenantId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Yeni CarpetTenantId sütunu ekle
            migrationBuilder.AddColumn<Guid>(
                name: "CarpetTenantId",
                table: "UserTenantMappings",
                type: "uniqueidentifier",
                nullable: true);

            // 2. TenantId değerlerini CarpetTenantId'ye kopyala
            migrationBuilder.Sql("UPDATE UserTenantMappings SET CarpetTenantId = TenantId");

            // 3. TenantId sütununu kaldır
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "UserTenantMappings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. TenantId sütununu geri ekle
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "UserTenantMappings",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: null);

            // 2. CarpetTenantId değerlerini TenantId'ye kopyala
            migrationBuilder.Sql("UPDATE UserTenantMappings SET TenantId = CarpetTenantId");

            // 3. CarpetTenantId sütununu kaldır
            migrationBuilder.DropColumn(
                name: "CarpetTenantId",
                table: "UserTenantMappings");
        }
    }
}
