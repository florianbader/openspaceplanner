#!/bin/bash
OUTPUT_DIR="/usr/share/nginx/html"

for js_file in "$OUTPUT_DIR"/*.js; do
    for env_var in $(env | grep '^APP_' | cut -d= -f1); do
        env_value=${!env_var}
        if [ -n "$env_value" ]; then
            sed -i "s|\${$env_var}|$env_value|g" "$js_file"
        else
            sed -i "s|'[[:space:]]*\${$env_var}[[:space:]]*'|undefined|g" "$js_file"
            sed -i "s|\"[[:space:]]*\${$env_var}[[:space:]]*\"|undefined|g" "$js_file"
            sed -i "s|\${$env_var}|undefined|g" "$js_file"
        fi
    done
done

exec nginx -g "daemon off;"
