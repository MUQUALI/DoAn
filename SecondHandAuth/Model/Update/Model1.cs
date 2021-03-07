namespace Model.Update
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartDetail> CartDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>()
                .Property(e => e.TotalMoney)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Cart>()
                .HasMany(e => e.CartDetails)
                .WithRequired(e => e.Cart)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CartDetail>()
                .Property(e => e.ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<CartDetail>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CartDetail>()
                .Property(e => e.Money)
                .HasPrecision(18, 0);
        }
    }
}
