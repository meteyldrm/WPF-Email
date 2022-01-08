namespace EmailWPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Thread")]
    public partial class Thread
    {
        public long threadID { get; set; }

        public long internalEmailID { get; set; }

        public virtual InternalEmail InternalEmail { get; set; }
    }
}
