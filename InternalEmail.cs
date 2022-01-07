namespace EmailWPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InternalEmail")]
    public partial class InternalEmail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InternalEmail()
        {
            IntInbound = new HashSet<IntInbound>();
            IntOutbound = new HashSet<IntOutbound>();
            Thread = new HashSet<Thread>();
        }

        [Key]
        public long intEmailID { get; set; }

        public int emailPriority { get; set; }

        [Required]
        [StringLength(128)]
        public string emailSubject { get; set; }

        [Required]
        [StringLength(8000)]
        public string emailBody { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime emailTime { get; set; }

        public int categoryID { get; set; }

        public int senderID { get; set; }

        public long? attachmentID { get; set; }

        public virtual Attachments Attachments { get; set; }

        public virtual Categories Categories { get; set; }

        public virtual UserAddress UserAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IntInbound> IntInbound { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IntOutbound> IntOutbound { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Thread> Thread { get; set; }
    }
}
