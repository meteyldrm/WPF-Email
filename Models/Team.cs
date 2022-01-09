namespace EmailWPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Team")]
    public partial class Team
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long teamID { get; set; }

        [Required]
        [StringLength(32)]
        public string teamName { get; set; }

        public virtual Recipients Recipients { get; set; }
    }
}
