namespace HonkaiStarRailSimulator;

public enum DebuffType
{
    Bleed,
    Burn,
    Frozen,
    Shock,
    WindSheer,
    Entanglement,
    Imprisonment,
    Control // overrides Frozen, Entanglement and Imprisonment RES if greater than them respectively
}

public enum EnemyID
{
    AbundanceLotus1,
    AbundanceLotus3,
    AbundanceSpriteGoldenHound,
    AbundanceSpriteWoodenLupus,
    Antibaryon,
    AutomatonBeetle,
    AutomatonHound,
    AutomatonSpider,
    AuxiliaryRobotArmUnit,
    Baryon,
    CloudKnightsPatroller
}

public class Enemy : Entity
{
    public uint Level { get; init; }

    public override void FinishTurn()
    {
        base.FinishTurn();
        foreach (var (key, value) in DebuffRes)
        {
            DebuffRes[key].ExhaustStatusEffects();
        }
    }

    public EnemyID Id { get; }

    public Dictionary<DebuffType, Stat> DebuffRes { set; get; } = new Dictionary<DebuffType, Stat>()
    {
        { DebuffType.Bleed, new Stat() },
        { DebuffType.Burn, new Stat() },
        { DebuffType.Control, new Stat() },
        { DebuffType.Entanglement, new Stat() },
        { DebuffType.Frozen, new Stat() },
        { DebuffType.Imprisonment, new Stat() },
        { DebuffType.Shock, new Stat() },
        { DebuffType.WindSheer, new Stat() },
    };

    public Enemy(EnemyID id, uint level) : base(GetEnemySpeed(id), GetEnemyMaxHp(id, level),
        GetEnemyAtk(id, level), 10 * level + 200)
    {
        Level = level;
        Id = id;
    }

    public static float GetEnemySpeed(EnemyID id)
    {
        // TODO: Implement
        return 100;
    }

    public static float GetEnemyMaxHp(EnemyID id, uint level)
    {
        // TODO: Implement
        return 1000;
    }

    public static float GetEnemyAtk(EnemyID id, uint level)
    {
        // TODO: Implement
        return 1000;
    }
}