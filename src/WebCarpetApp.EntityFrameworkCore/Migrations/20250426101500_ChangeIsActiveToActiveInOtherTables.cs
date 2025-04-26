using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCarpetApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIsActiveToActiveInOtherTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // MessageConfigurations tablosundaki IsActive alanını Active olarak değiştir
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "MessageConfigurations",
                newName: "Active");

            // MessageTasks tablosundaki IsActive alanını Active olarak değiştir
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "MessageTasks",
                newName: "Active");

            // MessageTemplates tablosundaki IsActive alanını Active olarak değiştir
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "MessageTemplates",
                newName: "Active");

            // MessageUsers tablosundaki IsActive alanını Active olarak değiştir
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "MessageUsers",
                newName: "Active");

            // UserTenantMappings tablosundaki IsActive alanını Active olarak değiştir
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "UserTenantMappings",
                newName: "Active");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // MessageConfigurations tablosundaki Active alanını IsActive olarak geri değiştir
            migrationBuilder.RenameColumn(
                name: "Active",
                table: "MessageConfigurations",
                newName: "IsActive");

            // MessageTasks tablosundaki Active alanını IsActive olarak geri değiştir
            migrationBuilder.RenameColumn(
                name: "Active",
                table: "MessageTasks",
                newName: "IsActive");

            // MessageTemplates tablosundaki Active alanını IsActive olarak geri değiştir
            migrationBuilder.RenameColumn(
                name: "Active",
                table: "MessageTemplates",
                newName: "IsActive");

            // MessageUsers tablosundaki Active alanını IsActive olarak geri değiştir
            migrationBuilder.RenameColumn(
                name: "Active",
                table: "MessageUsers",
                newName: "IsActive");

            // UserTenantMappings tablosundaki Active alanını IsActive olarak geri değiştir
            migrationBuilder.RenameColumn(
                name: "Active",
                table: "UserTenantMappings",
                newName: "IsActive");
        }
    }
}
