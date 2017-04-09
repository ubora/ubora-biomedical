#!/bin/bash
/bin/bash -c "dotnet restore ./Ubora.sln && sh RunTests.sh && dotnet publish ./Ubora.sln -c Release -o ./obj/Docker/publish"
docker-compose -f "docker-compose.ci.build.yml" run ci-build
