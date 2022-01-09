namespace EmailWPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Undisclosed")]
    public partial class Undisclosed
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long undisclosedID { get; set; }

        public virtual Recipients Recipients { get; set; }
    }
}
