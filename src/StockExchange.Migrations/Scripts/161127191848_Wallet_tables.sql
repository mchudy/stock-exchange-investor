CREATE TABLE [dbo].[Wallet]
(
	[Id] int identity(1,1) NOT NULL primary key,
	[Budget] decimal(18,2) NOT NULL,
	[UserId] int NOT NULL foreign key references [dbo].[AspNetUsers]([Id])
);

CREATE TABLE [dbo].[UserTransaction]
(
	[Id] int identity(1,1) NOT NULL primary key,
	[Price] decimal(18,2) NOT NULL,
	[Quantity] int,
	[Date] date, 
	[CompanyId] int NOT NULL foreign key references [dbo].[Company]([Id]),
	[WalletId] int NOT NULL foreign key references [dbo].[Wallet]([Id])
);