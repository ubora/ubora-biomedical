#!/bin/bash
# Runs all tests and returns nonzero exit code, if any failed
# using -framework netcoreapp2.0 -fxversion 2.0.6, which aspnetcore-build targeting 2.0.6
(cd Ubora.Domain.Tests && dotnet xunit -teamcity -framework netcoreapp2.0 -fxversion 2.0.6 --configuration Release)
exit_code=$?
(cd Ubora.Web.Tests && dotnet xunit -teamcity -framework netcoreapp2.0 -fxversion 2.0.6 --configuration Release)
let exit_code=$?+$exit_code
if [ $exit_code -ne 0 ]; then
    echo "Some tests failed!"
fi
exit $exit_code