create table dbo.Company
(
	id						int identity(1, 1)	NOT NULL primary key,
	code					char(3)				NOT NULL unique,
	name					varchar(50)			NOT NULL
);

create table dbo.Price
(
	id						int identity(1, 1)	NOT NULL primary key,
	companyId				int					NOT NULL foreign key references dbo.Company(id),
	date					date				NOT NULL,
	openPrice				decimal(18, 2)		NOT NULL,
	closePrice				decimal(18, 2)		NOT NULL,
	highPrice				decimal(18, 2)		NOT NULL,
	lowPrice				decimal(18, 2)		NOT NULL,
	volume					int					NOT NULL
);

insert into dbo.Company values
	('kgh', 'KGHM Polska Miedź SA (KGH)'),
	('acp', 'Asseco Poland SA (ACP)'),
	('alr', 'Alior Bank SA (ALR)'),
	('bzw', 'Bank Zachodni WBK SA (BZW)'),
	('ccc', 'CCC SA (CCC)'),
	('cps', 'Cyfrowy Polsat SA (CPS)'),
	('ena', 'Enea SA (ENA)'),
	('eng', 'Energa SA (ENG)'),
	('eur', 'Eurocash SA (EUR)'),
	('lpp', 'LPP SA (LPP)'),
	('lts', 'Grupa Lotos SA (LTS)'),
	('mbk', 'mBank SA (MBK)'),
	('opl', 'Orange Polska SA (OPL)'),
	('peo', 'Bank Polska Kasa Opieki SA (PEO)'),
	('pge', 'PGE Polska Grupa Energetyczna SA (PGE)'),
	('pgn', 'Polskie Górnictwo Naftowe i Gazownictwo SA (PGN)'),
	('pkn', 'Polski Koncern Naftowy ORLEN SA (PKN)'),
	('pko', 'Powszechna Kasa Oszczędności Bank Polski SA (PKO)'),
	('pzu', 'Powszechny Zakład Ubezpieczeń SA (PZU)'),
	('tpe', 'Tauron Polska Energia SA (TPE)');

select [c].[id] 
	  ,[c].[code]
	  ,[c].[name]
	  ,[p].[id]
	  ,[p].[date]
	  ,[p].[openPrice]
	  ,[p].[closePrice]
	  ,[p].[highPrice]
	  ,[p].[lowPrice]
	  ,[p].[volume]
  from [dbo].[Price] as [p]
  join [dbo].[Company] as [c] on [p].[companyId] = [c].[id]

select [c].[id]
	  ,[c].[code]
	  ,[c].[name]
	  ,count(*) as 'Days On Stock'
  from [dbo].[Price] as [p]
  join [dbo].[Company] as [c] on [c].[id] = [p].[companyId]
  group by [c].[id], [c].[name], [c].[code]