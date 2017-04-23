declare @name varchar(255);

SELECT @name = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = 'SimulationValue'
	AND COLUMN_NAME = 'SimulationResultId';

EXEC('ALTER TABLE [dbo].[SimulationValue] DROP CONSTRAINT ' + @name);

DROP TABLE [dbo].[SimulationValue];

SELECT @name = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = 'Transaction'
	AND COLUMN_NAME = 'CompanyId';

EXEC('ALTER TABLE [dbo].[Transaction] DROP CONSTRAINT ' + @name);

SELECT @name = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = 'Transaction'
	AND COLUMN_NAME = 'SimulationResultId';

EXEC('ALTER TABLE [dbo].[Transaction] DROP CONSTRAINT ' + @name);

DROP TABLE [dbo].[Transaction];



SELECT @name = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = 'CompanyStockQuantity'
	AND COLUMN_NAME = 'SimulationResultId';

EXEC('ALTER TABLE [dbo].[CompanyStockQuantity] DROP CONSTRAINT ' + @name);

SELECT @name = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = 'CompanyStockQuantity'
	AND COLUMN_NAME = 'CompanyId';

EXEC('ALTER TABLE [dbo].[CompanyStockQuantity] DROP CONSTRAINT ' + @name);

DROP TABLE [dbo].[CompanyStockQuantity];



SELECT @name = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = 'SimulationResult'
	AND COLUMN_NAME = 'SimulationId';

EXEC('ALTER TABLE [dbo].[SimulationResult] DROP CONSTRAINT ' + @name);

DROP TABLE [dbo].[SimulationResult];


SELECT @name = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = 'SimulationCompany'
	AND COLUMN_NAME = 'CompanyId';

EXEC('ALTER TABLE [dbo].[SimulationCompany] DROP CONSTRAINT ' + @name);

SELECT @name = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = 'SimulationCompany'
	AND COLUMN_NAME = 'SimulationId';

EXEC('ALTER TABLE [dbo].[SimulationCompany] DROP CONSTRAINT ' + @name);

DROP TABLE [dbo].[SimulationCompany];


SELECT @name = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = 'UserTransaction'
	AND COLUMN_NAME = 'WalletId';

EXEC('ALTER TABLE [dbo].[UserTransaction] DROP CONSTRAINT ' + @name);

ALTER TABLE [dbo].[UserTransaction]
DROP COLUMN [WalletId];

SELECT @name = CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = 'Wallet'
	AND COLUMN_NAME = 'UserId';

EXEC('ALTER TABLE [dbo].[Wallet] DROP CONSTRAINT ' + @name);

DROP TABLE [dbo].[Wallet];

ALTER TABLE [dbo].[AspNetUsers]
ADD [Budget] decimal(18, 2) NOT NULL DEFAULT(0);