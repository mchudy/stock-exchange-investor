create table [dbo].[Company]
(
	[Id]					int identity(1, 1)	NOT NULL primary key,
	[Code]					char(3)				NOT NULL unique,
	[Name]					nvarchar(50)		NOT NULL
);

create table [dbo].[Price]
(
	[Id]					int identity(1, 1)	NOT NULL primary key,
	[CompanyId]				int					NOT NULL foreign key references [dbo].[Company]([Id]),
	[Date]					date				NOT NULL,
	[OpenPrice]				decimal(18, 2)		NOT NULL,
	[ClosePrice]			decimal(18, 2)		NOT NULL,
	[HighPrice]				decimal(18, 2)		NOT NULL,
	[LowPrice]				decimal(18, 2)		NOT NULL,
	[Volume]				int					NOT NULL
);