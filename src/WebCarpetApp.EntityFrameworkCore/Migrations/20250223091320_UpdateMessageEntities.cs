using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCarpetApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessageEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "MessageConfigurations");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "MessageConfigurations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MessageConfigurations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "MessageConfigurations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "MessageConfigurations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MessageConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
