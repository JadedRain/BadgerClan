using BadgerClan.Logic;

public class ModifiedSimple
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

            if (closest.Location.Distance(unit.Location) <= unit.AttackDistance)
            {
                moves.Add(new Move(MoveType.Attack, unit.Id, closest.Location));
                moves.Add(new Move(MoveType.Attack, unit.Id, closest.Location));
            }
            else if (request.Medpacs > 0 && unit.Health < unit.MaxHealth)
            {
                moves.Add(new Move(MoveType.Medpac, unit.Id, unit.Location));
            }
            else
            {
                moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Away(closest.Location)));
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
}