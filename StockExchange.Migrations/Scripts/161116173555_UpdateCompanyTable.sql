delete from dbo.Price;

delete from dbo.Company;

alter table dbo.Company 
alter column Code nvarchar(20) not null;

alter table dbo.Company 
alter column Name nvarchar(50) null;