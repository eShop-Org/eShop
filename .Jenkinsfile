pipeline {
  	environment {
    		DOCKER_REGISTRY = "nivzi/"
    		registryCredential = 'docker-creds'
			IMAGE_TAG = "${BUILD_NUMBER}"
  	}
	agent any
   	stages {
		stage('Docker-compose build eshopwebmvc') {
			steps {
				sh "docker-compose up -d --build eshopwebmvc"
				
			}
		}
		stage('Docker-compose build eshoppublicapi') {
			steps {
				sh "docker-compose up -d --build eshoppublicapi"
			}
		}
		stage('Login to docker hub') {
			steps {
				sh "docker login --username=${env.DOCKERHUB_USER_NAME} --password=${env.DOCKERHUB_PASSWORD}"
			}
		}
		stage('Docker push') {
			steps {
				sh "docker push  ${DOCKER_REGISTRY}eshopwebmvc:${BUILD_NUMBER}"
				sh "docker push  ${DOCKER_REGISTRY}eshoppublicapi:${BUILD_NUMBER}"
			}
		}
		stage('Terraform init') {
			steps {
				sh "terraform init"
			}
		}
		stage('Terraform plan') {
			steps {
				sh "terraform plan -out eShop.tfplan"
			}
		}
		stage('Terraform apply') {
			steps {
				sh "terraform apply --auto-approve"
			}
		}
	}
	post {
		always {
			cleanWs deleteDirs: true, notFailBuild: true
		}
	}
}