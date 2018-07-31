$isPostgresRunning | docker inspect --format=" {{ .State.Running }} " ubora_postgres

if(!$isPostgresRunning) {
    docker rm -f ubora_postgres
    docker run -d -p 5400:5432 --name "ubora_postgres" -e 'POSTGRES_PASSWORD=ubora' -e 'POSTGRES_DB=ubora' postgres
}

$isAzureEmulatorRunning | docker inspect --format=" {{ .State.Running }} " ubora_azurite

if(!$isAzureEmulatorRunning) {
    docker rm -f ubora_azurite
    docker run -d -p 32500:10000 --name "ubora_azurite" mpahk/azurite
}