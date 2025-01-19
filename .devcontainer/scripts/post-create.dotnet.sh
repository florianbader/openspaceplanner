#!/bin/bash

# restore dotnet tooling
sudo dotnet workload update
dotnet tool restore

dotnet restore
