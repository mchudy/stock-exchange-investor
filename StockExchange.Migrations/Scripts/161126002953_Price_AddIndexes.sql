CREATE NONCLUSTERED INDEX [IX_Price_CompanyId]
    ON [dbo].[Price] ([CompanyId]);

CREATE NONCLUSTERED INDEX [IX_Price_Date]
    ON [dbo].[Price] ([Date]);