How to get and run this application on your computer?

Via .NET CLI:
1. Open terminal or cmd (on Windows) and write this command: git clone [repository url]. Press ENTER
2. Write: cd src. Press ENTER
3. Write: dotnet build. If everything is ok, press ENTER
4. Write: cd EventManager. Press ENTER
5. Write: dotnet run --project EventManager.csproj. Press ENTER
6. Have fun with my application!

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
							
Features from the previous branch (sprint2):
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

Features of the current branch (sprint3):
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

