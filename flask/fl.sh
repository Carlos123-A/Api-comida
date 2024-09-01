#!/bin/bash

docker build -t flask-app .

docker run -d --name flask-app --network my-network -p 8000:8000 flask-app
