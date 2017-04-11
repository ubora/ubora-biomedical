#!/bin/bash
chmod +x RunCIBuild.sh
docker-compose -f "docker-compose.ci.build.yml" run ci-build
exitCode=$?
docker-compose -f "docker-compose.ci.build.yml" down
exit $exitCode