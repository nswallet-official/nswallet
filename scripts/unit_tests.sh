set -e

# Debug log
set -x

# Enter project's directory
cd ${PROJECT_DIRECTORY}

# Install Coverlet Collector and add it as a package to unit tests project
dotnet tool install --global coverlet.console
dotnet add ${UNIT_TESTS_PROJECT} package coverlet.collector

# Restore & build unit tests project
dotnet restore
dotnet build ${UNIT_TESTS_PROJECT}

# Run unit tests with 'lcov' output format & coverlet settings
dotnet test --collect:"XPlat Code Coverage" --settings coverlet.runsettings --results-directory:"./coverage" ${UNIT_TESTS_PROJECT}
mv ./coverage/*/coverage.info coverage/lcov.info
