namespace HonkaiStarRailSimulator;

public class Stat
{
    public float BaseValue { get; set; }
    public float PercentageBonus { get; set; }
    public float FlatBonus { get; set; }
    public List<StatusEffect> StatusEffects { get; private set; }

    public float GetFinalValue()
    {
        var res = new StatModifier(BaseValue, PercentageBonus, FlatBonus);

        res = StatusEffects.Aggregate(res, (current, statusEffect) => current + statusEffect.GetModifiedValues());
        return res.GetValue();
    }

    public Stat(float baseValue = 0.0f, float percentageBonus = 0.0f, float flatBonus = 0.0f)
    {
        BaseValue = baseValue;
        PercentageBonus = percentageBonus;
        FlatBonus = flatBonus;
        StatusEffects = new List<StatusEffect>();
    }

    public Stat(Stat toClone)
    {
        BaseValue = toClone.BaseValue;
        PercentageBonus = toClone.PercentageBonus;
        FlatBonus = toClone.FlatBonus;
        StatusEffects = new List<StatusEffect>(toClone.StatusEffects.Capacity);
        foreach (var item in toClone.StatusEffects)
        {
            StatusEffects.Add(item.Copy());
        }
    }

    public bool AddStatusEffect(StatusEffect statusEffect)
    {
        var maxStacks = StatusEffect.GetMaxStacks(statusEffect.Id);

        return maxStacks.Match(
            onFinite: (finiteMaxStacks) =>
            {
                uint currentStacks = 0;
                for (var i = 0; i < StatusEffects.Count; ++i)
                {
                    var i1 = i;
                    var merged = statusEffect.MergeStacking(StatusEffects[i]).Match(
                        onSome: (someMerged) =>
                        {
                            StatusEffects[i1] = someMerged;
                            return true;
                        },
                        onNone: () => false
                    );
                    if (merged) return true;
                    if (StatusEffects[i].Id == statusEffect.Id)
                    {
                        ++currentStacks;
                    }
                }

                if (currentStacks >= finiteMaxStacks) return false;
                StatusEffects.Add(statusEffect);
                return true;
            },
            onInfinite:
            () =>
            {
                StatusEffects.Add(statusEffect);
                return true;
            }
        );
    }

    public bool RemoveStatusEffect(StatusEffect se)
    {
        return StatusEffects.Remove(se);
    }

    public void ClearStatusEffects()
    {
        StatusEffects.Clear();
    }

    public int RemoveStatusEffectsById(StatusEffectId id)
    {
        return StatusEffects.RemoveAll(se => se.Id == id);
    }

    public void ExhaustStatusEffects()
    {
        var res = new List<StatusEffect>();
        foreach (var result in StatusEffects.Select(statusEffect => statusEffect.GetExhausted()))
        {
            result.Match(
                onSome: toAdd => { res.Add(toAdd); },
                onNone: () => { }
            );
        }

        StatusEffects = res;
    }
}

public abstract class StatusEffect
{
    public abstract StatusEffect Copy();

    public static StatusEffectType GetStatusEffectType(StatusEffectId id)
    {
        return id switch
        {
            _ => StatusEffectType.Buff
        };
    }

    public static IFinity<uint> GetMaxStacks(StatusEffectId id)
    {
        return id switch
        {
            StatusEffectId.PermanentBaseStatBuff or StatusEffectId.PermanentFlatStatBuff => new Infinite<uint>(),
            StatusEffectId.LongevousDiscipleCritBuff => Finite<uint>.Of(2),
            _ => Finite<uint>.Of(1)
        };
    }

    private static IFinity<uint> GetInitialDuration(StatusEffectId id)
    {
        return id switch
        {
            StatusEffectId.PermanentBaseStatBuff or StatusEffectId.PermanentFlatStatBuff => new Infinite<uint>(),
            StatusEffectId.Benediction => Finite<uint>.Of(3),
            StatusEffectId.TheBelobogMarchCDmgBuff or StatusEffectId.TheBelobogMarchAtkBuff or
                StatusEffectId.AstaSpeedBuff or
                StatusEffectId.LongevousDiscipleCritBuff =>
                Finite<uint>.Of(2),
            _ => Finite<uint>.Of(1),
        };
    }

    private static IStackingStatus GetInitialStacks(StatusEffectId id)
    {
        return id switch
        {
            StatusEffectId.LongevousDiscipleCritBuff => Joined.Of(1),
            _ => new Separate()
        };
    }

    public StatusEffectId Id { get; init; }

    public IFinity<uint> Durations { get; protected set; }

    public IStackingStatus Stacking { get; protected set; }

    protected StatusEffect(StatusEffectId id)
    {
        Id = id;
        Durations = GetInitialDuration(id);
        Stacking = GetInitialStacks(id);
    }

    public IOption<StatusEffect> GetExhausted()
    {
        return Durations.Match(
            onFinite: finiteValue =>
            {
                var newDuration = finiteValue - 1;
                var res = this;
                Durations = Finite<uint>.Of(newDuration);
                return (newDuration <= 0 || newDuration > finiteValue
                    ? new None<StatusEffect>()
                    : Some<StatusEffect>.Of(res));
            },
            onInfinite: () => Some<StatusEffect>.Of(this));
    }

    public abstract StatModifier GetModifiedValues();

