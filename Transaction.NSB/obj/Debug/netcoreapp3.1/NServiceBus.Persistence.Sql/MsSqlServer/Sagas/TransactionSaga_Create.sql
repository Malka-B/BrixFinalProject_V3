
/* TableNameVariable */

declare @tableName nvarchar(max) = '[' + @schema + '].[' + @tablePrefix + N'TransactionSaga]';
declare @tableNameWithoutSchema nvarchar(max) = @tablePrefix + N'TransactionSaga';


/* Initialize */

/* CreateTable */

if not exists
(
    select *
    from sys.objects
    where
        object_id = object_id(@tableName) and
        type in ('U')
)
begin
declare @createTable nvarchar(max);
set @createTable = '
    create table ' + @tableName + '(
        Id uniqueidentifier not null primary key,
        Metadata nvarchar(max) not null,
        Data nvarchar(max) not null,
        PersistenceVersion varchar(23) not null,
        SagaTypeVersion varchar(23) not null,
        Concurrency int not null
    )
';
exec(@createTable);
end

/* AddProperty TransactionId */

if not exists
(
  select * from sys.columns
  where
    name = N'Correlation_TransactionId' and
    object_id = object_id(@tableName)
)
begin
  declare @createColumn_TransactionId nvarchar(max);
  set @createColumn_TransactionId = '
  alter table ' + @tableName + N'
    add Correlation_TransactionId uniqueidentifier;';
  exec(@createColumn_TransactionId);
end

/* VerifyColumnType Guid */

declare @dataType_TransactionId nvarchar(max);
set @dataType_TransactionId = (
  select data_type
  from INFORMATION_SCHEMA.COLUMNS
  where
    table_name = @tableNameWithoutSchema and
    table_schema = @schema and
    column_name = 'Correlation_TransactionId'
);
if (@dataType_TransactionId <> 'uniqueidentifier')
  begin
    declare @error_TransactionId nvarchar(max) = N'Incorrect data type for Correlation_TransactionId. Expected uniqueidentifier got ' + @dataType_TransactionId + '.';
    throw 50000, @error_TransactionId, 0
  end

/* WriteCreateIndex TransactionId */

if not exists
(
    select *
    from sys.indexes
    where
        name = N'Index_Correlation_TransactionId' and
        object_id = object_id(@tableName)
)
begin
  declare @createIndex_TransactionId nvarchar(max);
  set @createIndex_TransactionId = N'
  create unique index Index_Correlation_TransactionId
  on ' + @tableName + N'(Correlation_TransactionId)
  where Correlation_TransactionId is not null;';
  exec(@createIndex_TransactionId);
end

/* PurgeObsoleteIndex */

declare @dropIndexQuery nvarchar(max);
select @dropIndexQuery =
(
    select 'drop index ' + name + ' on ' + @tableName + ';'
    from sysindexes
    where
        Id = object_id(@tableName) and
        Name is not null and
        Name like 'Index_Correlation_%' and
        Name <> N'Index_Correlation_TransactionId'
);
exec sp_executesql @dropIndexQuery

/* PurgeObsoleteProperties */

declare @dropPropertiesQuery nvarchar(max);
select @dropPropertiesQuery =
(
    select 'alter table ' + @tableName + ' drop column ' + column_name + ';'
    from INFORMATION_SCHEMA.COLUMNS
    where
        table_name = @tableNameWithoutSchema and
        table_schema = @schema and
        column_name like 'Correlation_%' and
        column_name <> N'Correlation_TransactionId'
);
exec sp_executesql @dropPropertiesQuery

/* CompleteSagaScript */
