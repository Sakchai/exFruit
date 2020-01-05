using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MissionControl.Shared;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace MissionControl.Services
{
    public class ApplicationDbContext : IdentityDbContext, IDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<Reception> Receptions { get; set; }
        public DbSet<Sortation> Sortations { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<Product> Products { get; set; }

        public void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityEntry = Entry(entity);
            if (entityEntry == null)
                return;

            //set the entity is not being tracked by the context
            entityEntry.State = EntityState.Detached;
        }

        public IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseEntity
        {
            return null; //Set<TEntity>().FromSql(CreateSqlWithParameters(sql, parameters), parameters);
        }

        public virtual new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }
        public int ExecuteSqlCommand(RawSqlString sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            //set specific command timeout
            var previousTimeout = Database.GetCommandTimeout();
            Database.SetCommandTimeout(timeout);

            var result = 0;
            if (!doNotEnsureTransaction)
            {
                //use with transaction
                using (var transaction = Database.BeginTransaction())
                {
                    result = Database.ExecuteSqlCommand(sql, parameters);
                    transaction.Commit();
                }
            }
            else
                result = Database.ExecuteSqlCommand(sql, parameters);

            //return previous timeout back
            Database.SetCommandTimeout(previousTimeout);

            return result;
        }

        public string GenerateCreateScript()
        {
            return string.Empty;
        }

        [Obsolete]
        public IQueryable<TQuery> QueryFromSql<TQuery>(string sql, params object[] parameters) where TQuery : class
        {
            return Query<TQuery>().FromSql(CreateSqlWithParameters(sql, parameters), parameters);
        }

        protected virtual string CreateSqlWithParameters(string sql, params object[] parameters)
        {
            //add parameters to sql
            for (var i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
            {
                if (!(parameters[i] is DbParameter parameter))
                    continue;

                sql = $"{sql}{(i > 0 ? "," : string.Empty)} @{parameter.ParameterName}";

                //whether parameter is output
                if (parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Output)
                    sql = $"{sql} output";
            }

            return sql;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable(nameof(Product));
            modelBuilder.Entity<Vendor>().ToTable(nameof(Vendor));
            modelBuilder.Entity<Reception>().ToTable(nameof(Reception));
            modelBuilder.Entity<Reception>().HasKey(r => r.Id);
            modelBuilder.Entity<Sortation>().ToTable(nameof(Sortation));
            modelBuilder.Entity<Sortation>().HasKey(s => s.Id);

            modelBuilder.Entity<Purchase>().ToTable(nameof(Purchase));
            modelBuilder.Entity<Purchase>().HasKey(p => p.Id);
            modelBuilder.Entity<Purchase>().Property(p => p.TotalWeightKg).HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<Purchase>().Property(p => p.PurchaseTotal).HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Purchase>().HasOne(purchase => purchase.Vendor)
                              .WithMany()
                              .HasForeignKey(purchase => purchase.VendorId);
            modelBuilder.Entity<Purchase>().Ignore(purchase => purchase.PurchaseStatus);
            modelBuilder.Entity<Purchase>().Ignore(purchase => purchase.PurchaseProcess);
            // Purchase Item
            modelBuilder.Entity<PurchaseItem>().ToTable(nameof(PurchaseItem));
            modelBuilder.Entity<PurchaseItem>().HasKey(p => p.Id);
            modelBuilder.Entity<PurchaseItem>().Property(purchaseItem => purchaseItem.UnitPriceInclTax).HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<PurchaseItem>().Property(purchaseItem => purchaseItem.UnitPriceExclTax).HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<PurchaseItem>().Property(purchaseItem => purchaseItem.PriceInclTax).HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<PurchaseItem>().Property(purchaseItem => purchaseItem.PriceExclTax).HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<PurchaseItem>().Property(purchaseItem => purchaseItem.ItemWeight).HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<PurchaseItem>().Property(purchaseItem => purchaseItem.ReceivedTotalWeight).HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<PurchaseItem>().Property(purchaseItem => purchaseItem.ReceivedCratesWeight).HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<PurchaseItem>().Property(purchaseItem => purchaseItem.ReceivedActualWeight).HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<PurchaseItem>().Property(purchaseItem => purchaseItem.SortingWastageBad).HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<PurchaseItem>().Property(purchaseItem => purchaseItem.WeightKg).HasColumnType("decimal(18, 4)");
            modelBuilder.Entity<PurchaseItem>().HasOne(purchaseItem => purchaseItem.Purchase)
                .WithMany(purchase => purchase.PurchaseItems)
                .HasForeignKey(purchaseItem => purchaseItem.PurchaseId)
                .IsRequired();

            modelBuilder.Entity<PurchaseItem>().HasOne(purchaseItem => purchaseItem.Product)
                .WithMany()
                .HasForeignKey(purchaseItem => purchaseItem.ProductId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