    public IOption<StatusEffect> MergeStacking(StatusEffect s)
    {
        if (s.Id != Id) return new None<StatusEffect>();
        return Stacking.Match(
            onSeparate: () => new None<StatusEffect>(),
            onJoined: (thisJoined) =>
                s.Stacking.Match(
                    onSeparate: () => new None<StatusEffect>(),
                    onJoined: (sJoined) =>
                    {
                        var maxStacks = GetMaxStacks(Id);
                        var newStacks = maxStacks.Match(
                            onFinite: (finiteMaxStacks) => uint.Min(thisJoined + sJoined, finiteMaxStacks),
                            onInfinite: () => thisJoined + sJoined
                        );
                        s.Durations = s.Durations.GetGreater(Durations);
                        s.Stacking = Joined.Of(newStacks);
                        return Some<StatusEffect>.Of(s);
                    }
                )
        );
    }
}

public struct StatModifier
{
    public float BaseValue { get; set; }
    public float PercentageBonus { get; set; }
    public float FlatBonus { get; set; }

    public StatModifier(float baseValue = 0.0f, float percentageBonus = 0.0f, float flatBonus = 0.0f)
    {
        BaseValue = baseValue;
        PercentageBonus = percentageBonus;
        FlatBonus = flatBonus;
    }

    public float GetValue()
    {
        return BaseValue * (1.0f + PercentageBonus) + FlatBonus;
    }

    public static StatModifier operator +(StatModifier b, StatModifier c)
    {
        return new StatModifier(b.BaseValue + c.BaseValue, b.PercentageBonus + c.PercentageBonus,
            b.FlatBonus + c.FlatBonus);
    }

    public static StatModifier operator -(StatModifier b, StatModifier c)
    {
        return new StatModifier(b.BaseValue - c.BaseValue, b.PercentageBonus - c.PercentageBonus,
            b.FlatBonus - c.FlatBonus);
    }

    public static StatModifier operator *(StatModifier b, float m)
    {
        return new StatModifier(b.BaseValue * m, b.PercentageBonus * m,
            b.FlatBonus * m);
    }
}

public enum StatusEffectType
{
    Buff,
    Debuff
}

public enum StatusEffectId
{
    AstaSpeedBuff,
    MessengerTraversingHackerSpaceUltBuff,
    LongevousDiscipleCritBuff,
    PermanentBaseStatBuff,
    PermanentFlatStatBuff,
    TheBelobogMarchAtkBuff,
    TheBelobogMarchCDmgBuff,
    Benediction,
    BenedictionSpdBuff
}

public enum StackingType
{
    Separate,
    Joined
}

public interface IStackingStatus
{
    TResult Match<TResult>(Func<uint, TResult> onJoined, Func<TResult> onSeparate);
    void Match(Action<uint> onJoined, Action onSeparate);
    IOption<TResult> MapJoined<TResult>(Func<uint, TResult> f);
    IOption<TResult> MapSeparate<TResult>(Func<TResult> f);
}

public class Separate : IStackingStatus
{
    public TResult Match<TResult>(Func<uint, TResult> onJoined, Func<TResult> onSeparate) => onSeparate();

    public void Match(Action<uint> onJoined, Action onSeparate) => onSeparate();

    public IOption<TResult> MapJoined<TResult>(Func<uint, TResult> f) => new None<TResult>();

    public IOption<TResult> MapSeparate<TResult>(Func<TResult> f) => Some<TResult>.Of(f());
}

public class Joined : IStackingStatus
{
    private uint _stacks;

    private Joined(uint stacks)
    {
        _stacks = stacks;
    }

    public static IStackingStatus Of(uint data) => new Joined(data);

    public TResult Match<TResult>(Func<uint, TResult> onJoined, Func<TResult> onSeparate) => onJoined(_stacks);

    public void Match(Action<uint> onJoined, Action onSeparate) => onJoined(_stacks);

    public IOption<TResult> MapJoined<TResult>(Func<uint, TResult> f) => Some<TResult>.Of(f(_stacks));

    public IOption<TResult> MapSeparate<TResult>(Func<TResult> f) => new None<TResult>();
}

public class ConstantStatusEffect : StatusEffect
{
    public static ConstantStatusEffect LongevousDiscipleCritBuff()
    {
        return new ConstantStatusEffect(StatusEffectId.LongevousDiscipleCritBuff, new StatModifier(flatBonus: 0.08f));
    }

    private readonly StatModifier _value;

    public ConstantStatusEffect(StatusEffectId id, StatModifier value) : base(id)
    {
        _value = value;
    }

    public override StatusEffect Copy()
    {
        return new ConstantStatusEffect(Id, _value);
    }

    public override StatModifier GetModifiedValues()
    {
        return Stacking.Match(
            onSeparate: () => { return _value; },
            onJoined: (joinedStacks) => { return _value * joinedStacks; }
        );
    }
}

public class ConditionalStatusEffect : StatusEffect
{
    private readonly Func<StatModifier> _conditionalValue;

    public ConditionalStatusEffect(StatusEffectId id, Func<StatModifier> conditionalValue) : base(id)
    {
        _conditionalValue = conditionalValue;
    }

    public override StatusEffect Copy()
    {
        return new ConditionalStatusEffect(Id, _conditionalValue);
    }

    public override StatModifier GetModifiedValues()
    {
        return _conditionalValue();
    }
}