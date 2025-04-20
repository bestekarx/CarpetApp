using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCarpetApp.Migrations
{
    public partial class CompanyMessageSettingsIdDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "MessageSettingsId", table: "Companies");
        }
    }
}
