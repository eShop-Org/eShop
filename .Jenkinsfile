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
		stage ("Install dependencies") {
            steps {
                sh "curl -fsSL https://get.pulumi.com | sh"
                sh "$HOME/.pulumi/bin/pulumi version"
            }
        }

        stage ("Pulumi up") {
            steps {
                nodejs(nodeJSInstallationName: "node 14.11.0") {
                    withEnv(["PATH+PULUMI=$HOME/.pulumi/bin"]) {
                        sh "cd infrastructure && npm install"
                        sh "pulumi stack select ${PULUMI_STACK} --cwd infrastructure/"
                        sh "pulumi up --yes --cwd infrastructure/"
                    }
                }
            }
		}

	}
	post {
		always {
			cleanWs deleteDirs: true, notFailBuild: true
		}
	}
}
