using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace EmailWPF {
	public partial class EmailModel : DbContext {
		public EmailModel()
			: base("name=EmailModel") {
		}

		public virtual DbSet<Attachments> Attachments { get; set; }
		public virtual DbSet<Categories> Categories { get; set; }
		public virtual DbSet<ExternalDraft> ExternalDraft { get; set; }
		public virtual DbSet<ExternalEmail> ExternalEmail { get; set; }
		public virtual DbSet<ExtInbound> ExtInbound { get; set; }
		public virtual DbSet<ExtOutbound> ExtOutbound { get; set; }
		public virtual DbSet<InternalDraft> InternalDraft { get; set; }
		public virtual DbSet<InternalEmail> InternalEmail { get; set; }
		public virtual DbSet<IntInbound> IntInbound { get; set; }
		public virtual DbSet<IntOutbound> IntOutbound { get; set; }
		public virtual DbSet<Recipients> Recipients { get; set; }
		public virtual DbSet<TblUser> TblUser { get; set; }
		public virtual DbSet<Team> Team { get; set; }
		public virtual DbSet<Thread> Thread { get; set; }
		public virtual DbSet<Undisclosed> Undisclosed { get; set; }
		public virtual DbSet<UserAddress> UserAddress { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			modelBuilder.Entity<Attachments>()
				.Property(e => e.attachmentName)
				.IsUnicode(false);

			modelBuilder.Entity<Attachments>()
				.Property(e => e.attachmentSize)
				.IsUnicode(false);

			modelBuilder.Entity<Attachments>()
				.Property(e => e.attachmentURI)
				.IsUnicode(false);

			modelBuilder.Entity<Categories>()
				.Property(e => e.categoryName)
				.IsUnicode(false);

			modelBuilder.Entity<Categories>()
				.HasMany(e => e.InternalDraft)
				.WithRequired(e => e.Categories)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Categories>()
				.HasMany(e => e.InternalEmail)
				.WithRequired(e => e.Categories)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ExternalDraft>()
				.Property(e => e.recipients)
				.IsUnicode(false);

			modelBuilder.Entity<ExternalDraft>()
				.Property(e => e.emailSubject)
				.IsUnicode(false);

			modelBuilder.Entity<ExternalDraft>()
				.Property(e => e.emailBody)
				.IsUnicode(false);

			modelBuilder.Entity<ExternalDraft>()
				.Property(e => e.sender)
				.IsUnicode(false);

			modelBuilder.Entity<ExternalEmail>()
				.Property(e => e.recipients)
				.IsUnicode(false);

			modelBuilder.Entity<ExternalEmail>()
				.Property(e => e.emailSubject)
				.IsUnicode(false);

			modelBuilder.Entity<ExternalEmail>()
				.Property(e => e.emailBody)
				.IsUnicode(false);

			modelBuilder.Entity<ExternalEmail>()
				.Property(e => e.sender)
				.IsUnicode(false);

			modelBuilder.Entity<ExternalEmail>()
				.HasMany(e => e.ExtInbound)
				.WithRequired(e => e.ExternalEmail)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<ExternalEmail>()
				.HasMany(e => e.ExtOutbound)
				.WithRequired(e => e.ExternalEmail)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<InternalDraft>()
				.Property(e => e.emailSubject)
				.IsUnicode(false);

			modelBuilder.Entity<InternalDraft>()
				.Property(e => e.emailBody)
				.IsUnicode(false);

			modelBuilder.Entity<InternalEmail>()
				.Property(e => e.emailSubject)
				.IsUnicode(false);

			modelBuilder.Entity<InternalEmail>()
				.Property(e => e.emailBody)
				.IsUnicode(false);

			modelBuilder.Entity<InternalEmail>()
				.HasMany(e => e.IntInbound)
				.WithRequired(e => e.InternalEmail)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<InternalEmail>()
				.HasMany(e => e.IntOutbound)
				.WithRequired(e => e.InternalEmail)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<InternalEmail>()
				.HasMany(e => e.Thread)
				.WithRequired(e => e.InternalEmail)
				.HasForeignKey(e => e.internalEmailID)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Recipients>()
				.HasOptional(e => e.Team)
				.WithRequired(e => e.Recipients);

			modelBuilder.Entity<Recipients>()
				.HasOptional(e => e.Undisclosed)
				.WithRequired(e => e.Recipients);

			modelBuilder.Entity<Recipients>()
				.HasMany(e => e.UserAddress)
				.WithMany(e => e.Recipients)
				.Map(m => m.ToTable("RecipientUser").MapLeftKey("recipientID").MapRightKey("userID"));

			modelBuilder.Entity<TblUser>()
				.Property(e => e.userName)
				.IsUnicode(false);

			modelBuilder.Entity<TblUser>()
				.HasMany(e => e.UserAddress)
				.WithRequired(e => e.TblUser)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Team>()
				.Property(e => e.teamName)
				.IsUnicode(false);

			modelBuilder.Entity<UserAddress>()
				.Property(e => e.emailAddress)
				.IsUnicode(false);

			modelBuilder.Entity<UserAddress>()
				.Property(e => e.alias)
				.IsUnicode(false);

			modelBuilder.Entity<UserAddress>()
				.Property(e => e.emailSignature)
				.IsUnicode(false);

			modelBuilder.Entity<UserAddress>()
				.Property(e => e.passwordHash)
				.IsFixedLength()
				.IsUnicode(false);

			modelBuilder.Entity<UserAddress>()
				.HasMany(e => e.ExtInbound)
				.WithRequired(e => e.UserAddress)
				.HasForeignKey(e => e.userID)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<UserAddress>()
				.HasMany(e => e.ExtOutbound)
				.WithRequired(e => e.UserAddress)
				.HasForeignKey(e => e.userID)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<UserAddress>()
				.HasMany(e => e.InternalDraft)
				.WithRequired(e => e.UserAddress)
				.HasForeignKey(e => e.senderID)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<UserAddress>()
				.HasMany(e => e.InternalEmail)
				.WithRequired(e => e.UserAddress)
				.HasForeignKey(e => e.senderID)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<UserAddress>()
				.HasMany(e => e.IntInbound)
				.WithRequired(e => e.UserAddress)
				.HasForeignKey(e => e.userID)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<UserAddress>()
				.HasMany(e => e.IntOutbound)
				.WithRequired(e => e.UserAddress)
				.HasForeignKey(e => e.userID)
				.WillCascadeOnDelete(false);
		}
	}
}
