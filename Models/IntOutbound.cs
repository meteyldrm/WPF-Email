namespace EmailWPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IntOutbound")]
    public partial class IntOutbound
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int userID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long intEmailID { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? deliveryTime { get; set; }

        public virtual InternalEmail InternalEmail { get; set; }

        public virtual UserAddress UserAddress { get; set; }
    }
}
