CREATE TABLE [dbo].[Simulation]
(
	[Id] int identity(1,1) NOT NULL primary key,
	[Budget] decimal(18,2) NOT NULL,
	[StartDate] date NOT NULL,
	[EndDate] date NOT NULL,
	[StrategyId] int NOT NULL foreign key references [dbo].[Strategy]([Id])
);

CREATE TABLE [dbo].[SimulationCompany]
(
	[Id] int identity(1,1) NOT NULL primary key,
	[CompanyId] int NOT NULL foreign key references [dbo].[Company]([Id]),
	[SimulationId] int NOT NULL foreign key references [dbo].[Simulation]([Id])
);