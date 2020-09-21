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
		stage('Docker Run Pulumi') {
			steps {
				sh "docker run -it \
    -e PULUMI_ACCESS_TOKEN \
    -e AWS_ACCESS_KEY_ID \
    -e AWS_SECRET_ACCESS_KEY \
    -e AWS_REGION \
    -w /app \
    -v $(pwd):/app \
    --entrypoint bash \
    pulumi/pulumi \
    -c "npm install && pulumi preview --stack dev --non-interactive""
			}
		}

	}
	post {
		always {
			cleanWs deleteDirs: true, notFailBuild: true
		}
	}
}
