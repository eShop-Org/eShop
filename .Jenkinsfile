  
pipeline {
    agent any
   stages {
      stage("Docker Compose build") {
         steps {
            sh "docker-compose up -d --build"
         }
      }
      stage("Login to docker hub") {
         steps {
           sh "docker login --username=${env.DOCKERHUB_USER_NAME} --password=${env.DOCKERHUB_PASSWORD}"
         }
      }
      stage("Docker push") {
         steps {
           sh "docker push nivzi/eshop:${env.BUILD_ID}"
         }
      }
      stage("Docker run ") {
         steps {
           sh " docker run -d -p 8080:80 --name eshop nivzi/eshop:${env.BUILD_ID}"
         }
      }
   } 
    post {
        always {
            cleanWs deleteDirs: true, notFailBuild: true
        }
    }
}