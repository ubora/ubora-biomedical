#!/bin/bash
chmod +x Ubora.Web.FunctionalTests/wait-for-it.sh
docker-compose -f "docker-compose.ci.functional-tests.yml" up --abort-on-container-exit --exit-code-from "functional.tests"
#Check the docker-compose ps output and scan the exit codes and exit with an error code if any of the containers failed to exit gracefully
docker-compose -f "docker-compose.ci.functional-tests.yml" ps -q | xargs docker inspect -f '{{ .State.ExitCode }}' | while read code; do  
    if [ "$code" != "0" ]; then    
       exitCode=1
    fi
done 
docker-compose -f "docker-compose.ci.functional-tests.yml" down
exit $exitCode