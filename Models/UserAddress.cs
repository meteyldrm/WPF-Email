namespace EmailWPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserAddress")]
    public partial class UserAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserAddress()
        {
            ExtInbound = new HashSet<ExtInbound>();
            ExtOutbound = new HashSet<ExtOutbound>();
            InternalDraft = new HashSet<InternalDraft>();
            InternalEmail = new HashSet<InternalEmail>();
            IntInbound = new HashSet<IntInbound>();
            IntOutbound = new HashSet<IntOutbound>();
            Recipients = new HashSet<Recipients>();
        }

        [Key]
        public int aID { get; set; }

        [Required]
        [StringLength(50)]
        public string emailAddress { get; set; }

        [StringLength(16)]
        public string alias { get; set; }

        [StringLength(64)]
        public string emailSignature { get; set; }

        [Required]
        [StringLength(64)]
        public string passwordHash { get; set; }

        public int userID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExtInbound> ExtInbound { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExtOutbound> ExtOutbound { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InternalDraft> InternalDraft { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InternalEmail> InternalEmail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IntInbound> IntInbound { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IntOutbound> IntOutbound { get; set; }

        public virtual TblUser TblUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recipients> Recipients { get; set; }
    }
}
