declare @name varchar(255);

SELECT @name = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = 'InvestmentStrategy'
	AND COLUMN_NAME = 'InvestmentStrategyId';

EXEC('ALTER TABLE [dbo].[InvestmentStrategies] DROP CONSTRAINT ' + @name);

DROP TABLE [dbo].[InvestementCondition]
