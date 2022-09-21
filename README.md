# sample-masstransit

- .NET 6.0
- Swashbuckle Swagger
- Serilog
- MassTransit

## Run online on Play with Docker
[![Try in PWD](https://raw.githubusercontent.com/play-with-docker/stacks/master/assets/images/button.png)](https://labs.play-with-docker.com/?stack=https://raw.githubusercontent.com/hgmauri/sample-masstransit/master/pwd-docker-compose.yml)

## Or locally
In the directory with the docker-compose.yml run the command `docker-compose up -d`
After complete, give it a few seconds, and you can browse to:

### RabbitMQ
http://localhost:15672/
```
user: guest
pass: guest
```
### JAEGER UI
http://localhost:16686/

### WebApi Swagger UI
http://localhost:5053/swagger/index.html


---
https://henriquemauri.net/rabbitmq-com-masstransit-no-net-6/
