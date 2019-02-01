#!/bin/bash
set -e
rm -f -r Ubora.Web/obj/Docker/publish
dotnet restore ./Ubora.sln && dotnet build ./Ubora.sln --no-incremental && bash RunTests.sh && dotnet publish ./Ubora.sln -c Release -o ./obj/Docker/publish