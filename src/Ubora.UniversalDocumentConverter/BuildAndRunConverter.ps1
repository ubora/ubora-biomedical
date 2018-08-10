$isPandocRunning | docker inspect --format="{{ .State.Running }}" ubora_pandoc

if(!$isPandocRunning) {
    docker rm -f ubora_pandoc
    docker build -t ubora_pandoc .
    docker run -d -p 1337:1337 --name "ubora_pandoc" -e CONVERTER_API_KEY='testapikey' ubora_pandoc
}