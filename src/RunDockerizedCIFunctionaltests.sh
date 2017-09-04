#!/bin/bash
chmod +x Ubora.Web.FunctionalTests/wait-for-it.sh
docker-compose -f "docker-compose.ci.functional-tests.yml" build
docker-compose -f "docker-compose.ci.functional-tests.yml" run -e FunctionalTest functional.tests
exitCode=$?
docker-compose -f "docker-compose.ci.functional-tests.yml" down
exit $exitCode