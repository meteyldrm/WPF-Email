namespace EmailWPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Recipients
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Recipients()
        {
            UserAddress = new HashSet<UserAddress>();
        }

        [Key]
        public long recipientID { get; set; }

        public int threadID { get; set; }

        public virtual Team Team { get; set; }

        public virtual Undisclosed Undisclosed { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserAddress> UserAddress { get; set; }
    }
}
