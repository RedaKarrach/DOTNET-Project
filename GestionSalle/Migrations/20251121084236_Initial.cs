using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionSalle.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanAbonnement",
                columns: table => new
                {
                    idPlan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    prix = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    dureeEnMois = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PlanAbon__BECFB99658F303DF", x => x.idPlan);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateur",
                columns: table => new
                {
                    idUtilisateur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nomUtilisateur = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    motDePasse = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dateCreation = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Utilisat__5366DB1936444C3D", x => x.idUtilisateur);
                });

            migrationBuilder.CreateTable(
                name: "Entraineur",
                columns: table => new
                {
                    idEntraineur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nomComplet = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    specialite = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    statut = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Actif"),
                    idUtilisateur = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Entraine__72EED92B4C108FDE", x => x.idEntraineur);
                    table.ForeignKey(
                        name: "FK__Entraineu__idUti__76969D2E",
                        column: x => x.idUtilisateur,
                        principalTable: "Utilisateur",
                        principalColumn: "idUtilisateur");
                });

            migrationBuilder.CreateTable(
                name: "Membre",
                columns: table => new
                {
                    idMembre = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nomComplet = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    dateInscription = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    sexe = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    adresse = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    idPlan = table.Column<int>(type: "int", nullable: true),
                    idEntraineur = table.Column<int>(type: "int", nullable: true),
                    statut = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Actif"),
                    idUtilisateur = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Membre__7399C5D7C144F10B", x => x.idMembre);
                    table.ForeignKey(
                        name: "FK__Membre__idEntrai__7F2BE32F",
                        column: x => x.idEntraineur,
                        principalTable: "Entraineur",
                        principalColumn: "idEntraineur");
                    table.ForeignKey(
                        name: "FK__Membre__idPlan__7E37BEF6",
                        column: x => x.idPlan,
                        principalTable: "PlanAbonnement",
                        principalColumn: "idPlan");
                    table.ForeignKey(
                        name: "FK__Membre__idUtilis__00200768",
                        column: x => x.idUtilisateur,
                        principalTable: "Utilisateur",
                        principalColumn: "idUtilisateur");
                });

            migrationBuilder.CreateTable(
                name: "Paiement",
                columns: table => new
                {
                    idPaiement = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idMembre = table.Column<int>(type: "int", nullable: false),
                    montant = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    datePaiement = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    methode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Paiement__D44CB63D57A3FA4F", x => x.idPaiement);
                    table.ForeignKey(
                        name: "FK__Paiement__idMemb__0E6E26BF",
                        column: x => x.idMembre,
                        principalTable: "Membre",
                        principalColumn: "idMembre");
                });

            migrationBuilder.CreateTable(
                name: "Seance",
                columns: table => new
                {
                    idSeance = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idMembre = table.Column<int>(type: "int", nullable: true),
                    idEntraineur = table.Column<int>(type: "int", nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Seance__7CD10F7DD39F6B82", x => x.idSeance);
                    table.ForeignKey(
                        name: "FK__Seance__idEntrai__03F0984C",
                        column: x => x.idEntraineur,
                        principalTable: "Entraineur",
                        principalColumn: "idEntraineur");
                    table.ForeignKey(
                        name: "FK__Seance__idMembre__02FC7413",
                        column: x => x.idMembre,
                        principalTable: "Membre",
                        principalColumn: "idMembre");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entraineur_idUtilisateur",
                table: "Entraineur",
                column: "idUtilisateur");

            migrationBuilder.CreateIndex(
                name: "UQ__Entraine__AB6E61647760B8E4",
                table: "Entraineur",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Membre_Email",
                table: "Membre",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_Membre_IdEntraineur",
                table: "Membre",
                column: "idEntraineur");

            migrationBuilder.CreateIndex(
                name: "IX_Membre_idPlan",
                table: "Membre",
                column: "idPlan");

            migrationBuilder.CreateIndex(
                name: "IX_Membre_idUtilisateur",
                table: "Membre",
                column: "idUtilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_Membre_Statut",
                table: "Membre",
                column: "statut");

            migrationBuilder.CreateIndex(
                name: "UQ__Membre__AB6E616470938203",
                table: "Membre",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paiement_Date",
                table: "Paiement",
                column: "datePaiement");

            migrationBuilder.CreateIndex(
                name: "IX_Paiement_IdMembre",
                table: "Paiement",
                column: "idMembre");

            migrationBuilder.CreateIndex(
                name: "IX_Seance_Date",
                table: "Seance",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "IX_Seance_idEntraineur",
                table: "Seance",
                column: "idEntraineur");

            migrationBuilder.CreateIndex(
                name: "IX_Seance_idMembre",
                table: "Seance",
                column: "idMembre");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateur_Role",
                table: "Utilisateur",
                column: "role");

            migrationBuilder.CreateIndex(
                name: "UQ__Utilisat__BD52466D01D110EF",
                table: "Utilisateur",
                column: "nomUtilisateur",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Paiement");

            migrationBuilder.DropTable(
                name: "Seance");

            migrationBuilder.DropTable(
                name: "Membre");

            migrationBuilder.DropTable(
                name: "Entraineur");

            migrationBuilder.DropTable(
                name: "PlanAbonnement");

            migrationBuilder.DropTable(
                name: "Utilisateur");
        }
    }
}
