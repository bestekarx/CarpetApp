using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCarpetApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompanyModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "MessagedPhone",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MessageLogs");

            migrationBuilder.RenameColumn(
                name: "MessageSuccessfullySend",
                table: "MessageLogs",
                newName: "IsSent");

            migrationBuilder.RenameColumn(
                name: "MessageContent",
                table: "MessageLogs",
                newName: "Message");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "MessageLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "MessageTemplateId",
                table: "MessageLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MessageUserId",
                table: "MessageLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedAt",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "MessageSettingsId",
                table: "Companies",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "MessageTemplateId",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "MessageUserId",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "ConfirmedAt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "MessageLogs",
                newName: "MessageContent");

            migrationBuilder.RenameColumn(
                name: "IsSent",
                table: "MessageLogs",
                newName: "MessageSuccessfullySend");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "MessageLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "MessageLogs",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessagedPhone",
                table: "MessageLogs",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "MessageLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MessageSettingsId",
                table: "Companies",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
