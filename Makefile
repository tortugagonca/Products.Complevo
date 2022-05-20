.PHONY: build infra app clean stop 
		
build:	
	@docker-compose build

infra:
	@docker-compose up -d sql-database

app: build infra
	@docker-compose up -d  products.complevo.presentation

clean:
	@docker-compose down

stop:
	@docker-compose stop