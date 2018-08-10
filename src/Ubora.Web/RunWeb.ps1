$Env:ConnectionStrings__ApplicationDbConnection = "server=localhost;Port=5400;userid=postgres;password=ubora;database=postgres"
$Env:ConnectionStrings__AzureBlobConnectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:32500/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;"
$Env:Pandoc__Ip = "127.0.0.1"
$Env:Pandoc__Url = "http://localhost:1337"
$Env:Pandoc__Key = "testapikey"
$Env:ASPNETCORE_ENVIRONMENT = "Development"

Write-Host "Restoring packages..."
dotnet restore

Write-Host "Building project..."
dotnet build

Write-Host "Webpack watching files..."
start powershell ./Webpack_using_watch_mode.ps1

Write-Host "Starting project..."
dotnet watch run --urls "http://0.0.0.0:5000"
