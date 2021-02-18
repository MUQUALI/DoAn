namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Post")]
    public partial class Post
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Post()
        {
            Comments = new HashSet<Comment>();
        }

        [Key]
        public int PK_PostID { get; set; }

        public int FK_AccountID { get; set; }

        [Required]
        [StringLength(100)]
        public string FK_ProductID { get; set; }

        [Required]
        [StringLength(1000)]
        public string Contents { get; set; }

        [Required]
        [StringLength(200)]
        public string Author { get; set; }

        public DateTime CreatedDate { get; set; }

        public int DelFlg { get; set; }

        public virtual Account Account { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual Product Product { get; set; }
    }
}
