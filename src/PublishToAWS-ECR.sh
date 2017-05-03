#!/bin/bash
set -e
region=eu-west-1
while getopts u:p:s:t: option
do
        case "${option}"
        in
                u) id=${OPTARG};;
                p) secret=${OPTARG};;
                s) source=${OPTARG};;
                t) target=$OPTARG;;
        esac
done

# Setting AWS CLI credentials
aws configure set aws_access_key_id $id
aws configure set aws_secret_access_key $secret
# Getting temporary AWS ECR login
aws ecr get-login --region $region &>login.aws.ecr
# Login to AWS ECR
bash login.aws.ecr  
rm login.aws.ecr
cd Ubora.Web
docker build -t $target .
#docker tag $source $target
docker push $target 
docker rmi $target





