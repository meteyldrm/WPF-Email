namespace EmailWPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("InternalDraft")]
    public partial class InternalDraft
    {
        [Key]
        public long intDraftID { get; set; }

        public int emailPiority { get; set; }

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
    }
}
