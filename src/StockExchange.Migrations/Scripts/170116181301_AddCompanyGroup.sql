CREATE TABLE [dbo].[CompanyGroup]
(
	[Id] int identity(1,1) NOT NULL PRIMARY KEY,
	[Name] varchar(25) NOT NULL
);

CREATE TABLE [dbo].[CompanyGroupCompany]
(
	[CompanyId] int NOT NULL CONSTRAINT FK_CompanyGroupCompany_Company FOREIGN KEY REFERENCES [dbo].[Company]([Id]),
	[CompanyGroupId] int NOT NULL CONSTRAINT FK_CompanyGroupCompany_CompanyGroup FOREIGN KEY REFERENCES [dbo].[CompanyGroup]([Id]),
	CONSTRAINT PK_CompanyGroupCompany PRIMARY KEY NONCLUSTERED ([CompanyId], [CompanyGroupId])
);