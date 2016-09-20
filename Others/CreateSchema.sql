create table dbo.Company
(
	id						int identity(1, 1)	NOT NULL primary key,
	code					char(3)				NOT NULL,
	name					varchar(50)			NOT NULL
);

create table dbo.Price
(
	id						int identity(1, 1)	NOT NULL primary key,
	companyId				int					NOT NULL foreign key references dbo.Company(id),
	date					date				NOT NULL,
	openPrice				decimal				NOT NULL,
	closePrice				decimal				NOT NULL,
	highPrice				decimal				NOT NULL,
	lowPrice				decimal				NOT NULL,
	volume					int					NOT NULL
);

insert into dbo.Company values ('kgh', 'KGHM Polska Miedź SA (KGH)');