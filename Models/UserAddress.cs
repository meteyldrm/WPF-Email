namespace EmailWPF.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserAddress")]
    public partial class UserAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserAddress()
        {
            Email = new HashSet<Email>();
            UserEmails = new HashSet<UserEmails>();
        }

        [Key]
        public int addressID { get; set; }

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
        public virtual ICollection<Email> Email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserEmails> UserEmails { get; set; }
    }
}
