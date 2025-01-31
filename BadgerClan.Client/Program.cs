using BadgerClan.Logic;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();
var app = builder.Build();

string url = app.Configuration["ASPNETCORE_URLS"]?.Split(";").Last() ?? throw new Exception("Unable to find URL");
int port = new Uri(url).Port;

Console.Clear();
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("Welcome to the Sample BadgerClan Bot!");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("Change the code in Program.cs to add custom behavior.");
Console.WriteLine("If you're running this locally, use the following URL to join your bot:");
Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine($"\t{url}");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();
Console.WriteLine("For the competition, start a DevTunnel for this port with the following commands:");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\t winget install Microsoft.devtunnel");//DevTunnel explanation: https://learn.microsoft.com/en-us/azure/developer/dev-tunnels/overview
Console.WriteLine("\t [ restart your command line after installing devtunnel ]");
Console.WriteLine("\t devtunnel user login");
Console.WriteLine($"\t devtunnel host -p {port} --allow-anonymous");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();
Console.WriteLine("In the output from the 'devtunnel host' command, look for the \"Connect via browser:\" URL.  Paste that in the browser as your bot's address");


app.MapGet("/", () => "Sample BadgerClan bot.  Modify the code in Program.cs to change how the bot performs.");
int moveSet = 2;
app.MapPost("/", (MoveRequest request) =>
{
    // ***************************************************************************
    // ***************************************************************************
    // **
    // ** Your code goes right here.
    // ** Look in the request object to see the game state.
    // ** Then add your moves to the myMoves list.
    // **
    // ***************************************************************************
    // ***************************************************************************
    var myMoves = new List<Move>();
    if (moveSet == 1)
    {
        myMoves = SuperSimpleExampleBot.MakeMoves(request);//Very simple bot example.  Delete this line when you write your own bot.
    }
    else if (moveSet == 2)
    {
        myMoves = ModifiedSimple.MakeMoves(request);
    }
    else if(moveSet == 3)
    {
        myMoves = KiteBack.MakeMoves(request);
    }
    return new MoveResponse(myMoves);
});

app.MapPost("/setmove/{Id}", (int Id) =>
{
    moveSet = Id;
});

app.Run();
