ALTER TABLE [dbo].[Company]
ADD [StrategyId] int

ALTER TABLE [dbo].[Strategy]
ADD [CompanyId] int

CREATE TABLE [dbo].[StrategiesCompanies]
(
	[StrategyId] int NOT NULL CONSTRAINT FK_Strategy_CompanyId FOREIGN KEY REFERENCES [dbo].[Strategy]([Id]),
	[CompanyId] int NOT NULL CONSTRAINT FK_Company_StrategyId FOREIGN KEY REFERENCES [dbo].[Company]([Id])
);