namespace EmailWPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Attachments
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Attachments()
        {
            ExternalDraft = new HashSet<ExternalDraft>();
            ExternalEmail = new HashSet<ExternalEmail>();
            InternalDraft = new HashSet<InternalDraft>();
            InternalEmail = new HashSet<InternalEmail>();
        }

        [Key]
        public long attachmentID { get; set; }

        [Required]
        [StringLength(32)]
        public string attachmentName { get; set; }

        [Required]
        [StringLength(6)]
        public string attachmentSize { get; set; }

        [Required]
        [StringLength(128)]
        public string attachmentURI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalDraft> ExternalDraft { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalEmail> ExternalEmail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InternalDraft> InternalDraft { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InternalEmail> InternalEmail { get; set; }
    }
}
