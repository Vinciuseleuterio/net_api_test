using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NotesApp.Migrations
{
    /// <inheritdoc />
    public partial class adding_multiple_primary_key_to_group_membership_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_group_membership",
                table: "group_membership");

            migrationBuilder.DropIndex(
                name: "IX_group_membership_user_id",
                table: "group_membership");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "group_membership",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_group_membership",
                table: "group_membership",
                columns: new[] { "user_id", "group_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_group_membership",
                table: "group_membership");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "group_membership",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_group_membership",
                table: "group_membership",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_group_membership_user_id",
                table: "group_membership",
                column: "user_id");
        }
    }
}
