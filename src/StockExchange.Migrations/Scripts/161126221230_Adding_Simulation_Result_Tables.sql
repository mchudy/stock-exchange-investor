CREATE TABLE [dbo].[SimulationResult]
(
	[Id] int identity(1,1) NOT NULL primary key,
	[SimulationId] int NOT NULL foreign key references [dbo].[Simulation]([Id])
);

CREATE TABLE [dbo].[SimulationValue]
(
	[Id] int identity(1,1) NOT NULL primary key,
	[Date] date NOT NULL,
	[Value] decimal(18,2) NOT NULL,
	[SimulationResultId] int NOT NULL foreign key references [dbo].[SimulationResult]([Id])
);

CREATE TABLE [dbo].[Transaction]
(
	[Id] int identity(1,1) NOT NULL primary key,
	[Date] date NOT NULL,
	[Price] decimal(18,2) NOT NULL,
	[Quantity] int NOT NULL,
	[CompanyId] int NOT NULL foreign key references [dbo].[Company]([Id]),
	[SimulationResultId] int NOT NULL foreign key references [dbo].[SimulationResult]([Id]),
);

CREATE TABLE [dbo].[CompanyStockQuantity]
(
	[Id] int identity(1,1) NOT NULL primary key,
	[Date] date NOT NULL,
	[Price] decimal(18,2) NOT NULL,
	[Quantity] int NOT NULL,
	[CompanyId] int NOT NULL foreign key references [dbo].[Company]([Id]),
	[SimulationResultId] int NOT NULL foreign key references [dbo].[SimulationResult]([Id]),
);