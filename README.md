# Purpose
To learn how to develop an ASP.NET Core 2 MVC web application using a MySQL database that can run on either localhost or on cloud.gov.

# Microsoft Tutorial Used
```https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/```

# Database (MySQL)
## (localhost only)
```
CREATE database moviedb;
USE moviedb; 
CREATE TABLE Movie (both localhost and cloud.gov)
(
	ID			MEDIUMINT 		NOT NULL AUTO_INCREMENT,
 	Title      	VARCHAR(100)	NOT NULL, 
	ReleaseDate	VARCHAR(100)	NOT NULL,
 	Genre      	VARCHAR(100) ,
 	Rating     	VARCHAR(100) ,
 	Price      	VARCHAR(100)	NOT NULL,
	PRIMARY KEY	(ID)
);
```
## (cloud.gov only)
```
CREATE TABLE Movie
(
	ID			MEDIUMINT 		NOT NULL AUTO_INCREMENT,
 	Title      	VARCHAR(100)	NOT NULL, 
	ReleaseDate	VARCHAR(100)	NOT NULL,
 	Genre      	VARCHAR(100) ,
 	Rating     	VARCHAR(100) ,
 	Price      	VARCHAR(100)	NOT NULL,
	PRIMARY KEY	(ID)
);
```

# Setting Environment Variables
## localhost only (on Mac or Linux)
```
export LOCAL_CONNECTION_STRING="Username=<insert username here>;Password=<insert password here>;Host=localhost;Port=3306;Database=moviedb;Pooling=true;"
```
# Using cloud.gov
Learning how to use cloud.gov is a bit involved and beyond the scope of this README file. I recommend starting here: 
1. ```https://cloud.gov/quickstart/```
2. ```https://cloud.gov/docs/services/relational-database/```
