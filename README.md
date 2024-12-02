# Car Dealer API

An API for Car Dealer to add/remove car, alter car stock, search car by make/model, and get list of cars

Created by Timotius Hansel Kenny as a technical test assesment for Junior Developer position at OracleCMS

## Get started

I used Visual Studio 2019, and ASP.NET Core 3.1 as the framework. 
I've used dapper as it's ORM, and using sqlite as it's db.
So you also need to install SDK for ASP.NET Core 3.1

Open CarDealerAPI.sln using Visual Studio,
Then build and run the API in Visual Studio.

Navigate to [localhost:5001/swagger](https://localhost:5001/swagger). 
You should see your app running. 

For the first run, i've set the Database Context to automaticly create 
CarData table and DealerData table. They are linked through DealerID column in CarDealer table as foreign key.
I've set it that way so any dealer can't modify other dealer car's stock.
# Also on the first run, i've inserted a default Dealer's username and password :

    Username : THK
    Password : THK

This credential will be used to generate unique JWT token for each dealer. So everytime Dealer's using this API, they have to use
their own bearer jwt token.

You can create this token via swagger ui, using /auth endpoint. 
Insert those credential via the body request.
This will return JWT token.

Copy those token into authorise button on right top corner of swagger's UI.

After this step, you can try using other endpoint.

# Car's endpoint

This endpoint consist of :
    1. GetAll : This will return all the car's stocks and information from a dealer. Other dealer car's information wont be shown.
    2. searchbymakemodel : This consist of 2 query on the request. Query to search from make and model.
        The SQL query is : Select * from CarData where dealerid=(dealer's id) and make like '%(query make)%' and model like '%(query model)%'
    3. Get(id) : To get a car data based on it's id.
    4. Create : To create car's for the first time for each dealer.
    5. Update : To update certain's car stocks
    6. Delete : To soft delete ( flagged by column isdeleted in the table ) a car.

# Dealer's endpoint

This endpoint consist of :
    1. GetAll : This will return all dealer's data that are registered.
    2. GetById : To search a dealer's information based on its id.
    3. Create : To create a dealer.
    4. Update : to update a dealer's information.
    5. Delete : To soft delete a dealer.

All of post & put endpoint has been validated through model state validation.
Ex : When creating a car, you cannot fill in empty on it's make and model. Or you can't put non numeric to it's year and stock.






