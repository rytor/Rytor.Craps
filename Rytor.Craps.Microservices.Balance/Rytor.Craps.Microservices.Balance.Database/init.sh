## Incomplete script to take backup file from Git repo and instantiate dockerized SQL Server database w/restored database
sudo docker exec -it db_rytorcraps mkdir /var/opt/mssql/backup/db_rytorcraps

sudo docker exec -it db_rytorcraps mkdir /var/opt/mssql/backup/db_rytorcraps/Balance

sudo docker cp home/rytor/Projects/Git/rytor/Rytor.Craps/Rytor.Craps.Microservices.Balance/Rytor.Craps.Microservices.Balance.Database/Balance.bak db_rytorcraps:/var/opt/mssql/backup/db_rytorcraps/Balance

sudo docker exec -it db_rytorcraps /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P <password> -Q 'DROP DATABASE BALANCE'

sudo docker exec -it db_rytorcraps /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P <password> -Q 'RESTORE DATABASE Balance FROM DISK = "/var/opt/mssql/backup/db_rytorcraps/Balance/Balance.bak" WITH MOVE "Balance" TO "/var/opt/m
ssql/data/Balance.mdf", MOVE "Balance_log" TO "/var/opt/mssql/data/Balance.ldf"'