namespace Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SecondHandDbContext : DbContext
    {
        public SecondHandDbContext()
            : base("name=SecondHandDbContext")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<BillDetail> BillDetails { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Custom> Customs { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Firm> Firms { get; set; }
        public virtual DbSet<GiveBack> GiveBacks { get; set; }
        public virtual DbSet<GiveBackDetail> GiveBackDetails { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<Rule> Rules { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartDetail> CartDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Bills)
                .WithRequired(e => e.Account)
                .HasForeignKey(e => e.FK_AccountID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Account)
                .HasForeignKey(e => e.FK_AccountID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.GiveBacks)
                .WithRequired(e => e.Account)
                .HasForeignKey(e => e.FK_AccountID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Account)
                .HasForeignKey(e => e.FK_AccountID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Posts)
                .WithRequired(e => e.Account)
                .HasForeignKey(e => e.FK_AccountID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bill>()
                .Property(e => e.TotalMoney)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Bill>()
                .Property(e => e.Discount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Bill>()
                .HasMany(e => e.BillDetails)
                .WithRequired(e => e.Bill)
                .HasForeignKey(e => e.Bill_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BillDetail>()
                .Property(e => e.ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<BillDetail>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<BillDetail>()
                .Property(e => e.Money)
                .HasPrecision(18, 0);

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

            modelBuilder.Entity<Custom>()
                .HasMany(e => e.BillDetails)
                .WithRequired(e => e.Custom)
                .HasForeignKey(e => e.FK_CustomID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Custom>()
                .HasMany(e => e.GiveBackDetails)
                .WithRequired(e => e.Custom)
                .HasForeignKey(e => e.FK_CustomID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Custom>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Custom)
                .HasForeignKey(e => e.FK_CustomID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Accounts)
                .WithOptional(e => e.Customer)
                .HasForeignKey(e => e.FK_CustomerID);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Bills)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.FK_CustomerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.FK_CustomerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Accounts)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.FK_EmpID);

            modelBuilder.Entity<Firm>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.Firm)
                .HasForeignKey(e => e.FK_FirmID);

            modelBuilder.Entity<GiveBack>()
                .Property(e => e.TotalMoney)
                .HasPrecision(18, 0);

            modelBuilder.Entity<GiveBack>()
                .HasMany(e => e.GiveBackDetails)
                .WithRequired(e => e.GiveBack)
                .HasForeignKey(e => e.GiveBackID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GiveBackDetail>()
                .Property(e => e.ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<GiveBackDetail>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<GiveBackDetail>()
                .Property(e => e.Money)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Money)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.TotalMoney)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Order)
                .HasForeignKey(e => e.OrderID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.FK_ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Post)
                .HasForeignKey(e => e.FK_PostID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.PK_ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Images)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.BuyPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.SalePrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.BillDetails)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.ProductID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.GiveBackDetails)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.ProductID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.ProductID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Posts)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.FK_ProductID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductType>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.ProductType)
                .HasForeignKey(e => e.FK_ProductTypeID);

            modelBuilder.Entity<Rule>()
                .HasMany(e => e.Accounts)
                .WithRequired(e => e.Rule)
                .HasForeignKey(e => e.FK_RuleID)
                .WillCascadeOnDelete(false);
        }
    }
}
