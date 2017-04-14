#!/bin/bash
chmod +x RunCIBuild.sh
docker-compose -f "docker-compose.ci.build.yml" run -e TEAMCITY_PROJECT_NAME=$1 ci-build
exitCode=$?
docker-compose -f "docker-compose.ci.build.yml" down
exit $exitCode