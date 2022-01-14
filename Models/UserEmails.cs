namespace EmailWPF.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class UserEmails
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int addressID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long emailID { get; set; }

        public bool isRead { get; set; }

        public bool isArchived { get; set; }

        public bool isDeleted { get; set; }

        public virtual Email Email { get; set; }

        public virtual UserAddress UserAddress { get; set; }
    }
}
