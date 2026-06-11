How to get and run this application on your computer?

Via .NET CLI:
1. Open terminal or cmd (on Windows) and write this command: git clone [repository url]. Press ENTER
2. Write: cd .\EventManager\src\Presentation\EventManager\. Press ENTER
3. Write: dotnet build. If everything is ok, press ENTER
4. Write: dotnet run --project EventManager.csproj. Press ENTER
5. Have fun with my application!

Via Visual Studio 2026:
1. Open Visual Studio.
2. Press on the button "Clone a repository"
3. Write [repository url] to text box for repository location and press on the button "Clone"
4. Build -> Build solution (or use shortcut: ctrl + B or ctrl + Shift + B). If everything is ok, next step
5. Press on the button "Start Without Debugging" (or use shortcut: Shift + F5)
6. Have fun with my application!

How to run tests?
1. Open the EventsManager.Tests directory via cli
2. Write: dotnet build. If everything is ok, press ENTER
3. Then you will have 2 ways:
3.1 If you want to run all tests, write:  'dotnet test' or 'dotnet test path_to_your_foler\EventManager\src\EventsManager.Tests\EventsManager.Tests.csproj"'.
3.2 If you want to run group of tests, write:  dotnet test --filter <Group tittle>.
	For example: 'dotnet test --filter GetEvents' or 'dotnet test path_to_your_foler\EventManager\src\EventsManager.Tests\EventsManager.Tests.csproj" --filter GetEvents'
	
	

Features from the sprint2:
1. The EventsController.All has parameters:

	EventsController.All(
		[FromQuery] string? title, 
		[FromQuery] DateTime? from, 
		[FromQuery] DateTime? to, 
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 10) 

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


Features from the sprint6:
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