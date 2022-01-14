namespace EmailWPF.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TblUser")]
    public partial class TblUser
    {
        [Key]
        public int userID { get; set; }

        [Required]
        [StringLength(50)]
        public string userName { get; set; }
    }
}
