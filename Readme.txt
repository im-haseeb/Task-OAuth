
* 	Please Update the database connection string as required in appsettings.json

* 	In the Documentation it was mentioned that using OAUTH, as OAUTH is the integration of login system using third party Services like google, Facebook etc,
	but in that document it was also metioned that userdata shuld be static with username and password so thats why I Performed the Custom Authentication With JWT
	using Custom Claims, roles, and scopes

*	After running the application there are three user that are by default seeding into the database using SeedUser.cs Service

User: 

username: admin@game.com
password: admin
---------------------------

username: normaluser@game.com
password: normal
---------------------------

username: vipuser@game.com
password: vip
