delete from dbo.Price;

delete from dbo.Company;

declare @unique_constraint_name nvarchar(100);

select top 1 @unique_constraint_name = c.constraint_name
from   INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE col
join   INFORMATION_SCHEMA.TABLE_CONSTRAINTS c on c.constraint_name = col.constraint_name
where  c.table_name = 'Company'
and    c.constraint_type = 'unique';

EXEC('alter table [dbo].[Company] drop constraint ' + @unique_constraint_name);

alter table dbo.Company 
alter column Code nvarchar(20) not null;

alter table dbo.Company
add constraint UQ_Company_Code unique (Code);

alter table dbo.Company 
alter column Name nvarchar(50) null;