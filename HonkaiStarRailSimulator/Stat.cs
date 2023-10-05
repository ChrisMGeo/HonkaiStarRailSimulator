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
        if (maxStacks is Finity<uint>.Finite finiteMaxStacks)
        {
            uint currentStacks = 0;
            for (var i = 0; i < StatusEffects.Count; ++i)
            {
                var merged = statusEffect.MergeStacking(StatusEffects[i]).Match(
                    onSome: (someMerged) =>
                    {
                        StatusEffects[i] = someMerged;
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

            if (currentStacks >= finiteMaxStacks.Value) return false;
            StatusEffects.Add(statusEffect);
            return true;
        }

        StatusEffects.Add(statusEffect);
        return true;
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

    public static Finity<uint> GetMaxStacks(StatusEffectId id)
    {
        return id switch
        {
            StatusEffectId.WeaponStatsBuff => new Finity<uint>.Infinite(),
            StatusEffectId.LongevousDiscipleCritBuff => new Finity<uint>.Finite(2),
            _ => new Finity<uint>.Finite(1)
        };
    }

    private static Finity<uint> GetInitialDuration(StatusEffectId id)
    {
        return id switch
        {
            StatusEffectId.Benediction => new Finity<uint>.Finite(3),
            StatusEffectId.TheBelobogMarchCDmgBuff or StatusEffectId.TheBelobogMarchAtkBuff or
                StatusEffectId.AstaSpeedBuff or
                StatusEffectId.LongevousDiscipleCritBuff =>
                new Finity<uint>.Finite(2),
            _ => new Finity<uint>.Finite(1),
        };
    }

    private static StackingStatus GetInitialStacks(StatusEffectId id)
    {
        return id switch
        {
            StatusEffectId.LongevousDiscipleCritBuff => new StackingStatus.Joined(1),
            _ => new StackingStatus.Separate()
        };
    }

    public static StackingType GetStackingType(StatusEffectId id)
    {
        return GetInitialStacks(id) is StackingStatus.Joined ? StackingType.Joined : StackingType.Separate;
    }

    public StatusEffectId Id { get; init; }

    public Finity<uint> Durations { get; protected set; }

    public StackingStatus Stacking { get; protected set; }

    protected StatusEffect(StatusEffectId id)
    {
        Id = id;
        Durations = GetInitialDuration(id);
        Stacking = GetInitialStacks(id);
    }

    public bool Exhaust()
    {
        if (Durations is Finity<uint>.Finite finiteValue)
        {
            var newDuration = finiteValue.Value - 1;
            Durations = new Finity<uint>.Finite(newDuration);
            return newDuration <= 0;
        }
        else
        {
            return false;
        }
    }

    public IOption<StatusEffect> GetExhausted()
    {
        if (Durations is Finity<uint>.Finite finiteValue)
        {
            var newDuration = finiteValue.Value - 1;
            var res = this;
            res.Durations = new Finity<uint>.Finite(newDuration);
            return (newDuration <= 0 || newDuration > finiteValue.Value
                ? new None<StatusEffect>()
                : Some<StatusEffect>.Of(res));
        }
        else
        {
            return Some<StatusEffect>.Of(this);
        }
    }

    public abstract StatModifier GetModifiedValues();

    public IOption<StatusEffect> MergeStacking(StatusEffect s)
    {
        if (Stacking is not StackingStatus.Joined thisJoined || s.Stacking is not StackingStatus.Joined sJoined ||
            s.Id != Id) return new None<StatusEffect>();
        var maxStacks = GetMaxStacks(Id);
        uint newStacks;
        if (maxStacks is Finity<uint>.Finite finiteMaxStacks)
        {
            newStacks = uint.Min(thisJoined.Stacks + sJoined.Stacks, finiteMaxStacks.Value);
        }
        else
        {
            newStacks = thisJoined.Stacks + sJoined.Stacks;
        }

        var res = s;
        res.Durations = s.Durations.GetGreater(Durations);
        res.Stacking = new StackingStatus.Joined(newStacks);
        return Some<StatusEffect>.Of(res);
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
    WeaponStatsBuff,
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

public abstract record StackingStatus
{
    public record Separate : StackingStatus;

    public record Joined(uint Stacks) : StackingStatus;
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
        if (Stacking is StackingStatus.Joined joinedStacks)
        {
            return _value * joinedStacks.Stacks;
        }

        return _value;
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