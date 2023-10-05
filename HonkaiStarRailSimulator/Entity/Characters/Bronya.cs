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


    public override void NormalAttack(params MovableEntity[] entities)
    {
        // Could technically override FinishTurn to ActionAdvance but need to check for if previous action was normal attack which is annoying
        void TalentEvent(object? sender, EventArgs args)
        {
            Console.WriteLine("Bronya Talent LVL 10");
            ActionAdvance(.3f);
            FinishTurnEvent -= TalentEvent;
        }

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
                NormalAttack();
                break;
        }

        FinishTurn();
    }
}