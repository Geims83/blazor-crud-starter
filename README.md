# blazor-crud-starter

This is a template Blazor server application, with a couple of CRUD pages already built in.
The CRUD pages are built with the FluentUI library, and the data access is done with Dapper.
The database is defined in the ./Db folder, deployed with SqlPackage.

To start development, start the database server with the provided deply-db.yml (a docker-compose file), and then debug the application.
A taskfile is provided to make this easier.
