#!/bin/bash
set -e
while getopts u:p:r:t: option
do
        case "${option}"
        in
                u) id=${OPTARG};;
                p) secret=${OPTARG};;
                r) repo=${OPTARG};;
                t) tag=${OPTARG};;
        esac
done
# Tag can be also a git branch name
tag=${tag//\//-}

target="$repo:$tag"
# Setting AWS CLI credentials
aws configure set aws_access_key_id $id
aws configure set aws_secret_access_key $secret
# Getting temporary AWS ECR login
aws_login=$(aws ecr get-login --region eu-west-1)
aws_login=${aws_login%/https:\/\//}
# Login to AWS ECR without exposing secrets
l=$(eval $aws_login)
# Build container
cd Ubora.Web
docker build -t $target .
#docker tag $source $target
docker push $target
docker rmi $target





