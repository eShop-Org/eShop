  
pipeline {
    agent any
   stages {
         stage("createCredentials") {
         steps {
            createCredentials
         }
      }
      stage("Docker Compose build") {
         steps {
            sh "docker-compose -d --build eShop-Org/eShop:${env.BUILD_ID} ."
         }
      }
      stage("Login to docker hub") {
         steps {
           sh "docker login --username=${env.DOCKERHUB_USER_NAME} --password=${env.DOCKERHUB_PASSWORD}"
         }
      }
      stage("Docker push") {
         steps {
           sh "ddocker push nivzi/eshop:${env.BUILD_ID}"
         }
      }
      stage("Docker run ") {
         steps {
           sh " docker run -d -p 8080:80 --name eShop eShop-Org/eShop:${env.BUILD_ID}"
         }
      }
   } 
    post {
        always {
            cleanWs deleteDirs: true, notFailBuild: true
        }
    }
}