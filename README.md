Prerequisites
- Visual Studio 2022 or any other IDE by choice
- Docker Desktop
- SSMS or other SQL client



1. How to run MSSQL server?
cd /SyncpWallet
run command - docker-compose up

2. How to initialize the database?
Option 1.Using sqlcmd and command line
- open cmd
- run the following command - sqlcmd -S localhost,1433 -U sa -P YourNewStrong@Passw0rd -i database-initialize.sql
Option 2. Using SSMS
- Open SSMS by using credentials set in docker-compose file
- Click on database -> New query and paste the query from database-initialize.sql file

3. If the database-initialize.sql file is executed successfully, you should see 1 table and 1 stored procedure created.