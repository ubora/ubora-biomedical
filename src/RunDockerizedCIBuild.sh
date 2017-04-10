#!/bin/bash
set -e

chmod +x RunCIBuild.sh
docker-compose -f "docker-compose.ci.build.yml" run ci-build
docker-compose -f "docker-compose.ci.build.yml" down