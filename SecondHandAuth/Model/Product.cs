namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            BillDetails = new HashSet<BillDetail>();
            GiveBackDetails = new HashSet<GiveBackDetail>();
            OrderDetails = new HashSet<OrderDetail>();
            Posts = new HashSet<Post>();
        }

        [Key]
        [StringLength(100)]
        public string PK_ProductID { get; set; }

        public int? FK_ProductTypeID { get; set; }

        public int? FK_FirmID { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        [Required]
        [StringLength(8000)]
        public string Images { get; set; }

        public int Quantity { get; set; }

        public decimal BuyPrice { get; set; }

        public decimal SalePrice { get; set; }

        [StringLength(200)]
        public string Status { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int? Vote { get; set; }

        public int DelFlg { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillDetail> BillDetails { get; set; }

        public virtual Firm Firm { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiveBackDetail> GiveBackDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Post> Posts { get; set; }

        public virtual ProductType ProductType { get; set; }
    }
}
