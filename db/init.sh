#!/bin/bash

# Set the path to the workspace directory
workspace="./.."

# Find each master.sql file in the workspace directory and its subdirectories
find "$workspace" -name 'master.sql' -type f | while read file; do
    # Execute the file using sqlcmd
    sqlcmd -S localhost -U sa -P $SA_PASSWORD  -i "$file"
done