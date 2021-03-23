namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BillDetail")]
    public partial class BillDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Bill_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string ProductID { get; set; }

        [Required]
        [StringLength(500)]
        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Money { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FK_CustomID { get; set; }

        public virtual Bill Bill { get; set; }

        public virtual Custom Custom { get; set; }

        public virtual Product Product { get; set; }
    }
}
