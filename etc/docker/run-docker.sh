#!/bin/bash

if [[ ! -d certs ]]
then
    mkdir certs
    cd certs/
    if [[ ! -f localhost.pfx ]]
    then
        dotnet dev-certs https -v -ep localhost.pfx -p 5402bcae-7e03-47d7-ba9b-bee7542fec26 -t
    fi
    cd ../
fi

docker-compose up -d
