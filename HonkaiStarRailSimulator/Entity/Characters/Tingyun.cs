namespace HonkaiStarRailSimulator.Characters;

public class Tingyun : Character
{
    private IOption<Character> target = new None<Character>();

    public IOption<Character> Target
    {
        get => target;
        set
        {
            prevTarget = target;
            target = value;
        }
    }

    private IOption<Character> prevTarget = new None<Character>();

    private void RemovePreviousBenediction()
    {
        prevTarget.Match(
            onSome: character =>
            {
                character.Atk.RemoveStatusEffectsById(StatusEffectId.Benediction);
            },
            onNone: () => { }
        );
    }

    public override void Skill(params MovableEntity[] entities)
    {
        IOption<Character> finalTarget = Target.Match(
            onSome: character => Some<Character>.Of(character),
            onNone: () =>
            {
                if (entities.Length >= 1 && entities[0] is Character character)
                {
                    return Some<Character>.Of(character);
                }
                return new None<Character>();
            });
        finalTarget.Match(
            onSome: character =>
            {
                character.Atk.AddStatusEffect(new ConditionalStatusEffect(StatusEffectId.Benediction,
                    () => new StatModifier(percentageBonus: float.Min(0.5f, Atk.GetFinalValue() * .25f))));
                RemovePreviousBenediction();
                if (Ascension2.Active)
                {
                    Speed.AddStatusEffect(new ConstantStatusEffect(StatusEffectId.BenedictionSpdBuff, new StatModifier(percentageBonus: .2f)));
                }
            },
            onNone: () => { }
        );
    }

    public Tingyun(int level = 80) : base(CharacterId.Tingyun, level)
    {
    }

    public override void NormalAttack(params MovableEntity[] entities)
    {
    }

    public override void Ultimate(params MovableEntity[] entities)
    {
        // TODO: Implement DMG Bonuses/Resistances using Enums or Bit flags and apply that for other things
    }

    public override void DoAction()
    {
        // Turn cycle depends on how fast Tingyun is in relation to Target. But let's assume same speed breakpoint, but DPS acts after.
        switch (Turns % 3)
        {
            case 0:
                Console.WriteLine("Tingyun Skill");
                Skill();
                break;
            default:
                NormalAttack();
                break;
        }

        FinishTurn();
    }
}