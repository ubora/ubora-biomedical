$isPostgresRunning | docker inspect --format=" {{ .State.Running }} " ubora_postgres
if(!$isPostgresRunning) {
    docker rm -f ubora_postgres
    docker run -d -p 5400:5432 --name "ubora_postgres" -e 'POSTGRES_PASSWORD=ubora' -e 'POSTGRES_DB=ubora' postgres
}
$Env:ConnectionStrings__ApplicationDbConnection = "server=localhost;Port=5400;userid=postgres;password=ubora;database=postgres"
echo "restoring packages..."
dotnet restore
echo "building web..."
dotnet build
echo "starting web..."
dotnet run