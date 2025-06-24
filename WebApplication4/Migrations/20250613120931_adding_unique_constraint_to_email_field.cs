using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication4.Migrations
{
    /// <inheritdoc />
    public partial class adding_unique_constraint_to_email_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notes_groups_group_id",
                table: "notes");

            migrationBuilder.AlterColumn<long>(
                name: "group_id",
                table: "notes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_notes_groups_group_id",
                table: "notes",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notes_groups_group_id",
                table: "notes");

            migrationBuilder.DropIndex(
                name: "IX_users_email",
                table: "users");

            migrationBuilder.AlterColumn<long>(
                name: "group_id",
                table: "notes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_notes_groups_group_id",
                table: "notes",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
