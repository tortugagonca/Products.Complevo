# Products.Complevo

## Swagger API
	URL: http://localhost:32033/swagger/index.html

## Usefull commands
### Build application
	make build
### Run infra
	make infra
### Run application with sql 
	make app
### Stop running containers
	make stop
### Stop and remove containers
	make clean

### Docker

	Liste containers: docker container ls -a

	Acess container: docker container exec -it <CONTAINER ID> bash

 
### SQL Server

	URL: http://localhost:1433
	USER: sa
	PASS: Sql_p4ssword
 
### Postman collection
	 There's a collection on postman/Products-Complevo.postman_collection.json that can be used to consume and tests the APIs with Postman