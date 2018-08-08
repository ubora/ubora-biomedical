#!/bin/bash
# Runs all tests and returns nonzero exit code, if any failed
(cd Ubora.Domain.Tests && dotnet test --verbosity normal --configuration Release)
exit_code=$?
(cd Ubora.Web.Tests && dotnet test --verbosity normal --configuration Release)
let exit_code=$?+$exit_code
if [ $exit_code -ne 0 ]; then
    echo "Some tests failed!"
fi
exit $exit_code