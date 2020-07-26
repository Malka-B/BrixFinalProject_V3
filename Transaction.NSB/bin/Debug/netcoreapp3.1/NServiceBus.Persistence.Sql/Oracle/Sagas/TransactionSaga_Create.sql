
/* TableNameVariable */

/* Initialize */

declare
  sqlStatement varchar2(500);
  dataType varchar2(30);
  n number(10);
  currentSchema varchar2(500);
begin
  select sys_context('USERENV','CURRENT_SCHEMA') into currentSchema from dual;


/* CreateTable */

  select count(*) into n from user_tables where table_name = 'TRANSACTIONSAGA';
  if(n = 0)
  then

    sqlStatement :=
       'create table "TRANSACTIONSAGA"
       (
          id varchar2(38) not null,
          metadata clob not null,
          data clob not null,
          persistenceversion varchar2(23) not null,
          sagatypeversion varchar2(23) not null,
          concurrency number(9) not null,
          constraint "TRANSACTIONSAGA_PK" primary key
          (
            id
          )
          enable
        )';

    execute immediate sqlStatement;

  end if;

/* AddProperty TransactionId */

select count(*) into n from all_tab_columns where table_name = 'TRANSACTIONSAGA' and column_name = 'CORR_TRANSACTIONID' and owner = currentSchema;
if(n = 0)
then
  sqlStatement := 'alter table "TRANSACTIONSAGA" add ( CORR_TRANSACTIONID VARCHAR2(38) )';

  execute immediate sqlStatement;
end if;

/* VerifyColumnType Guid */

select data_type ||
  case when char_length > 0 then
    '(' || char_length || ')'
  else
    case when data_precision is not null then
      '(' || data_precision ||
        case when data_scale is not null and data_scale > 0 then
          ',' || data_scale
        end || ')'
    end
  end into dataType
from all_tab_columns
where table_name = 'TRANSACTIONSAGA' and column_name = 'CORR_TRANSACTIONID' and owner = currentSchema;

if(dataType <> 'VARCHAR2(38)')
then
  raise_application_error(-20000, 'Incorrect data type for Correlation_CORR_TRANSACTIONID.  Expected "VARCHAR2(38)" got "' || dataType || '".');
end if;

/* WriteCreateIndex TransactionId */

select count(*) into n from user_indexes where table_name = 'TRANSACTIONSAGA' and index_name = 'SAGAIDX_B828FACB4CBDD8DD6A3F2C';
if(n = 0)
then
  sqlStatement := 'create unique index "SAGAIDX_B828FACB4CBDD8DD6A3F2C" on "TRANSACTIONSAGA" (CORR_TRANSACTIONID ASC)';

  execute immediate sqlStatement;
end if;

/* PurgeObsoleteIndex */

/* PurgeObsoleteProperties */

select count(*) into n
from all_tab_columns
where table_name = 'TRANSACTIONSAGA' and column_name like 'CORR_%' and
        column_name <> 'CORR_TRANSACTIONID' and owner = currentSchema;

if(n > 0)
then

  select 'alter table "TRANSACTIONSAGA" drop column ' || column_name into sqlStatement
  from all_tab_columns
  where table_name = 'TRANSACTIONSAGA' and column_name like 'CORR_%' and
        column_name <> 'CORR_TRANSACTIONID' and owner = currentSchema;

  execute immediate sqlStatement;

end if;

/* CompleteSagaScript */
end;
