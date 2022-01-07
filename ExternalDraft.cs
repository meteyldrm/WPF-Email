namespace EmailWPF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ExternalDraft")]
    public partial class ExternalDraft
    {
        [Key]
        public long extDraftID { get; set; }

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
    }
}
