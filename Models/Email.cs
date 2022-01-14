namespace EmailWPF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Email")]
    public partial class Email
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Email()
        {
            UserEmails = new HashSet<UserEmails>();
        }

        public long emailID { get; set; }

        public int priority { get; set; }

        [Required]
        [StringLength(128)]
        public string subject { get; set; }

        [Required]
        [StringLength(8000)]
        public string body { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime composeTime { get; set; }

        public bool isDraft { get; set; }

        public long? replyTo { get; set; }

        public int categoryID { get; set; }

        public int senderID { get; set; }

        public int threadID { get; set; }

        public virtual Category Category { get; set; }

        public virtual UserAddress UserAddress { get; set; }

        public virtual Thread Thread { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserEmails> UserEmails { get; set; }
    }
}
