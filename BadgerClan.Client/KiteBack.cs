using Microsoft.Extensions.Logging;
using BadgerClan.Logic;
public class KiteBack
{
    public static List<Move> MakeMoves(GameState request)
    {
        var myTeamId = request.YourTeamId;
        var myUnits = findMyUnits(myTeamId, request.Units);
        var enemies = findEnemies(myTeamId, request.Units);
        var moves = new List<Move>();

        foreach (var unit in myUnits)
        {
            var closest = findClosest(unit, enemies);
            var distanceFromClosest = unit.Location.Distance(closest.Location);

            if (unit.Type == "Archer")
            {
                if (distanceFromClosest > 2)
                {
                    Console.WriteLine("Archer: Searching for enemy");
                    moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(closest.Location)));
                    moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(closest.Location)));
                }
                else if (distanceFromClosest > 1 && distanceFromClosest < unit.AttackDistance)
                {
                    Console.WriteLine("Archer: Attacking and kite");
                    moves.Add(new Move(MoveType.Attack, unit.Id, closest.Location));
                    moves.Add(new Move(MoveType.Attack, unit.Id, closest.Location));
                    moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Away(closest.Location)));
                }
                else if (distanceFromClosest == 1)
                {
                    Console.WriteLine("Archer: Kiting backwards");
                    moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Away(closest.Location)));
                    moves.Add(new Move(MoveType.Attack, unit.Id, closest.Location));
                    moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Away(closest.Location)));
                }
            }
            else
            {
                var archers = findArchers(myTeamId, myUnits);
                var closestArcher = unit;
                if (archers.Count > 0)
                {
                    closestArcher = findClosest(unit, archers);
                }
                if (distanceFromClosest > 2)
                {
                    Console.WriteLine("Knight: Searching for enemy");
                    moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(closest.Location)));
                    moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(closest.Location)));
                }
                else if (distanceFromClosest == unit.AttackDistance)
                {
                    Console.WriteLine("Knight: Attacking");
                    moves.Add(new Move(MoveType.Attack, unit.Id, closest.Location));
                    moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(closest.Location)));
                }
            }
        }

        return moves;
    }

    private static List<Unit> findMyUnits(int myTeamId, IEnumerable<Unit> units)
    {
        var myUnits = new List<Unit>();
        foreach (var unit in units)
        {
            if (unit.Team == myTeamId)
            {
                myUnits.Add(unit);
            }
        }

        return myUnits;
    }

    private static List<Unit> findEnemies(int myTeamId, IEnumerable<Unit> units)
    {
        var enemies = new List<Unit>();
        foreach (var unit in units)
        {
            if (unit.Team != myTeamId)
            {
                enemies.Add(unit);
            }
        }

        return enemies;
    }

    private static Unit findClosest(Unit unit, List<Unit> enemies)
    {
        var closest = enemies[0];
        foreach (var enemy in enemies)
        {
            if (enemy.Location.Distance(unit.Location) < closest.Location.Distance(unit.Location))
            {
                closest = enemy;
            }
        }

        return closest;
    }

    private static List<Unit> findArchers(int myTeamId, List<Unit> units) 
    {
        var archers = new List<Unit>();
        foreach (var unit in units)
        {
            if (unit.Type == "Archer")
            {
                archers.Add(unit);
            }
        }
        return archers;
    }

}
