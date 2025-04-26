using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCarpetApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIsActiveToActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // AbpUsers tablosundaki IsActive alanını Active olarak değiştir
            migrationBuilder.Sql("EXEC sp_rename '[AbpUsers].[IsActive]', 'Active', 'COLUMN'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // AbpUsers tablosundaki Active alanını IsActive olarak geri değiştir
            migrationBuilder.Sql("EXEC sp_rename '[AbpUsers].[Active]', 'IsActive', 'COLUMN'");
        }
    }
} 