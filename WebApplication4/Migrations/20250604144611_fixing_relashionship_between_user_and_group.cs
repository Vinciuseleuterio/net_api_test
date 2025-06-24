using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication4.Migrations
{
    /// <inheritdoc />
    public partial class fixing_relashionship_between_user_and_group : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_groups_group_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_group_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "group_id",
                table: "users");

            migrationBuilder.CreateIndex(
                name: "IX_groups_creator_id",
                table: "groups",
                column: "creator_id");

            migrationBuilder.AddForeignKey(
                name: "FK_groups_users_creator_id",
                table: "groups",
                column: "creator_id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_groups_users_creator_id",
                table: "groups");

            migrationBuilder.DropIndex(
                name: "IX_groups_creator_id",
                table: "groups");

            migrationBuilder.AddColumn<long>(
                name: "group_id",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_users_group_id",
                table: "users",
                column: "group_id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_groups_group_id",
                table: "users",
                column: "group_id",
                principalTable: "groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
