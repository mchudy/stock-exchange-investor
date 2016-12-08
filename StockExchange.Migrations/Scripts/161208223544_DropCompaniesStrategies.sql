ALTER TABLE [dbo].[Company]
DROP COLUMN [StrategyId];

ALTER TABLE [dbo].[Strategy]
DROP COLUMN [CompanyId];

ALTER TABLE [dbo].[StrategiesCompanies]
DROP CONSTRAINT [FK_Strategy_CompanyId];

ALTER TABLE [dbo].[StrategiesCompanies]
DROP CONSTRAINT [FK_Company_StrategyId];

DROP TABLE [dbo].[StrategiesCompanies];