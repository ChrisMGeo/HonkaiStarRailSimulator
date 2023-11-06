namespace HonkaiStarRailSimulator.Characters;

public class Tingyun : Character
{
    private IOption<Character> _target = new None<Character>();

    public IOption<Character> Target
    {
        get => _target;
        set
        {
            _prevTarget = _target;
            _target = value;
        }
    }

    private IOption<Character> _prevTarget = new None<Character>();

    private void RemovePreviousBenediction()
    {
        _prevTarget.Match(
            onSome: character =>
            {
                character.Atk.RemoveStatusEffectsById(StatusEffectId.Benediction);
            },
            onNone: () => { }
        );
    }

    public override void Skill(params MovableEntity[] entities)
    {
        var skillScalings = _getSkillScalings();
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
                    () => new StatModifier(percentageBonus: float.Min(skillScalings[1], Atk.GetFinalValue() * skillScalings[2]))));
                RemovePreviousBenediction();
                if (Ascension2.Active)
                {
                    Speed.AddStatusEffect(new ConstantStatusEffect(StatusEffectId.BenedictionSpdBuff, new StatModifier(percentageBonus: .2f)));
                }
            },
            onNone: () => { }
        );
        // TODO: When the ally with Benediction attacks, they will deal Additional Lightning DMG equal to 50% of that ally's ATK for 1 time.
    }

    public Tingyun(int level = 80) : base(CharacterId.Tingyun, level)
    {
    }

    public override void BasicAttack(params MovableEntity[] entities)
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
                BasicAttack();
                break;
        }

        FinishTurn();
    }
}