using Microsoft.EntityFrameworkCore;
using RabbitConsumer.Interface;
using RabbitConsumer.Repositories.Models;

namespace RabbitConsumer.Repositories;

public partial class RabbitTestContext : DbContext, IDbContext

{
    public RabbitTestContext()
    {
    }

    public RabbitTestContext(DbContextOptions<RabbitTestContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Organization> Organizations { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseNpgsql("Host=192.168.108.199;Database=AlarmControlSystem;Username=postgres;Password=!Q2wAZsx");
        //            }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.ToTable("Organization");

            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

        });

        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}