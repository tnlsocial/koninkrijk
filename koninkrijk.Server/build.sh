#!/bin/bash
git pull
docker image prune -a -f
docker build -t koninkrijk-api . -f Dockerfile

