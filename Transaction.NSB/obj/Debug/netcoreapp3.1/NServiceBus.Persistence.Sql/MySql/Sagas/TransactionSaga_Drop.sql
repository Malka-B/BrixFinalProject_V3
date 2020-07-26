
/* TableNameVariable */

set @tableNameQuoted = concat('`', @tablePrefix, 'TransactionSaga`');
set @tableNameNonQuoted = concat(@tablePrefix, 'TransactionSaga');


/* DropTable */

set @dropTable = concat('drop table if exists ', @tableNameQuoted);
prepare script from @dropTable;
execute script;
deallocate prepare script;
