create database EmailDB

use EmailDB

--USER

create table TblUser(
userID int primary key identity(1,1),
userName varchar(50) not null
)

create table UserAddress(
aID int primary key identity(1,1),
emailAddress varchar(50) not null,
alias varchar(16) null,
emailSignature varchar(64) null,
passwordHash char(64) not null,
userID int not null
)

create table Recipients(
recipientID bigint primary key identity(1,1),
threadID int not null
)

create table Thread(
threadID bigint primary key identity(1,1),
internalEmailID bigint not null
)

create table RecipientUser(
userID int not null,
recipientID bigint not null
)

create table Team(
teamID bigint primary key,
teamName varchar(32) not null
)

create table Undisclosed(
undisclosedID bigint primary key
)

create table Attachments(
attachmentID bigint primary key identity(1,1),
attachmentName varchar(32) not null,
attachmentSize varchar(6) not null,
attachmentURI varchar(128) not null
)

create table Categories(
categoryID int primary key identity(1,1),
categoryName varchar(16) not null
)

--EXTERNAL EMAIL

create table ExternalEmail(
extEmailID bigint primary key identity(1,1),
recipients varchar(512) not null,
emailSubject varchar(128) not null,
emailBody varchar(8000) not null, --Max size varchar (UTF-8)
emailTime smalldatetime not null,
sender varchar(50) not null,
attachmentID bigint null
)

create table ExternalDraft(
extDraftID bigint primary key identity(1,1),
recipients varchar(512) not null,
emailSubject varchar(128) not null,
emailBody varchar(8000) not null, --Max size varchar (UTF-8)
emailTime smalldatetime not null,
sender varchar(50) not null,
attachmentID bigint null
)

--INBOX

create table ExtInbound(
userID int not null,
extEmailID bigint not null,
isRead bit not null,
isSpam bit not null,
isDeleted bit not null
)

create table ExtOutbound(
userID int not null,
extEmailID bigint not null,
deliveryTime smalldatetime null
)

create table IntInbound(
userID int not null,
intEmailID bigint not null,
isRead bit not null,
isSpam bit not null,
isDeleted bit not null
)

create table IntOutbound(
userID int not null,
intEmailID bigint not null,
deliveryTime smalldatetime null
)

create table InternalEmail(
intEmailID bigint primary key identity(1,1),
emailPriority int not null,
emailSubject varchar(128) not null,
emailBody varchar(8000) not null, --Max size varchar (UTF-8)
emailTime smalldatetime not null,
categoryID int not null,
senderID int not null,
attachmentID bigint null
)

create table InternalDraft(
intDraftID bigint primary key identity(1,1),
emailPiority int not null,
emailSubject varchar(128) not null,
emailBody varchar(8000) not null, --Max size varchar (UTF-8)
emailTime smalldatetime not null,
categoryID int not null,
senderID int not null,
attachmentID bigint null
)

alter table UserAddress add constraint TblUserFKuID foreign key (userID) references TblUser(userID)

alter table Thread add constraint ThreadFKinternalEmailID foreign key (internalEmailID) references InternalEmail(intEmailID)

alter table RecipientUser add constraint RecipientUserPK primary key (userID, recipientID)
alter table RecipientUser add constraint RecipientUserFKuID foreign key (userID) references UserAddress(aID)
alter table RecipientUser add constraint RecipientUserFKrID foreign key (recipientID) references Recipients(recipientID)

alter table Team add constraint TeamFKrID foreign key (teamID) references Recipients(recipientID)
alter table Undisclosed add constraint UndisclosedFKrID foreign key (undisclosedID) references Recipients(recipientID)

alter table ExternalEmail add constraint ExternalEmailFKattachmentID foreign key (attachmentID) references Attachments(attachmentID)
alter table ExternalDraft add constraint ExternalDraftFKattachmentID foreign key (attachmentID) references Attachments(attachmentID)

alter table ExtInbound add constraint ExtInboundPK primary key (userID, extEmailID)
alter table ExtInbound add constraint ExtInboundFKuserID foreign key (userID) references UserAddress(aID)
alter table ExtInbound add constraint ExtInboundFKextEmailID foreign key (extEmailID) references ExternalEmail(extEmailID)

alter table ExtOutbound add constraint ExtOutboundPK primary key (userID, extEmailID)
alter table ExtOutbound add constraint ExtOutboundFKuserID foreign key (userID) references UserAddress(aID)
alter table ExtOutbound add constraint ExtOutboundFKextEmailID foreign key (extEmailID) references ExternalEmail(extEmailID)

alter table IntInbound add constraint IntInboundPK primary key (userID, intEmailID)
alter table IntInbound add constraint IntInboundFKuserID foreign key (userID) references UserAddress(aID)
alter table IntInbound add constraint IntInboundFKextEmailID foreign key (intEmailID) references InternalEmail(intEmailID)

alter table IntOutbound add constraint IntOutboundPK primary key (userID, intEmailID)
alter table IntOutbound add constraint IntOutboundFKuserID foreign key (userID) references UserAddress(aID)
alter table IntOutbound add constraint IntOutboundFKextEmailID foreign key (intEmailID) references InternalEmail(intEmailID)

alter table InternalEmail add constraint InternalEmailFKcategoryID foreign key (categoryID) references Categories(categoryID)
alter table InternalEmail add constraint InternalEmailFKsenderID foreign key (senderID) references UserAddress(aID)
alter table InternalEmail add constraint InternalEmailFKattachmentID foreign key (attachmentID) references Attachments(attachmentID)

alter table InternalDraft add constraint InternalDraftFKcategoryID foreign key (categoryID) references Categories(categoryID)
alter table InternalDraft add constraint InternalDraftFKsenderID foreign key (senderID) references UserAddress(aID)
alter table InternalDraft add constraint InternalDraftFKattachmentID foreign key (attachmentID) references Attachments(attachmentID)

--CREATE DATA

insert into TblUser (userName) values ('John Doe'), ('Jane Doe')

insert into Categories (categoryName) values ('General'), ('Inquiry'), ('Update'), ('Event'), ('News')
insert into Attachments (attachmentName, attachmentSize, attachmentURI) values ('TestAttachment.zip', '238 KB', 'https://cloud.data.storage/TestAttachment_zip1')
