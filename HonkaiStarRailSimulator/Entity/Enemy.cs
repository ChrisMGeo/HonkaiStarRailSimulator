﻿namespace HonkaiStarRailSimulator;

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

public enum EnemyId
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
        foreach (var (key, _) in DebuffRes)
        {
            DebuffRes[key].ExhaustStatusEffects();
        }
    }

    public EnemyId Id { get; }

    public Dictionary<DebuffType, Stat> DebuffRes { set; get; } = Enum.GetValues(typeof(DebuffType)).Cast<DebuffType>()
        .ToDictionary(d => d, _ => new Stat());

    public Enemy(EnemyId id, uint level) : base(GetEnemySpeed(id), GetEnemyMaxHp(id, level),
        GetEnemyAtk(id, level), 10 * level + 200)
    {
        Level = level;
        Id = id;
        switch (Level)
        {
            case >= 86:
                Speed.PercentageBonus += .32f;
                break;
            case >= 78:
                Speed.PercentageBonus += .2f;
                break;
            case >= 65:
                Speed.PercentageBonus += .1f;
                break;
        }
    }

    public static float GetEnemySpeed(EnemyId id)
    {
        // TODO: Implement
        return 100;
    }

    public static float GetEnemyMaxHp(EnemyId id, uint level)
    {
        // TODO: Implement
        return 1000;
    }

    public static float GetEnemyAtk(EnemyId id, uint level)
    {
        // TODO: Implement
        return 1000;
    }
}