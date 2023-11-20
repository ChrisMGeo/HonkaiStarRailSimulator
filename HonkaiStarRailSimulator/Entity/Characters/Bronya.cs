namespace HonkaiStarRailSimulator.Characters;

public class Bronya : Character
{
    public IOption<MovableEntity> Target { get; set; } = new None<MovableEntity>();

    public override void Skill(params MovableEntity[] entities)
    {
        var skillScalings = _getSkillScalings();
        Target.Match(
            onSome: target =>
            {
                if (target!=this)
                    target.ActionAdvance(1);
                if (target is Character character)
                    character.DamageBonuses[DamageBonusType.All].AddStatusEffect(new ConstantStatusEffect(StatusEffectId.CombatRedeployment, new StatModifier(skillScalings[0])));
            },
            onNone: () =>
            {
                if (entities.Length >= 1)
                {
                    if (entities[0]!=this)
                        entities[0].ActionAdvance(1);
                    if (entities[0] is Character character)
                        character.DamageBonuses[DamageBonusType.All].AddStatusEffect(new ConstantStatusEffect(StatusEffectId.CombatRedeployment, new StatModifier(skillScalings[0])));
                }
            }
        );
    }

    public Bronya(int level = 80) : base(CharacterId.Bronya, level)
    {
    }

    private void TalentEvent(MovableEntity sender, IOption<TurnSystem> turnSystem, FinishTurnArgs args)
    {
        var advanceFactor = _getTalentScalings()[0];
        Console.WriteLine($"Bronya Talent LVL {TalentLevel}: AA by {advanceFactor}");
        ActionAdvance(advanceFactor);
        FinishTurnEvent -= TalentEvent;
    }

    public override void BasicAttack(params MovableEntity[] entities)
    {
        FinishTurnEvent += TalentEvent;
    }

    public override void Ultimate(params MovableEntity[] entities)
    {
        var ultimateScalings = _getUltimateScalings();
        foreach (var entity in entities)
        {
            if (entity is not Character charEntity) continue;
            charEntity.Atk.AddStatusEffect(new ConstantStatusEffect(StatusEffectId.TheBelobogMarchAtkBuff,
                new StatModifier(percentageBonus: ultimateScalings[0])));
            charEntity.CritDamage.AddStatusEffect(new ConstantStatusEffect(StatusEffectId.TheBelobogMarchCDmgBuff,
                new StatModifier(flatBonus: ultimateScalings[1] + CritDamage.GetFinalValue() * ultimateScalings[2])));
        }
    }

    public override void DoAction()
    {
        switch (Turns % 2)
        {
            case 1:
                Console.WriteLine("Bronya Skill");
                Skill();
                break;
            default:
                BasicAttack();
                break;
        }

        FinishTurn();
    }
}