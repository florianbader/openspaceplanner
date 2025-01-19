#!/bin/bash

chmod +x ./.devcontainer/scripts/post-create.git.sh
./.devcontainer/scripts/post-create.git.sh "$1"

chmod +x ./.devcontainer/scripts/post-create.dotnet.sh
./.devcontainer/scripts/post-create.dotnet.sh "$1"

chmod +x ./.devcontainer/scripts/post-create.node.sh
./.devcontainer/scripts/post-create.node.sh "$1"

chmod +x ./.devcontainer/scripts/post-create.git-hooks.sh
./.devcontainer/scripts/post-create.git-hooks.sh "$1"
