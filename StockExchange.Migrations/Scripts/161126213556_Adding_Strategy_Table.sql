CREATE TABLE [dbo].[Strategy]
(
	[Id] int identity(1,1) NOT NULL primary key,
	[UserId] int NOT NULL foreign key references [dbo].[AspNetUsers]([Id]),
);

CREATE TABLE [dbo].[Condition]
(
	[Id] int identity(1,1) NOT NULL primary key,
	[IndicatorName] nvarchar(30) NOT NULL,
	[BuyValue] decimal(18,2) NULL,
	[SellValue] decimal(18,2) NULL,
	[StrategyId] int NOT NULL foreign key references [dbo].[Strategy]([Id])
);