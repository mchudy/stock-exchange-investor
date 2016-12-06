declare @name varchar(255);

SELECT @name = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = 'Condition'
	AND COLUMN_NAME = 'StrategyId';

EXEC('ALTER TABLE [dbo].[Condition] DROP CONSTRAINT ' + @name);

DROP TABLE [dbo].[Condition]
