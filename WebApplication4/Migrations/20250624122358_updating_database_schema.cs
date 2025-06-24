using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApplication4.Migrations
{
    /// <inheritdoc />
    public partial class updating_database_schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notes_groups_group_id",
                table: "notes");

            migrationBuilder.DropTable(
                name: "user_to_group");

            migrationBuilder.RenameColumn(
                name: "text",
                table: "notes",
                newName: "content");

            migrationBuilder.AddColumn<long>(
                name: "GroupId",
                table: "users",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "group_id",
                table: "notes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "group_membership",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    group_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_group_membership", x => x.Id);
                    table.ForeignKey(
                        name: "FK_group_membership_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_group_membership_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_GroupId",
                table: "users",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_group_membership_group_id",
                table: "group_membership",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_group_membership_user_id",
                table: "group_membership",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_notes_groups_group_id",
                table: "notes",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_groups_GroupId",
                table: "users",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notes_groups_group_id",
                table: "notes");

            migrationBuilder.DropForeignKey(
                name: "FK_users_groups_GroupId",
                table: "users");

            migrationBuilder.DropTable(
                name: "group_membership");

            migrationBuilder.DropIndex(
                name: "IX_users_GroupId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "notes",
                newName: "text");

            migrationBuilder.AlterColumn<long>(
                name: "group_id",
                table: "notes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "user_to_group",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_to_group", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_to_group_groups_group_id",
                        column: x => x.group_id,
                        principalTable: "groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_to_group_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_to_group_group_id",
                table: "user_to_group",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_to_group_user_id",
                table: "user_to_group",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_notes_groups_group_id",
                table: "notes",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "Id");
        }
    }
}
