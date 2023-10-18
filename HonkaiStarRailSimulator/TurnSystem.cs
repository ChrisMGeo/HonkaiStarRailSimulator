namespace HonkaiStarRailSimulator;

public class TurnSystem
{
    public List<MovableEntity> Entities = new();
    public float TotalAv { get; set; }
    public int Cycle => TotalAv < 150 ? 0 : 1 + (int)(TotalAv - 150) / 100;
    public float NextCycleAv => 150 + Cycle * 100;
    public IOption<MovableEntity> CurrentEntity { get; private set; } = new None<MovableEntity>();

    public TurnSystem()
    {
    }

    public void Display()
    {
        var cloned = new List<MovableEntity>(Entities);
        cloned.Sort((a, b) => a.ActionValue.CompareTo(b.ActionValue));
        Console.WriteLine($"Cycle: {Cycle} Total AV: {TotalAv}");
        foreach (var t in cloned)
        {
            Console.WriteLine($"{t} > {t.ActionValue}({t.Speed.GetFinalValue()})");
        }

        Console.WriteLine("--------------------------------------------------------");
    }

    public MovableEntity AddEntity(MovableEntity entity)
    {
        Entities.Add(entity);
        return entity;
    }

    public float MoveToNextTurn()
    {
        if (Entities.Count == 0)
        {
            CurrentEntity = new None<MovableEntity>();
            return 0.0f;
        }

        var nextEntity = Entities[0];
        var minAv = Entities[0].ActionValue;
        foreach (var entity in Entities.Where(entity => entity.ActionValue < minAv))
        {
            minAv = entity.ActionValue;
            nextEntity = entity;
        }

        foreach (var t in Entities)
        {
            t.ActionValue -= minAv;
        }

        CurrentEntity = Some<MovableEntity>.Of(nextEntity);
        TotalAv += minAv;
        return minAv;
    }

    private float NextTurnValue()
    {
        if (Entities.Count == 0)
        {
            return 0.0f;
        }

        var nextEntity = Entities[0];
        var minAv = Entities[0].ActionValue;
        foreach (var entity in Entities.Where(entity => entity.ActionValue < minAv))
        {
            minAv = entity.ActionValue;
            nextEntity = entity;
        }

        return minAv;
    }

    private void DoAction()
    {
        CurrentEntity.Match(
            onSome: (entity) => {entity.DoAction();},
            onNone: () => {}
        );
    }

    public void RunCycle()
    {
        var nextCycleAv = NextCycleAv;
        do
        {
            Display();
            if (TotalAv + NextTurnValue() >= nextCycleAv)
            {
                var diff = nextCycleAv - TotalAv;
                foreach (var t in Entities)
                {
                    t.ActionValue -= diff;
                }

                TotalAv = nextCycleAv;
                break;
            }

            MoveToNextTurn();
            Console.WriteLine("MOVED TO NEXT TURN");
            Display();
            DoAction();
        } while (TotalAv < nextCycleAv);
    }
}