namespace HonkaiStarRailSimulator;

public enum Element
{
    Physical,
    Fire,
    Ice,
    Lightning,
    Wind,
    Quantum,
    Imaginary,
}

public abstract class Entity : MovableEntity
{
    public override void FinishTurn()
    {
        base.FinishTurn();
        MaxHp.ExhaustStatusEffects();
        Atk.ExhaustStatusEffects();
        Def.ExhaustStatusEffects();
        EffectHitRate.ExhaustStatusEffects();
        EffectRes.ExhaustStatusEffects();
        foreach (var (key, _) in ElementalResBoost)
        {
            ElementalResBoost[key].ExhaustStatusEffects();
        }
    }

    // public uint Level { get; protected set; }
    public Stat MaxHp { get; set; }
    public float Hp { get; set; }
    public bool IsDead => Hp <= 0.0f;

    public Stat Atk { get; set; }
    public Stat Def { get; set; }

    public Stat EffectHitRate { get; set; } = new();
    public Stat EffectRes { get; set; } = new();

    public Dictionary<Element, Stat> ElementalResBoost { get; set; } = new()
    {
        { Element.Fire, new Stat() },
        { Element.Quantum, new Stat() },
        { Element.Ice, new Stat() },
        { Element.Imaginary, new Stat() },
        { Element.Lightning, new Stat() },
        { Element.Physical, new Stat() },
        { Element.Wind, new Stat() }
    };

    public Entity(float initialSpd, float maxHp, float atk, float def) : base(initialSpd)
    {
        MaxHp = new Stat(baseValue: maxHp);
        Hp = MaxHp.GetFinalValue();
        Atk = new Stat(baseValue: atk);
        Def = new Stat(baseValue: def);
    }
}