How to get and run this application on your computer?

I.  Before sprint 9 (legacy)
	1. Open the projects's root folder (e.g. C:\EventManager) in terminal or cmd (on Windows)
	2. Navigate to the EventManager folder.
	3. Enter the command: "dotnet build". If everything is ok, press ENTER
	4. Enter the command: "dotnet run --project EventManager.csproj". Press ENTER
	5. Have fun with EventManager!

II. After sprint 9
	The application had been splited into 3 microservices.
	In this topic we will look at how to run the Users microservice.
	Each of the microservices has the same running instructions.

	1. Open the projects's root folder (e.g. C:\EventManager) in terminal or cmd (on Windows)
	2. Navigate to the Users folder.
	3. Navigate to the Users.API folder.
	4. Enter the command "dotnet run --launch-profile https" Press ENTER
	5. Have fun with the Users microservice! 



Features from the sprint2:
1. The EventsController.All has parameters:
	```csharp
	EventsController.All(
		[FromQuery] string? title, 
		[FromQuery] DateTime? from, 
		[FromQuery] DateTime? to, 
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 10)
	```

	More about EventsController.All new parameters. 
	"title", "from" and "to" optional filers. In other words, these parameters can be null.
	We have to pay a little attention to the "title" parameter. This parameter helps to implement
	partial matching, i.e. user can set title = "day" and find e.g. "Son's birthday" or "Thanksgiving day".
	Last two parameters (page and pageSize) help to implement pagination.

2. Errors handling unification.
	In the current project version we use child classes of the WebApiException class.
	WebApiException contains object of the Error class that contains 2 read only
	properties: StatusCode (int) and Message (string). When server receives bad data,
	one of the methods of the EventsService throws exception. After that CustomExceptionMiddleware
	handles this exception and modifies response to client by setting status code and adding to response
	body serialized to JSON object of the Error class.

	One important moment: if CustomExceptionMiddleware catches other type of exception (not WebApiException),
	CustomExceptionMiddleware handles it as internal server error (status code 500).
				
	Example of the response body:

	{
       "statusCode": 404,
       "message": "Event with id = '00000000-0000-0000-0000-000000000000' was not found!"
    }

	Fields of the response:
	1. statusCode - contains http status code of the response (In this example 404 or not found)
	2. message - contains description of error


Features from the sprint3:
1. New domain model - Booking. It contains:
	a. Guid Id - primary key. Required field.
	b. Guid EventId - event id. Required field.
	c. DateTime CreatedAt - date and time when booking had been created. Required field.
	d. DateTime ProcessedAt - date and time when booking had been processed. Optional field.
	e. BookingStatus Status - status of the booking. Required field. The Booking class serializes this field to string

	BookingStatus is enum with values:
	-Pending = 0
	-Confirmed = 1
	-Rejected = 2
	
2. Add new (and first in this project) background service for handling booking. The service class extends the BackgroundService class. How does it works?
	In the ExecuteAsync method service tryies to get all Bookings with Status = "Pending". After that it changes their status from "Pending" 
	to "confirmed".

3. Add new end point: EventsController.Book:
	 Book(
         [FromRoute] Guid id,
         CancellationToken cancellationToken)
	More about EventsController.Book parameters. Id - primary key of the Event that user is going to book.
	The cancellationToken is the object of the structure CancellationToken (More about this structure you can learn here:
    https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken?view=net-10.0) 
				
	Example of the response body:
	{
      "id": "196dae0b-2673-4e4d-b3c2-99ba915f73e6",
      "message": "Your request is pending!",
      "url": "https://localhost:7199/bookings/196dae0b-2673-4e4d-b3c2-99ba915f73e6"
    }
	
	One important moment: status code is 202 (Accepted)

4. Add new controller - BookingController. Now it has only one endpoint - BookingController.GetById :
	GetById([FromRoute] Guid id)
	More about EventsController.Book parameters. Id - primary key of the Booking that user is trying to get.

5. Add new project - EventManager.Queues. This projects contains some classes for task queues. But
	now you can't see samples of using classes from this library in this applications. In future branches
	classes from this library will be used in the code.


Features from the sprint4:
1. Modify the Event
	- Add TotalSeats property (shows total count of seats at this event)
	- Add AvailableSeats property (shows count of free seats at this event)
	- Add TryReverseSeats(int count = 1) method (provides decreasing in number of available seats)
	- Add TryReleaseSeats(int count = 1) method (provides releasing seats)
	
	The TryReverseSeats and TryReleaseSeats methods use the _lock for critical section protection.

