DELETE FROM [dbo].[StrategyIndicatorProperty] 
WHERE IndicatorId IN (SELECT Id FROM [dbo].[StrategyIndicator]
WHERE IndicatorType = 10 OR IndicatorType = 11)

UPDATE [dbo].[Strategy]
SET IsDeleted = 1
WHERE Id IN (SELECT StrategyId FROM [dbo].[StrategyIndicator]
WHERE IndicatorType = 10 OR IndicatorType = 11)

DELETE FROM [dbo].[StrategyIndicator]
WHERE IndicatorType = 10 OR IndicatorType = 11