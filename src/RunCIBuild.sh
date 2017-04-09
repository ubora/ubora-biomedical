#!/bin/bash
dotnet restore ./Ubora.sln && sh RunTests.sh && dotnet publish ./Ubora.sln -c Release -o ./obj/Docker/publish