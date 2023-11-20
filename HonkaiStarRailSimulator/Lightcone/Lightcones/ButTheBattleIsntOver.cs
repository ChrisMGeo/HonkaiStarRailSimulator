namespace HonkaiStarRailSimulator.Lightcones;

public class ButTheBattleIsntOver : Lightcone
{
    private void _dmgBoostAfterSkill(Character sender, IOption<TurnSystem> ts, EventArgs args)
    {
        ts.Match(
            onSome: someTurnSystem =>
            {
                someTurnSystem.GetNext<Character>().Match(
                    onSome: someNextCharacter =>
                    {
                        someNextCharacter.DamageBonuses[DamageBonusType.All].AddStatusEffect(new ConditionalStatusEffect(StatusEffectId.ButTheBattleIsntOverDmgBuff,
                            () => new StatModifier((SuperImposition-1)*.05f+.30f)));
                    },
                    onNone: () => { }
                );
            },
            onNone: () => { }
        );
    }

    private readonly StatusEffect _errBoost;
    public ButTheBattleIsntOver(int level = 80) : base(LightconeId.ButTheBattleIsnTOver, level)
    {
        _errBoost = new ConditionalStatusEffect(StatusEffectId.PermanentStatBuff, () => new StatModifier(flatBonus:.10f+(SuperImposition-1)*.02f));
    }

    public override void AttachTo(Character c)
    {
        base.AttachTo(c);
        c.EnergyRegenerationRate.AddStatusEffect(_errBoost);
        c.OnSkill += _dmgBoostAfterSkill;
    }

    public override void DetachFrom(Character c)
    {
        base.DetachFrom(c);
        c.EnergyRegenerationRate.RemoveStatusEffect(_errBoost);
        c.OnSkill -= _dmgBoostAfterSkill;
    }
}