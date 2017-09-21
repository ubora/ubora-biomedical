#!/bin/bash
set -e

dotnet clean ./Ubora.sln --configuration Release
rm -f -r Ubora.Web/obj/Docker/publish
dotnet restore ./Ubora.sln && bash RunTests.sh && dotnet publish ./Ubora.sln -c Release -o ./obj/Docker/publish