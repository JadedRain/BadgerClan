using BadgerClan.Logic;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();
var app = builder.Build();


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
