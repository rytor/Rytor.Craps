psql -U $POSTGRES_USER -d account -f /usr/scripts/sql/account_ddl.sql
psql -U $POSTGRES_USER -d balance -f /usr/scripts/sql/balance_ddl.sql
psql -U $POSTGRES_USER -d balance -f /usr/scripts/sql/balance_dml.sql
psql -U $POSTGRES_USER -d game -f /usr/scripts/sql/game_ddl.sql
psql -U $POSTGRES_USER -d game -f /usr/scripts/sql/game_dml.sql
