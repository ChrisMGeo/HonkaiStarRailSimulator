namespace HonkaiStarRailSimulator.Lightcones;

public class MomentOfVictory : Lightcone
{
    private readonly StatusEffect _defPercBoost;
    private readonly StatusEffect _ehrBoost;

    private void _defPercBoostOnHit(object? sender, EventArgs args)
    {
        _equippedCharacter.Match(
            onNone: () => { },
            onSome: (c) =>
            {
                c.Def.AddStatusEffect(new ConditionalStatusEffect(StatusEffectId.MomentOfVictoryDefBoostOnHit, () => new StatModifier(percentageBonus:.24f+(SuperImposition-1)*.04f)));
            }
        );
    }
    public MomentOfVictory(int level) : base(LightconeId.MomentOfVictory, level)
    {
        _defPercBoost = new ConditionalStatusEffect(StatusEffectId.PermanentStatBuff, () => new StatModifier(percentageBonus:.24f+(SuperImposition-1)*.04f));
        _ehrBoost = new ConditionalStatusEffect(StatusEffectId.PermanentStatBuff, () => new StatModifier(flatBonus:.24f+(SuperImposition-1)*.04f));
    }

    public override void AttachTo(Character c)
    {
        base.AttachTo(c);
        c.Def.AddStatusEffect(_defPercBoost);
        c.EffectHitRate.AddStatusEffect(_ehrBoost);
        c.OnHit += _defPercBoostOnHit;
    }

    public override void DetachFrom(Character c)
    {
        base.DetachFrom(c);
        c.Def.RemoveStatusEffect(_defPercBoost);
        c.EffectHitRate.RemoveStatusEffect(_ehrBoost);
        c.OnHit -= _defPercBoostOnHit;
        c.Def.RemoveStatusEffectsById(StatusEffectId.MomentOfVictoryDefBoostOnHit);
    }
}