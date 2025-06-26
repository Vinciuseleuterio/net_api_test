using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesApp.Migrations
{
    /// <inheritdoc />
    public partial class defining_group_id_in_note_to_be_nullable : Migration
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
