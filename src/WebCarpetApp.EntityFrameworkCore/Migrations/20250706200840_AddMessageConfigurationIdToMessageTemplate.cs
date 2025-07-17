using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCarpetApp.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageConfigurationIdToMessageTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MessageConfigurationId",
                table: "MessageTemplates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MessageTemplates_MessageConfigurationId",
                table: "MessageTemplates",
                column: "MessageConfigurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageTemplates_MessageConfigurations_MessageConfigurationId",
                table: "MessageTemplates",
                column: "MessageConfigurationId",
                principalTable: "MessageConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageTemplates_MessageConfigurations_MessageConfigurationId",
                table: "MessageTemplates");

            migrationBuilder.DropIndex(
                name: "IX_MessageTemplates_MessageConfigurationId",
                table: "MessageTemplates");

            migrationBuilder.DropColumn(
                name: "MessageConfigurationId",
                table: "MessageTemplates");
        }
    }
}
