namespace EmailWPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExtOutbound")]
    public partial class ExtOutbound
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int userID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long extEmailID { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? deliveryTime { get; set; }

        public virtual ExternalEmail ExternalEmail { get; set; }

        public virtual UserAddress UserAddress { get; set; }
    }
}
