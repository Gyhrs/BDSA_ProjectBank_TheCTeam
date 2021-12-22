# BDSA_ProjectBank_TheCTeam
The C Team's BDSA project 2021 ITU: StudyBank.

## How to run the program from "scratch"
Open Docker.
Make sure you aren't running any Docker container with port 1433 exposed. 
Run startup.ps1 with PowerShell.

Alternatively run these commands:
```powershell
$project = "./MyApp/Server"
$password = New-Guid

docker rm --force StudyBankDB
docker run --name StudyBankDB -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest 
$database = "StudyBank"
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password"

dotnet user-secrets init --project $project
dotnet user-secrets set "ConnectionStrings:StudyBank" "$connectionString" --project $project

dotnet ef database update -p .\MyApp\Infrastructure\ -s .\MyApp\Server\ 

dotnet run --project $project
```
