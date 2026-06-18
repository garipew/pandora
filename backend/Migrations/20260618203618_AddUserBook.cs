using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pandora.Migrations
{
    /// <inheritdoc />
    public partial class AddUserBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_Users_OwnerId",
                table: "Boxes");

            migrationBuilder.DropIndex(
                name: "IX_Books_AuthorId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PagesRead",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Boxes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Boxes_OwnerId",
                table: "Boxes",
                newName: "IX_Boxes_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_Users_UserId",
                table: "Boxes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_Users_UserId",
                table: "Boxes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Boxes",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Boxes_UserId",
                table: "Boxes",
                newName: "IX_Boxes_OwnerId");

            migrationBuilder.AddColumn<int>(
                name: "PagesRead",
                table: "Entries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Entries",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Entries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Books",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Authors_AuthorId",
                table: "Books",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_Users_OwnerId",
                table: "Boxes",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
