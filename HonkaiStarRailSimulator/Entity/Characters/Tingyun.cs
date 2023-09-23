namespace HonkaiStarRailSimulator.Characters;

public class Tingyun : Character
{
    private Option<Character> target = new Option<Character>.None();

    public Option<Character> Target
    {
        get => target;
        set
        {
            prevTarget = target;
            target = value;
        }
    }

    private Option<Character> prevTarget = new Option<Character>.None();

    private void RemovePreviousBenediction()
    {
        if (prevTarget is Option<Character>.Some somePrevCharacter)
        {
            somePrevCharacter.Value.Atk.RemoveStatusEffectsById(StatusEffectId.Benediction);
        }
    }

    public override void Skill(params MovableEntity[] entities)
    {
        Option<Character> finalTarget = new Option<Character>.None();
        if (entities.Length >= 1 && Target is Option<Character>.None && entities[0] is Character character)
        {
            finalTarget = new Option<Character>.Some(character);
        }
        else if (Target is Option<Character>.Some validTarget)
        {
            finalTarget = validTarget;
        }

        if (finalTarget is Option<Character>.Some validFinalTarget)
        {
            validFinalTarget.Value.Atk.AddStatusEffect(new ConditionalStatusEffect(StatusEffectId.Benediction,
                () => new StatModifier(percentageBonus: float.Min(0.5f, Atk.GetFinalValue() * .25f))));
            RemovePreviousBenediction();
            // 20% SPD buff if A2 Ascension Passive is active
            // TODO: Traces and Ascension Passive System (Tree type or just named since there are only three ascension passives and 6 other traces)
            if (Ascension2.Active)
            {
                Speed.AddStatusEffect(
                    new ConstantStatusEffect(StatusEffectId.BenedictionSpdBuff,
                        new StatModifier(percentageBonus: .2f)));
            }
        }
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