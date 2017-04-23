CREATE TABLE [dbo].[StrategyIndicator]
(
	[Id] int identity(1,1) NOT NULL PRIMARY KEY,
	[IndicatorType] int NOT NULL,

	[StrategyId] int NOT NULL CONSTRAINT FK_Indicator_StrategyId FOREIGN KEY REFERENCES [dbo].[Strategy]([Id])
);

CREATE TABLE [dbo].[StrategyIndicatorProperty]
(
	[Id] int identity(1,1) NOT NULL PRIMARY KEY,
	[Name] varchar(25) NOT NULL,
	[Value] int NOT NULL,

	[IndicatorId] int NOT NULL CONSTRAINT FK_Property_IndicatorId FOREIGN KEY REFERENCES [dbo].[StrategyIndicator]([Id])
);

ALTER TABLE [dbo].[Strategy]
ADD [Name] nvarchar(50) NOT NULL