2. Modify the EventsController.Book: when Event.AvaliableSeats = 0 (overbooking), this end point
	sends response with status code 409 (conflict).

	Sample of overbooking sutuiation: one event (e.g. hackaton) has only five avaliable seats.
	But there are twenty users who want to take part in this event. Every one tries too book place,
	but server will send 5 respones with status code 202 (accepted) and 15 responses with status
	code 409 (conflict).

3. Modify the BookingHandlingService. Add new method - ProcessBookingsAsync. This method handles new bookings.
   How does it works? The ProcessBookingsAsync uses try-catch-finally construction because protection of the
   criticall area implemented via semaphore. In try code block object of the SemaphoreSlim class invokes the WaitAsync()
   method. After that IEventsService.GetEventByIdAsync tries to get object of the Event class. If everything is Ok,
   booking changes its status from "Pending" to "Confirmed". If somethings went wrong booking changes its status from
   "Pending" to "Rejected". In finally code block object of the SemaphoreSlim class invokes the Release() method.

4. Remove the EventManager.Queues project.


Features from the sprint5:
1. Use the PostgreSQL database management system from storing data. If you want to run this application, you have to install PostgreSQL to your device
2. Modify appsetting.json: add connection string. If PostgreSQL on your device had another parameters (e.g. another port) you can edit conncection string
3. If your PostgreSQL server does not contains the eventmanager database created new database, the application will create it automatically via Database.EnsureCreated
4. Modify tests project: now it contains in-memory provider for creating test cases


Features from the sprint7:
1. Use migrations instead of Database.EnsureCreated
	How to create new migration? There are two ways:

	Via dotnet cli:
	A. Open cmd or terminal in the 'EventManager.DataAccess.PostgreSQL' directory
	B. Write "dotnet ef migrations add <migrationName>" and press Enter or Return
	C. Write "dotnet ef database update" and press Enter or Return
	
	Via Visual Studio 2026:
	A. Open package manager console
	B. Write "Add-Migration <migrationName>" and press Enter
	C. Write "Update-Database" and press Enter
	
2. Add docker-compose.yml. 
    So, you do not need installed PostgreSQL and pgAdmin on your device, but docker is required (especially for integration tests)

3. Add integration tests. This type of tests provides testing of interactions between lots of different components of the application.


Features from the last branch (sprint7):
1. The EventManager contains 4 layers:
	- Domain (project: EventManager.Domain)
	- Application (project: EventManager.Application)
	- Infrastructure (project: EventManager.Infrastructure.PostgreSQL)
	- Presentation (projects: EventManager, EventManager.API)


2. Remove IEventsService and IBookingsService. For interacting with events and bookings use classes from the EventManager.Handlers projects.

3. Split integration tests into 2 independant projects: EventManager.Tests.Integration and EventsManager.Tests.End2End. Both projects require docker to be installed on your device.  

4. Using clean architecture principles: the application and domain layers know nothing about the Infrastructure layer.

5. Add value objects.

6. Refusing from some installed libs (e.g. FluentValidation)


Features from the sprint 8:
1. Add new entity - user. Every user has role. There are 2 roles: user and admin.
   Admin can create, edit and events, cancelliing every booking. Users can make 
   new bookings and cancelling their bookins.
   Users with both roles can have only 10 active bookings.

2. Modify validation system: add IValidatableValueObject and IValidatableEntity interfaces
 
3. Implementation of Authentication and Authorization
   The application uses JWT for authentication. JWT is an open standard (RFC 7519).
   Jwt consist of three parts: a header, a payload and a signature.
   The application uses the JwtHmacSha256Wyzard class for generating tokens.
   Most endpoints require authentication. To test protected endpoints in Swagger, 
   enter the username and password into the corresponding fields, click the "Execute" 
   button, copy the long string from the response body, click the "Authorize" button, 
   paste the token value into the input field, and click "Authorize" again.

4. Modify appsettings.json. Now it contains the secret key for creating signatures.
   WARNING! In production secret key should be kept in user secrets or in environment
   variables!

