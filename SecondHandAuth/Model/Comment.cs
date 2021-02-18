namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        [Key]
        public int PK_CommentID { get; set; }

        public int FK_AccountID { get; set; }

        public int FK_CustomerID { get; set; }

        public int FK_PostID { get; set; }

        [Required]
        [StringLength(200)]
        public string Author { get; set; }

        [Required]
        [StringLength(500)]
        public string Contents { get; set; }

        public DateTime CreatedDate { get; set; }

        public int DelFlg { get; set; }

        public virtual Account Account { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Post Post { get; set; }
    }
}
