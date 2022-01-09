namespace EmailWPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("IntInbound")]
    public partial class IntInbound
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int userID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long intEmailID { get; set; }

        public bool isRead { get; set; }

        public bool isSpam { get; set; }

        public bool isDeleted { get; set; }

        public virtual InternalEmail InternalEmail { get; set; }

        public virtual UserAddress UserAddress { get; set; }
    }
}
