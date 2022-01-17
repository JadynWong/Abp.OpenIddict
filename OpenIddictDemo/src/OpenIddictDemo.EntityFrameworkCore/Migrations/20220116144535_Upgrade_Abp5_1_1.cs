﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenIddictDemo.Migrations;

public partial class Upgrade_Abp5_1_1 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_AbpSettings_Name_ProviderName_ProviderKey",
            table: "AbpSettings");

        migrationBuilder.DropIndex(
            name: "IX_AbpPermissionGrants_Name_ProviderName_ProviderKey",
            table: "AbpPermissionGrants");

        migrationBuilder.DropIndex(
            name: "IX_AbpFeatureValues_Name_ProviderName_ProviderKey",
            table: "AbpFeatureValues");

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "AbpUsers",
            type: "bit",
            nullable: false,
            defaultValue: true);

        migrationBuilder.AlterColumn<string>(
            name: "TenantName",
            table: "AbpAuditLogs",
            type: "nvarchar(64)",
            maxLength: 64,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ImpersonatorTenantName",
            table: "AbpAuditLogs",
            type: "nvarchar(64)",
            maxLength: 64,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ImpersonatorUserName",
            table: "AbpAuditLogs",
            type: "nvarchar(256)",
            maxLength: 256,
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_AbpSettings_Name_ProviderName_ProviderKey",
            table: "AbpSettings",
            columns: new[] { "Name", "ProviderName", "ProviderKey" },
            unique: true,
            filter: "[ProviderName] IS NOT NULL AND [ProviderKey] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_AbpPermissionGrants_TenantId_Name_ProviderName_ProviderKey",
            table: "AbpPermissionGrants",
            columns: new[] { "TenantId", "Name", "ProviderName", "ProviderKey" },
            unique: true,
            filter: "[TenantId] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_AbpFeatureValues_Name_ProviderName_ProviderKey",
            table: "AbpFeatureValues",
            columns: new[] { "Name", "ProviderName", "ProviderKey" },
            unique: true,
            filter: "[ProviderName] IS NOT NULL AND [ProviderKey] IS NOT NULL");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_AbpSettings_Name_ProviderName_ProviderKey",
            table: "AbpSettings");

        migrationBuilder.DropIndex(
            name: "IX_AbpPermissionGrants_TenantId_Name_ProviderName_ProviderKey",
            table: "AbpPermissionGrants");

        migrationBuilder.DropIndex(
            name: "IX_AbpFeatureValues_Name_ProviderName_ProviderKey",
            table: "AbpFeatureValues");

        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "AbpUsers");

        migrationBuilder.DropColumn(
            name: "ImpersonatorTenantName",
            table: "AbpAuditLogs");

        migrationBuilder.DropColumn(
            name: "ImpersonatorUserName",
            table: "AbpAuditLogs");

        migrationBuilder.AlterColumn<string>(
            name: "TenantName",
            table: "AbpAuditLogs",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(64)",
            oldMaxLength: 64,
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_AbpSettings_Name_ProviderName_ProviderKey",
            table: "AbpSettings",
            columns: new[] { "Name", "ProviderName", "ProviderKey" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpPermissionGrants_Name_ProviderName_ProviderKey",
            table: "AbpPermissionGrants",
            columns: new[] { "Name", "ProviderName", "ProviderKey" });

        migrationBuilder.CreateIndex(
            name: "IX_AbpFeatureValues_Name_ProviderName_ProviderKey",
            table: "AbpFeatureValues",
            columns: new[] { "Name", "ProviderName", "ProviderKey" });
    }
}
