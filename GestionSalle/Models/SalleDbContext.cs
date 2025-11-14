using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GestionSalle.Models;

public partial class SalleDbContext : DbContext
{
    public SalleDbContext()
    {
    }

    public SalleDbContext(DbContextOptions<SalleDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Entraineur> Entraineurs { get; set; }

    public virtual DbSet<Membre> Membres { get; set; }

    public virtual DbSet<Paiement> Paiements { get; set; }

    public virtual DbSet<PlanAbonnement> PlanAbonnements { get; set; }

    public virtual DbSet<Seance> Seances { get; set; }

    public virtual DbSet<Utilisateur> Utilisateurs { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=salle_db;Trusted_Connection=True;TrustServerCertificate=True;");
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entraineur>(entity =>
        {
            entity.HasKey(e => e.IdEntraineur).HasName("PK__Entraine__72EED92B4C108FDE");

            entity.ToTable("Entraineur");

            entity.HasIndex(e => e.Email, "UQ__Entraine__AB6E61647760B8E4").IsUnique();

            entity.Property(e => e.IdEntraineur).HasColumnName("idEntraineur");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IdUtilisateur).HasColumnName("idUtilisateur");
            entity.Property(e => e.NomComplet)
                .HasMaxLength(200)
                .HasColumnName("nomComplet");
            entity.Property(e => e.Specialite)
                .HasMaxLength(100)
                .HasColumnName("specialite");
            entity.Property(e => e.Statut)
                .HasMaxLength(50)
                .HasDefaultValue("Actif")
                .HasColumnName("statut");
            entity.Property(e => e.Telephone)
                .HasMaxLength(20)
                .HasColumnName("telephone");

            entity.HasOne(d => d.IdUtilisateurNavigation).WithMany(p => p.Entraineurs)
                .HasForeignKey(d => d.IdUtilisateur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Entraineu__idUti__76969D2E");
        });

        modelBuilder.Entity<Membre>(entity =>
        {
            entity.HasKey(e => e.IdMembre).HasName("PK__Membre__7399C5D7C144F10B");

            entity.ToTable("Membre");

            entity.HasIndex(e => e.Email, "IX_Membre_Email");

            entity.HasIndex(e => e.IdEntraineur, "IX_Membre_IdEntraineur");

            entity.HasIndex(e => e.Statut, "IX_Membre_Statut");

            entity.HasIndex(e => e.Email, "UQ__Membre__AB6E616470938203").IsUnique();

            entity.Property(e => e.IdMembre).HasColumnName("idMembre");
            entity.Property(e => e.Adresse)
                .HasMaxLength(300)
                .HasColumnName("adresse");
            entity.Property(e => e.DateInscription)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateInscription");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.IdEntraineur).HasColumnName("idEntraineur");
            entity.Property(e => e.IdPlan).HasColumnName("idPlan");
            entity.Property(e => e.IdUtilisateur).HasColumnName("idUtilisateur");
            entity.Property(e => e.NomComplet)
                .HasMaxLength(200)
                .HasColumnName("nomComplet");
            entity.Property(e => e.Sexe)
                .HasMaxLength(10)
                .HasColumnName("sexe");
            entity.Property(e => e.Statut)
                .HasMaxLength(50)
                .HasDefaultValue("Actif")
                .HasColumnName("statut");
            entity.Property(e => e.Telephone)
                .HasMaxLength(20)
                .HasColumnName("telephone");

            entity.HasOne(d => d.IdEntraineurNavigation).WithMany(p => p.Membres)
                .HasForeignKey(d => d.IdEntraineur)
                .HasConstraintName("FK__Membre__idEntrai__7F2BE32F");

            entity.HasOne(d => d.IdPlanNavigation).WithMany(p => p.Membres)
                .HasForeignKey(d => d.IdPlan)
                .HasConstraintName("FK__Membre__idPlan__7E37BEF6");

            entity.HasOne(d => d.IdUtilisateurNavigation).WithMany(p => p.Membres)
                .HasForeignKey(d => d.IdUtilisateur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Membre__idUtilis__00200768");
        });

        modelBuilder.Entity<Paiement>(entity =>
        {
            entity.HasKey(e => e.IdPaiement).HasName("PK__Paiement__D44CB63D57A3FA4F");

            entity.ToTable("Paiement");

            entity.HasIndex(e => e.DatePaiement, "IX_Paiement_Date");

            entity.HasIndex(e => e.IdMembre, "IX_Paiement_IdMembre");

            entity.Property(e => e.IdPaiement).HasColumnName("idPaiement");
            entity.Property(e => e.DatePaiement)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("datePaiement");
            entity.Property(e => e.IdMembre).HasColumnName("idMembre");
            entity.Property(e => e.Methode)
                .HasMaxLength(50)
                .HasColumnName("methode");
            entity.Property(e => e.Montant)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("montant");

            entity.HasOne(d => d.IdMembreNavigation).WithMany(p => p.Paiements)
                .HasForeignKey(d => d.IdMembre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Paiement__idMemb__0E6E26BF");
        });

        modelBuilder.Entity<PlanAbonnement>(entity =>
        {
            entity.HasKey(e => e.IdPlan).HasName("PK__PlanAbon__BECFB99658F303DF");

            entity.ToTable("PlanAbonnement");

            entity.Property(e => e.IdPlan).HasColumnName("idPlan");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.DureeEnMois).HasColumnName("dureeEnMois");
            entity.Property(e => e.Nom)
                .HasMaxLength(100)
                .HasColumnName("nom");
            entity.Property(e => e.Prix)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("prix");
        });

        modelBuilder.Entity<Seance>(entity =>
        {
            entity.HasKey(e => e.IdSeance).HasName("PK__Seance__7CD10F7DD39F6B82");

            entity.ToTable("Seance");

            entity.HasIndex(e => e.Date, "IX_Seance_Date");

            entity.Property(e => e.IdSeance).HasColumnName("idSeance");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.IdEntraineur).HasColumnName("idEntraineur");
            entity.Property(e => e.IdMembre).HasColumnName("idMembre");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasColumnName("type");

            entity.HasOne(d => d.IdEntraineurNavigation).WithMany(p => p.Seances)
                .HasForeignKey(d => d.IdEntraineur)
                .HasConstraintName("FK__Seance__idEntrai__03F0984C");

            entity.HasOne(d => d.IdMembreNavigation).WithMany(p => p.Seances)
                .HasForeignKey(d => d.IdMembre)
                .HasConstraintName("FK__Seance__idMembre__02FC7413");
        });

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.IdUtilisateur).HasName("PK__Utilisat__5366DB1936444C3D");

            entity.ToTable("Utilisateur");

            entity.HasIndex(e => e.Role, "IX_Utilisateur_Role");

            entity.HasIndex(e => e.NomUtilisateur, "UQ__Utilisat__BD52466D01D110EF").IsUnique();

            entity.Property(e => e.IdUtilisateur).HasColumnName("idUtilisateur");
            entity.Property(e => e.DateCreation)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreation");
            entity.Property(e => e.MotDePasse)
                .HasMaxLength(255)
                .HasColumnName("motDePasse");
            entity.Property(e => e.NomUtilisateur)
                .HasMaxLength(100)
                .HasColumnName("nomUtilisateur");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
