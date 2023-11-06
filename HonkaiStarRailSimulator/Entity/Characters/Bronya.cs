namespace HonkaiStarRailSimulator.Characters;

public class Bronya : Character
{
    public IOption<MovableEntity> Target { get; set; } = new None<MovableEntity>();

    public override void Skill(params MovableEntity[] entities)
    {
        Target.Match(
            onSome: target =>
            {
                if (target!=this)
                    target.ActionAdvance(1);
                
            },
            onNone: () =>
            {
                if (entities.Length >= 1)
                {
                    if (entities[0]!=this)
                        entities[0].ActionAdvance(1);
                }
            }
        );
    }

    public Bronya(int level = 80) : base(CharacterId.Bronya, level)
    {
    }

    private void TalentEvent(object? sender, EventArgs args)
    {
        var advanceFactor = _getTalentScalings()[0];
        Console.WriteLine($"Bronya Talent LVL {TalentLevel}: AA by {advanceFactor}");
        ActionAdvance(advanceFactor);
        // TODO: ..., and increases their DMG by 49% for 1 turn(s).
        FinishTurnEvent -= TalentEvent;
    }

    public override void BasicAttack(params MovableEntity[] entities)
    {
        FinishTurnEvent += TalentEvent;
    }

    public override void Ultimate(params MovableEntity[] entities)
    {
        foreach (var entity in entities)
        {
            if (entity is not Character charEntity) continue;
            charEntity.Atk.AddStatusEffect(new ConstantStatusEffect(StatusEffectId.TheBelobogMarchAtkBuff,
                new StatModifier(percentageBonus: 0.55f)));
            charEntity.CritDamage.AddStatusEffect(new ConstantStatusEffect(StatusEffectId.TheBelobogMarchCDmgBuff,
                new StatModifier(flatBonus: .2f + CritDamage.GetFinalValue() * .16f)));
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