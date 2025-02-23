using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCarpetApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCompanySchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Mevcut int değerlerini varsayılan bir GUID ile güncelle
            migrationBuilder.Sql(@"
        UPDATE [Companies]
        SET [MessageSettingsId] = '00000000-0000-0000-0000-000000000000'
        WHERE [MessageSettingsId] IS NOT NULL;
    ");

            migrationBuilder.AlterColumn<Guid>(
                name: "MessageSettingsId",
                table: "Companies",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
