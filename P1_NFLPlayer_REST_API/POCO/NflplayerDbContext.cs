using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace P1_NFLPlayer_REST_API.POCO;

public partial class NflplayerDbContext : DbContext
{
    public NflplayerDbContext()
    {
    }

    public NflplayerDbContext(DbContextOptions<NflplayerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Quarterback> Quarterbacks { get; set; }

    public virtual DbSet<Stat> Stats { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=localhost; database=NFLPlayerDB; integrated security = true; TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PK__Contract__C90D3469C4C5CC49");

            entity.ToTable("Contract", tb => tb.HasTrigger("trgUpdateQuarterbackTeam"));

            entity.Property(e => e.Qbid).HasColumnName("QBId");
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.SalaryFormatted)
                .HasMaxLength(31)
                .IsUnicode(false)
                .HasComputedColumnSql("('$'+CONVERT([varchar],[Salary],(1)))", false);

            entity.HasOne(d => d.Qb).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.Qbid)
                .HasConstraintName("FK__Contract__QBId__47DBAE45");

            entity.HasOne(d => d.Team).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_Contract_TeamId");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PK__Game__2AB897FD6DF8E6AE");

            entity.ToTable("Game");

            entity.Property(e => e.Stadium).IsUnicode(false);
        });

        modelBuilder.Entity<Quarterback>(entity =>
        {
            entity.HasKey(e => e.Qbid).HasName("PK__Quarterb__DFE79D998F12E367");

            entity.ToTable("Quarterback");

            entity.Property(e => e.Qbid).HasColumnName("QBId");
            entity.Property(e => e.Image).IsUnicode(false);
            entity.Property(e => e.Name).IsUnicode(false);

            entity.HasOne(d => d.Team).WithMany(p => p.Quarterbacks)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_Quarterbacks_TeamId");
        });

        modelBuilder.Entity<Stat>(entity =>
        {
            entity.HasKey(e => e.StatId).HasName("PK__Stats__3A162D3E86A332C6");

            entity.Property(e => e.Qbid).HasColumnName("QBId");

            entity.HasOne(d => d.Game).WithMany(p => p.Stats)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK__Stats__GameId__403A8C7D");

            entity.HasOne(d => d.Qb).WithMany(p => p.Stats)
                .HasForeignKey(d => d.Qbid)
                .HasConstraintName("FK__Stats__QBId__412EB0B6");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PK__Team__123AE799B9BF3835");

            entity.ToTable("Team");

            entity.Property(e => e.City).IsUnicode(false);
            entity.Property(e => e.Logo).IsUnicode(false);
            entity.Property(e => e.Name).IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
