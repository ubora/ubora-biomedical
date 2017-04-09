#!/bin/bash
chmod +x RunCIBuild.sh
docker-compose -f "docker-compose.ci.build.yml" run ci-build
buildresult=$?
docker-compose -f "docker-compose.ci.build.yml" down
exit $buildresult