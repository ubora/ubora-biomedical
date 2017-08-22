$isPostgresRunning | docker inspect --format=" {{ .State.Running }} " ubora_postgres

if(!$isPostgresRunning) {
    docker rm -f ubora_postgres
    docker run -d -p 5400:5432 --name "ubora_postgres" -e 'POSTGRES_PASSWORD=ubora' -e 'POSTGRES_DB=ubora' postgres
}

$isAzureEmulatorRunning | docker inspect --format=" {{ .State.Running }} " ubora_azurite

if(!$isPostgresRunning) {
    docker rm -f ubora_azurite
    docker run -d -p 32500:10000 --name "ubora_azurite" mpahk/azurite
}

$Env:ConnectionStrings__ApplicationDbConnection = "server=localhost;Port=5400;userid=postgres;password=ubora;database=postgres"
$Env:ConnectionStrings__AzureBlobConnectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:32500/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;"
$Env:ASPNETCORE_ENVIRONMENT = "Development";

Write-Host "Restoring packages..."
dotnet restore

Write-Host "Building project..."
dotnet build

Write-Host "Starting project..."
dotnet watch run http://0.0.0.0:5000
