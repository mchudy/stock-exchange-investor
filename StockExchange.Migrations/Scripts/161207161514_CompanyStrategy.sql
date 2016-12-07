ALTER TABLE [dbo].[Company]
ADD [StrategyId] int NOT NULL

ALTER TABLE [dbo].[Strategy]
ADD [CompanyId] int NOT NULL

CREATE TABLE [dbo].[StrategiesCompanies]
(
	[StrategyId] int NOT NULL FOREIGN KEY REFERENCES [dbo].[Strategy]([Id]),
	[CompanyId] int NOT NULL FOREIGN KEY REFERENCES [dbo].[Company]([Id])
);