
/* TableNameVariable */

set @tableNameQuoted = concat('`', @tablePrefix, 'TransactionSaga`');
set @tableNameNonQuoted = concat(@tablePrefix, 'TransactionSaga');


/* Initialize */

drop procedure if exists sqlpersistence_raiseerror;
create procedure sqlpersistence_raiseerror(message varchar(256))
begin
signal sqlstate
    'ERROR'
set
    message_text = message,
    mysql_errno = '45000';
end;

/* CreateTable */

set @createTable = concat('
    create table if not exists ', @tableNameQuoted, '(
        Id varchar(38) not null,
        Metadata json not null,
        Data json not null,
        PersistenceVersion varchar(23) not null,
        SagaTypeVersion varchar(23) not null,
        Concurrency int not null,
        primary key (Id)
    ) default charset=ascii;
');
prepare script from @createTable;
execute script;
deallocate prepare script;

/* AddProperty TransactionId */

select count(*)
into @exist
from information_schema.columns
where table_schema = database() and
      column_name = 'Correlation_TransactionId' and
      table_name = @tableNameNonQuoted;

set @query = IF(
    @exist <= 0,
    concat('alter table ', @tableNameQuoted, ' add column Correlation_TransactionId varchar(38) character set ascii'), 'select \'Column Exists\' status');

prepare script from @query;
execute script;
deallocate prepare script;

/* VerifyColumnType Guid */

set @column_type_TransactionId = (
  select concat(column_type,' character set ', character_set_name)
  from information_schema.columns
  where
    table_schema = database() and
    table_name = @tableNameNonQuoted and
    column_name = 'Correlation_TransactionId'
);

set @query = IF(
    @column_type_TransactionId <> 'varchar(38) character set ascii',
    'call sqlpersistence_raiseerror(concat(\'Incorrect data type for Correlation_TransactionId. Expected varchar(38) character set ascii got \', @column_type_TransactionId, \'.\'));',
    'select \'Column Type OK\' status');

prepare script from @query;
execute script;
deallocate prepare script;

/* WriteCreateIndex TransactionId */

select count(*)
into @exist
from information_schema.statistics
where
    table_schema = database() and
    index_name = 'Index_Correlation_TransactionId' and
    table_name = @tableNameNonQuoted;

set @query = IF(
    @exist <= 0,
    concat('create unique index Index_Correlation_TransactionId on ', @tableNameQuoted, '(Correlation_TransactionId)'), 'select \'Index Exists\' status');

prepare script from @query;
execute script;
deallocate prepare script;

/* PurgeObsoleteIndex */

select concat('drop index ', index_name, ' on ', @tableNameQuoted, ';')
from information_schema.statistics
where
    table_schema = database() and
    table_name = @tableNameNonQuoted and
    index_name like 'Index_Correlation_%' and
    index_name <> 'Index_Correlation_TransactionId' and
    table_schema = database()
into @dropIndexQuery;
select if (
    @dropIndexQuery is not null,
    @dropIndexQuery,
    'select ''no index to delete'';')
    into @dropIndexQuery;

prepare script from @dropIndexQuery;
execute script;
deallocate prepare script;

/* PurgeObsoleteProperties */

select concat('alter table ', table_name, ' drop column ', column_name, ';')
from information_schema.columns
where
    table_schema = database() and
    table_name = @tableNameNonQuoted and
    column_name like 'Correlation_%' and
    column_name <> 'Correlation_TransactionId'
into @dropPropertiesQuery;

select if (
    @dropPropertiesQuery is not null,
    @dropPropertiesQuery,
    'select ''no property to delete'';')
    into @dropPropertiesQuery;

prepare script from @dropPropertiesQuery;
execute script;
deallocate prepare script;

/* CompleteSagaScript */
