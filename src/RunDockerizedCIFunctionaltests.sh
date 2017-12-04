#!/bin/bash
chmod +x Ubora.Web.FunctionalTests/wait-for-it.sh
docker-compose -f "docker-compose.ci.functional-tests.yml" build
docker-compose -f "docker-compose.ci.functional-tests.yml" up --abort-on-container-exit --exit-code-from functional.tests
exitCode=$?
docker-compose -f "docker-compose.ci.functional-tests.yml" down
exit $exitCode