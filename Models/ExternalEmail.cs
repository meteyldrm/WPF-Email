namespace EmailWPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExternalEmail")]
    public partial class ExternalEmail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExternalEmail()
        {
            ExtInbound = new HashSet<ExtInbound>();
            ExtOutbound = new HashSet<ExtOutbound>();
        }

        [Key]
        public long extEmailID { get; set; }

        [Required]
        [StringLength(512)]
        public string recipients { get; set; }

        [Required]
        [StringLength(128)]
        public string emailSubject { get; set; }

        [Required]
        [StringLength(8000)]
        public string emailBody { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime emailTime { get; set; }

        [Required]
        [StringLength(50)]
        public string sender { get; set; }

        public long? attachmentID { get; set; }

        public virtual Attachments Attachments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExtInbound> ExtInbound { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExtOutbound> ExtOutbound { get; set; }
    }
}