Features from the sprint 9:
1. The EventManager application had been splited into 3 microservises:
	a. Events.
       The service provides basic CRUD operations for the events.
	   The service's database contains 2 types of tables:
	   1. Tables for storing domain entities (the Events table)
	   2. Tables for storing Kafka messages (InboxPendingMessages, InboxCancelledMessages and OutboxConfirmedBookingsMessages). This enables the implementation of the Inbox and Outbox patterns.
	   The file and folder structure of this microservice is as follows:
	  ```
		Events/
		├─ Events.API/
		│  ├─ Connected Services/
		│  ├─ Properties/
		│  ├─ Api/
		│  ├─ Contracts/
		│  ├─ Validators/
		│  ├─ appsettings.json
		│  └─ Program.cs
		├─ Events.Application/
		│  ├─ Dtos/
		│  ├─ Handlers/
		│  ├─ Repositories/
		│  ├─ DependenciesInjection.cs
		│  └─ IPublisher.cs
		├─ Events.Domain/
		│  ├─ Exceptions/
		│  ├─ ValueObjects/
		│  └─ EventEntity.cs
		└─ Events.Infrastructure/
		   ├─ Configurations/
		   ├─ Messaging/
		   ├─ Migrations/
		   ├─ Repositories/
		   ├─ DependenciesInjection.cs
		   ├─ EventsDbContext.cs
		   └─ EventsDesignFactory.cs
	```

	b. Users
	   This service handles authorization and aythentification. 
	   The service's database stores user data.
	   The file and folder structure of this microservice is as follows:
	```
	   Users/
           ├─ Users.API/
           │  ├─ Connected Services/
           │  ├─ Properties/
           │  ├─ Api/
           │  ├─ Extensions/
           │  ├─ appsettings.json
           │  └─ Program.cs
           ├─ Users.Application/
           │  ├─ Contracts/
           │  ├─ Dtos/
           │  ├─ Repositories/
           │  ├─ Security/
           │  ├─ Services/
           │  └─ DependenciesInjection.cs
           ├─ Users.Domain/
           │  ├─ ValueObjects/
           │  └─ User.cs
           ├─ Users.Infrastructure.Postgre/
           │  ├─ Configurations/
           │  ├─ Migrations/
           │  ├─ Repositories/
           │  ├─ DependenciesInjection.cs
           │  ├─ UsersDbContext.cs
           │  └─ UsersDesignFactory.cs
           └─ Users.Infrastructure.Security/
           ├─ Jwt/
           ├─ DependenciesInjection.cs
           └─ PasswordHasherSHA256.cs
	```
	
	- Bookings
		The service provides basic CRUD operations for the bookings.
		The service's database stores bookings. Bookings has 4 statuses: Pending, Confirmed, Cancelled and Rejected.
		The file and folder structure of this microservice is as follows:
	```
		Bookings/
            ├─ Bookings.API/
            │  ├─ Connected Services/
            │  ├─ Properties/
            │  ├─ Api/
            │  ├─ appsettings.json
            │  └─ Program.cs
            ├─ Bookings.Application/
            │  ├─ Dtos/
            │  ├─ Handlers/
            │  ├─ Publishers/
            │  ├─ Repositories/
            │  └─ DependenciesInjection.cs
            ├─ Bookings.Domain/
            │  ├─ Enums/
            │  └─ Booking.cs
            └─ Bookings.Infrastructure/
               ├─ Configurations/
               ├─ Messaging/
               ├─ Migrations/
               ├─ Repositories/
               ├─ BookingsDbContext.cs
               ├─ BookingsDesignFactory.cs
               └─ DependenciesInjection.cs
	```
	
2. Each microservice has its own database and migrations. All databases are run in a Docker container.

3. Kafka is used as the message broker; the Kafka server runs in a Docker container.

4. Add global.json that contains shared confuguration options (kafka, jwt and e.t.c.)

5. Add a lot of shared libraries (e.g. Shared.Objects, Shared.Validation and e.t.c.)


Features from the sprint 10:
1. Add new end point - events/top
   This endpoint sends to client 10 most popular events.

2. Implemented result caching for specific endpoints (`GET events/top` and `GET events/{id}`) using Redis.

3. Modified the appsettings.json file in the Events.API project: add two new sections - CacheKeysOptions and RedisOptions.

4. Added the ICacheRepository

5. Created the RedisCashAsideRepository and RedisReadThrowRepository. These classes implement the ICacheRepository interface

6. Implemented cache invalidation for the GET events/{id} endpoint.
   Invalidation is performed by removing stale data.
   This method was selected as one of the most reliable invalidation strategies.

7. Modified the docker-compose.yml file (added a Redis container).


Features from the sprint 11: