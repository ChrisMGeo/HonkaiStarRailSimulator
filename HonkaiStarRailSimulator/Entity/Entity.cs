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

    public class OnHitArgs : EventArgs
    {
    }

    public event HSREventHandler<Entity, OnHitArgs> OnHit; // Explicitly hit but not neccesarily damaged

    public class OnHurtArgs : EventArgs
    {
    }

    public event HSREventHandler<Entity, OnHurtArgs> OnHurt; // hurt whether by  attack, dot, etc.

    public Stat Atk { get; set; }
    public Stat Def { get; set; }

    public Stat EffectHitRate { get; set; } = new();
    public Stat EffectRes { get; set; } = new();

    public Dictionary<Element, Stat> ElementalResBoost { get; set; } = Enum.GetValues(typeof(Element)).Cast<Element>()
        .ToDictionary(d => d, _ => new Stat());

    public Dictionary<DamageResType, Stat> DamageResistances { get; set; } = Enum.GetValues(typeof(DamageResType))
        .Cast<DamageResType>()
        .ToDictionary(d => d, _ => new Stat());

    public Entity(float initialSpd, float maxHp, float atk, float def) : base(initialSpd)
    {
        MaxHp = new Stat(baseValue: maxHp);
        Hp = MaxHp.GetFinalValue();
        Atk = new Stat(baseValue: atk);
        Def = new Stat(baseValue: def);
        OnHit = (_, _, _) => { };
        OnHurt = (_, _, _) => { };
    }
}