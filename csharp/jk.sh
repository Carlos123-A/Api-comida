#!/bin/bash

docker build -t comida .

docker run -d --name comida-1 --network my-network -p 5000:5000 comida
