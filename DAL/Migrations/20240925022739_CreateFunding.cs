using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class CreateFunding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "trn_funding",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    lender_id = table.Column<string>(type: "text", nullable: false),
                    loan_id = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    funded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MstLoansId = table.Column<string>(type: "text", nullable: true),
                    MstUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trn_funding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_trn_funding_mst_loans_MstLoansId",
                        column: x => x.MstLoansId,
                        principalTable: "mst_loans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_trn_funding_mst_user_MstUserId",
                        column: x => x.MstUserId,
                        principalTable: "mst_user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_trn_funding_MstLoansId",
                table: "trn_funding",
                column: "MstLoansId");

            migrationBuilder.CreateIndex(
                name: "IX_trn_funding_MstUserId",
                table: "trn_funding",
                column: "MstUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trn_funding");
        }
    }
}
