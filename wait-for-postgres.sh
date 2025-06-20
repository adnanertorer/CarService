#!/usr/bin/env bash
# Using: wait-for-it.sh host:port -- command_to_run

hostport=$1
shift
until nc -z $(echo $hostport | cut -d: -f1) $(echo $hostport | cut -d: -f2); do
  echo "Waiting: $hostport..."
  sleep 1
done

echo "Connection ready: $hostport"
exec "$@"
