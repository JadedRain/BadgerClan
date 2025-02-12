using BadgerClan.Logic;
using BadgerClan.Logic.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();
builder.Services.AddCodeFirstGrpc();
builder.Services.AddSingleton<MoveSetService>();

var app = builder.Build();
app.MapGrpcService<StrategySwapService>();




app.MapGet("/", () => "Sample BadgerClan bot.  Modify the code in Program.cs to change how the bot performs.");
app.MapPost("/", (GameState request, MoveSetService moveSetService) =>
{
    // Should work 
    int moveSet = moveSetService.MoveSet;

    var myMoves = new List<Move>();
    if (moveSet == 1)
    {
        myMoves = SuperSimpleExampleBot.MakeMoves(request);//Very simple bot example.  Delete this line when you write your own bot.
    }
    else if (moveSet == 2)
    {
        Console.WriteLine("Moveset");
        myMoves = ModifiedSimple.MakeMoves(request);
    }
    else if(moveSet == 3)
    {
        myMoves = KiteBack.MakeMoves(request);
    }
    return new MoveResponse(myMoves);
});

app.MapPost("/setmove/{Id}", (int Id, MoveSetService moveSetService) =>
{
   moveSetService.MoveSet = Id;
});

app.MapGet("/currentmove", (MoveSetService moveSetService) => moveSetService.MoveSet);

app.Run();

public record GameState(IEnumerable<Unit> Units, IEnumerable<int> TeamIds, int YourTeamId, int TurnNumber, string GameId, int BoardSize, int Medpacs, int NextMedpac);
public record Unit(string Type, int Id, int Attack, int AttackDistance, int Health, int MaxHealth, double Moves, double MaxMoves, Coordinate Location, int Team);

