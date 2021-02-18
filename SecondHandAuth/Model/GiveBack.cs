namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GiveBack")]
    public partial class GiveBack
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GiveBack()
        {
            GiveBackDetails = new HashSet<GiveBackDetail>();
        }

        [Key]
        public int PK_GiveBackID { get; set; }

        public int FK_AccountID { get; set; }

        [Required]
        [StringLength(100)]
        public string Customer { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal TotalMoney { get; set; }

        [Required]
        [StringLength(500)]
        public string Note { get; set; }

        public int DelFlg { get; set; }

        public virtual Account Account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GiveBackDetail> GiveBackDetails { get; set; }
    }
}
