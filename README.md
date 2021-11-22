# BDSA_ProjectBank_TheCTeam
The C Team's BDSA project 2021 ITU
## Run SQL Server in Docker Container

```powershell
$password = New-Guid
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
$database = "Studybank"
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password"
```

## Enable User Secrets

```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Studybank" "$connectionString"
```
