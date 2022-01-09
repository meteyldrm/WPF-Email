namespace EmailWPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ExtInbound")]
    public partial class ExtInbound
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int userID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long extEmailID { get; set; }

        public bool isRead { get; set; }

        public bool isSpam { get; set; }

        public bool isDeleted { get; set; }

        public virtual ExternalEmail ExternalEmail { get; set; }

        public virtual UserAddress UserAddress { get; set; }
    }
}
