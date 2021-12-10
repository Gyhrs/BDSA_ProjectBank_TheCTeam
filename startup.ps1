$project = "./MyApp/Server"
$password = New-Guid

Write-Host "---- Starting SQL Server ----"
docker rm --force StudyBankDB
docker run --name StudyBankDB -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest 
$database = "StudyBank"
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password"

Write-Host "---- Configuring User Secrets ----"
Write-Host "Configuring Connection String"
dotnet user-secrets init --project $project
dotnet user-secrets set "ConnectionStrings:StudyBank" "$connectionString" --project $project

Write-Host "---- Migrating database ----"
dotnet ef database update -p .\MyApp\Infrastructure\ -s .\MyApp\Server\ 

Write-Host "---- Starting App ----"
dotnet run --project $project