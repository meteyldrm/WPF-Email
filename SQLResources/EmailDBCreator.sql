--301 v2

create database EmailDB_301

use EmailDB_301

create table TblUser(
    userID int primary key identity(1,1),
    userName varchar(50) not null
)

create table UserAddress(
    addressID int primary key identity(1,1),
    emailAddress varchar(50) not null,
    alias varchar(16) null,
    emailSignature varchar(64) null,
    passwordHash char(64) not null,
    userID int not null
)

create table Thread(
    threadID int primary key identity (1,1),
    threadName varchar(50) not null,
    threadStartTime smalldatetime not null
)

create table Category(
    categoryID int primary key identity(1,1),
    categoryName varchar(16) not null
)

create table Email(
    emailID bigint primary key identity (1,1),
    priority int not null,
    subject varchar(128) not null,
    body varchar(8000) not null, --Max size varchar (UTF-8)
    composeTime smalldatetime not null,
    isDraft bit not null,
    replyTo bigint null,
    categoryID int not null,
    senderID int not null,
	threadID int not null,
    constraint FK_Email_Sender foreign key (senderID) references UserAddress (addressID),
    constraint FK_Email_Category foreign key (categoryID) references Category (categoryID),
    constraint FK_Email_Thread foreign key (threadID) references Thread (threadID)
)

create table UserEmails(
    addressID int,
    emailID bigint,
    isRead bit not null,
    isArchived bit not null,
    isDeleted bit not null,
    constraint PK_EmailRecipients primary key (addressID, emailID),
    constraint FK_EmailRecipients_Email foreign key (emailID) references Email (emailID),
    constraint FK_EmailRecipients_UserAddress foreign key (addressID) references UserAddress (addressID)
)

insert into Category (categoryName) values ('General'), ('Inquiry'), ('Update'), ('Event'), ('News')
insert into TblUser (userName) values ('John Doe'), ('Jane Doe')

insert into Thread (threadName, threadStartTime) values ('Email Portal Start', '2021-12-13 12:43:10')

insert into UserAddress (emailAddress, alias, passwordHash, userID) values ('johndoe@emailprovider.com', 'Personal', '9f735e0df9a1ddc702bf0a1a7b83033f9f7153a00c29de82cedadc9957289b05', 1)
insert into UserAddress (emailAddress, alias, passwordHash, userID) values ('janedoe@organization.com', 'Work', '0d7a73a5e72468e4dafa3790ede477cc507d838681c5af6d1836e6f0b5f6a1fd', 2)
insert into UserAddress (emailAddress, alias, passwordHash, userID) values ('johndoe@organization.com', 'Work', '0d7a73a5e72468e4dafa3790ede477cc507d838681c5af6d1836e6f0b5f6a1fd', 1)

