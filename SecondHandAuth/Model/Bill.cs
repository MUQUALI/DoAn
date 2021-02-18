namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bill")]
    public partial class Bill
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bill()
        {
            BillDetails = new HashSet<BillDetail>();
        }

        [Key]
        public int PK_Bill_ID { get; set; }

        public int FK_AccountID { get; set; }

        public int FK_CustomerID { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal TotalMoney { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public decimal? Discount { get; set; }

        public int Status { get; set; }

        public int DelFlg { get; set; }

        public virtual Account Account { get; set; }

        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}
