using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusTicketAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Firmalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirmaAdi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firmalar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sehirler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SehirAdi = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sehirler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cinsiyet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Otobusler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plaka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KoltukSayisi = table.Column<int>(type: "int", nullable: false),
                    OtobusModel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirmaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Otobusler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Otobusler_Firmalar_FirmaId",
                        column: x => x.FirmaId,
                        principalTable: "Firmalar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Seferler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KalkisSehirId = table.Column<int>(type: "int", nullable: false),
                    VarisSehirId = table.Column<int>(type: "int", nullable: false),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Saat = table.Column<TimeSpan>(type: "time", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FirmaId = table.Column<int>(type: "int", nullable: false),
                    OtobusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seferler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seferler_Firmalar_FirmaId",
                        column: x => x.FirmaId,
                        principalTable: "Firmalar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Seferler_Otobusler_OtobusId",
                        column: x => x.OtobusId,
                        principalTable: "Otobusler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Seferler_Sehirler_KalkisSehirId",
                        column: x => x.KalkisSehirId,
                        principalTable: "Sehirler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Seferler_Sehirler_VarisSehirId",
                        column: x => x.VarisSehirId,
                        principalTable: "Sehirler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Biletler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SeferId = table.Column<int>(type: "int", nullable: false),
                    KoltukNumarasi = table.Column<int>(type: "int", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SatinAlmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biletler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Biletler_Seferler_SeferId",
                        column: x => x.SeferId,
                        principalTable: "Seferler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Biletler_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Koltuklar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeferId = table.Column<int>(type: "int", nullable: false),
                    KoltukNumarasi = table.Column<int>(type: "int", nullable: false),
                    Dolu = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Koltuklar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Koltuklar_Seferler_SeferId",
                        column: x => x.SeferId,
                        principalTable: "Seferler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Koltuklar_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeferDuraklari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeferId = table.Column<int>(type: "int", nullable: false),
                    Durak = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false),
                    Fiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeferDuraklari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeferDuraklari_Seferler_SeferId",
                        column: x => x.SeferId,
                        principalTable: "Seferler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_SeferId",
                table: "Biletler",
                column: "SeferId");

            migrationBuilder.CreateIndex(
                name: "IX_Biletler_UserId",
                table: "Biletler",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Koltuklar_SeferId",
                table: "Koltuklar",
                column: "SeferId");

            migrationBuilder.CreateIndex(
                name: "IX_Koltuklar_UserId",
                table: "Koltuklar",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Otobusler_FirmaId",
                table: "Otobusler",
                column: "FirmaId");

            migrationBuilder.CreateIndex(
                name: "IX_SeferDuraklari_SeferId",
                table: "SeferDuraklari",
                column: "SeferId");

            migrationBuilder.CreateIndex(
                name: "IX_Seferler_FirmaId",
                table: "Seferler",
                column: "FirmaId");

            migrationBuilder.CreateIndex(
                name: "IX_Seferler_KalkisSehirId",
                table: "Seferler",
                column: "KalkisSehirId");

            migrationBuilder.CreateIndex(
                name: "IX_Seferler_OtobusId",
                table: "Seferler",
                column: "OtobusId");

            migrationBuilder.CreateIndex(
                name: "IX_Seferler_VarisSehirId",
                table: "Seferler",
                column: "VarisSehirId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Biletler");

            migrationBuilder.DropTable(
                name: "Koltuklar");

            migrationBuilder.DropTable(
                name: "SeferDuraklari");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Seferler");

            migrationBuilder.DropTable(
                name: "Otobusler");

            migrationBuilder.DropTable(
                name: "Sehirler");

            migrationBuilder.DropTable(
                name: "Firmalar");
        }
    }
}
