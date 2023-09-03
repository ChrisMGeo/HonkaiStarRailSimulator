namespace MainApp;

public enum Element
{
    Physical,
    Fire,
    Ice,
    Lightning,
    Wind,
    Quantum,
    Imaginary,
}

public class MovableEntity
{
    public delegate void FinishTurnDelegate();
    public event FinishTurnDelegate finishTurnEvent;
    
    private Stat speed;

    public Stat Speed
    {
        get => speed;
        set
        {
            var avOld = ActionValue;
            var spdOld = this.speed.GetFinalValue();
            this.speed = value;
            ActionValue = avOld * spdOld / speed.GetFinalValue();
        }
    }

    public int Turns { get; protected set; } = 0;

    public float ActionValue { get; set; }

    public void ModifySpeed(StatusEffect statusEffect)
    {
        var avOld = ActionValue;
        var spdOld = speed.GetFinalValue();
        speed.AddStatusEffect(statusEffect);
        ActionValue = avOld * spdOld / speed.GetFinalValue();
    }

    public void ActionAdvance(float advancementPercentage)
    {
        ActionValue -= BaseActionValue * advancementPercentage;
        ActionValue = float.Max(ActionValue, 0);
    }

    public float BaseActionValue => CalcBaseActionValue(speed.GetFinalValue());

    public static float CalcBaseActionValue(float spd)
    {
        return 10000 / spd;
    }

    public MovableEntity(float initialSpd)
    {
        this.speed = new Stat(baseValue: initialSpd);
        this.ActionValue = this.BaseActionValue;
    }

    public virtual void DoAction()
    {
        // Do Nothing
        FinishTurn();
    }

    public virtual void FinishTurn()
    {
        Turns += 1;
        ActionValue = BaseActionValue;
        var spdOld = Speed.GetFinalValue();
        Speed.ExhaustStatusEffects();
        var spdNew = Speed.GetFinalValue();
        var avOld = ActionValue;
        ActionValue = avOld * spdOld / spdNew;
        finishTurnEvent?.Invoke();
    }

    public override string ToString()
    {
        return "Unnamed";
    }
}

public class Entity : MovableEntity
{
    public override void FinishTurn()
    {
        base.FinishTurn();
        MaxHp.ExhaustStatusEffects();
        Atk.ExhaustStatusEffects();
        Def.ExhaustStatusEffects();
        EffectHitRate.ExhaustStatusEffects();
        EffectRes.ExhaustStatusEffects();
        foreach (var (key, value) in ElementalResBoost)
        {
            ElementalResBoost[key].ExhaustStatusEffects();
        }
    }
    public uint Level { get; protected set; }
    public Stat MaxHp { get; set; }
    public float Hp { get; set; }
    public bool IsDead => Hp <= 0.0f;

    public Stat Atk { get; set; }
    public Stat Def { get; set; }

    public Stat EffectHitRate { get; set; } = new Stat();
    public Stat EffectRes { get; set; } = new Stat();

    public Dictionary<Element, Stat> ElementalResBoost { get; set; } = new Dictionary<Element, Stat>()
    {
        { Element.Fire, new Stat() },
        { Element.Quantum, new Stat() },
        { Element.Ice, new Stat() },
        { Element.Imaginary, new Stat() },
        { Element.Lightning, new Stat() },
        { Element.Physical, new Stat() },
        { Element.Wind, new Stat() }
    };

    public Entity(uint level, float initialSpd, float maxHp, float atk, float def) : base(initialSpd)
    {
        Level = level;
        MaxHp = new Stat(baseValue: maxHp);
        Hp = MaxHp.GetFinalValue();
        Atk = new Stat(baseValue: atk);
        Def = new Stat(baseValue: def);
    }
}

public enum DebuffType
{
    Bleed,
    Burn,
    Frozen,
    Shock,
    WindSheer,
    Entanglement,
    Imprisonment,
    Control // overrides Frozen, Entanglement and Imprisonment RES if greater than them respectively
}

public enum EnemyID
{
    AbundanceLotus1,
    AbundanceLotus3,
    AbundanceSpriteGoldenHound,
    AbundanceSpriteWoodenLupus,
    Antibaryon,
    AutomatonBeetle,
    AutomatonHound,
    AutomatonSpider,
    AuxiliaryRobotArmUnit,
    Baryon,
    CloudKnightsPatroller
}

public class Enemy : Entity
{
    public override void FinishTurn()
    {
        base.FinishTurn();
        foreach (var (key, value) in DebuffRes)
        {
            DebuffRes[key].ExhaustStatusEffects();
        }
    }

    public EnemyID Id { get; }

    public Dictionary<DebuffType, Stat> DebuffRes { set; get; } = new Dictionary<DebuffType, Stat>()
    {
        { DebuffType.Bleed, new Stat() },
        { DebuffType.Burn, new Stat() },
        { DebuffType.Control, new Stat() },
        { DebuffType.Entanglement, new Stat() },
        { DebuffType.Frozen, new Stat() },
        { DebuffType.Imprisonment, new Stat() },
        { DebuffType.Shock, new Stat() },
        { DebuffType.WindSheer, new Stat() },
    };

    public Enemy(EnemyID id, uint level) : base(level, GetEnemySpeed(id), GetEnemyMaxHp(id, level),
        GetEnemyAtk(id, level), 10 * level + 200)
    {
        Level = level;
        Id = id;
    }

    public static float GetEnemySpeed(EnemyID id)
    {
        // TODO: Implement
        return 100;
    }

    public static float GetEnemyMaxHp(EnemyID id, uint level)
    {
        // TODO: Implement
        return 1000;
    }

    public static float GetEnemyAtk(EnemyID id, uint level)
    {
        // TODO: Implement
        return 1000;
    }
}

public class Character : Entity
{
    public override void FinishTurn()
    {
        base.FinishTurn();
        CritRate.ExhaustStatusEffects();
        CritDamage.ExhaustStatusEffects();
        BreakEffect.ExhaustStatusEffects();
        OutgoingHealingBoost.ExhaustStatusEffects();
        EnergyRegenerationRate.ExhaustStatusEffects();
        foreach (var (key, value) in DamageBoost)
        {
            DamageBoost[key].ExhaustStatusEffects();
        }
    }

    public CharacterID Id { get; }
    public Stat CritRate { get; set; } = new Stat(baseValue: .05f);
    public Stat CritDamage { get; set; } = new Stat(baseValue: .5f);
    public Stat BreakEffect { get; set; } = new Stat();
    public Stat OutgoingHealingBoost { get; set; } = new Stat();
    public float MaxEnergy { get; set; }
    public Stat EnergyRegenerationRate { get; set; } = new Stat();

    public Dictionary<Element, Stat> DamageBoost { get; set; } = new Dictionary<Element, Stat>()
    {
        { Element.Fire, new Stat() },
        { Element.Quantum, new Stat() },
        { Element.Ice, new Stat() },
        { Element.Imaginary, new Stat() },
        { Element.Lightning, new Stat() },
        { Element.Physical, new Stat() },
        { Element.Wind, new Stat() }
    };

    public Character(CharacterID id, uint level) : base(level, GetCharacterSpeed(id), GetCharacterMaxHp(id, level),
        GetCharacterAtk(id, level), GetCharacterDef(id, level))
    {
        Id = id;
        Level = level;
        MaxEnergy = GetCharacterMaxEnergy(id);
    }

    public static float GetCharacterSpeed(CharacterID id)
    {
        switch (id)
        {
            case CharacterID.Bronya:
                return 99;
            case CharacterID.Blade:
                return 97;
            default:
                throw new ArgumentOutOfRangeException(nameof(id), id, null);
        }
    }

    public static float GetCharacterMaxEnergy(CharacterID id)
    {
        // TODO: Implement
        return 100;
    }

    public static float GetCharacterMaxHp(CharacterID id, uint level)
    {
        // TODO: Implement
        return 1000;
    }

    public static float GetCharacterAtk(CharacterID id, uint level)
    {
        // TODO: Implement
        return 1000;
    }

    public static float GetCharacterDef(CharacterID id, uint level)
    {
        // TODO: Implement
        return 1000;
    }

    // TODO: Implement abstract record or simple enum to denote if NA/Skill/Ult ends turn (they return this type)
    public virtual void NormalAttack(params MovableEntity[] entities)
    {
    }

    public virtual void Skill(params MovableEntity[] entities)
    {
    }

    public virtual void Ultimate(params MovableEntity[] entities)
    {
    }
}

public enum CharacterID
{
    Bronya,
    Blade,
    Tingyun
}

public class Bronya : Character
{
    public Option<MovableEntity> Target { get; set; } = new Option<MovableEntity>.None();

    public override void Skill(params MovableEntity[] entities)
    {
        if (entities.Length >= 1 && Target is Option<MovableEntity>.None)
        {
            entities[0].ActionAdvance(1);
        }
        else if (Target is Option<MovableEntity>.Some validTarget)
        {
            validTarget.Value.ActionAdvance(1);
        }
    }

    public Bronya(uint level = 80) : base(CharacterID.Bronya, level)
    {
    }

    public override string ToString()
    {
        return "Bronya";
    }

    private void TalentEvent()
    {
        Console.WriteLine("Bronya Talent LVL 10");
        ActionAdvance(.3f);
        finishTurnEvent -= TalentEvent;
    }

    public override void NormalAttack(params MovableEntity[] entities)
    {
        // Could technically override FinishTurn to ActionAdvance but need to check for if previous action was normal attack which is annoying
        // AddObserver(new BasicObserver<EntityEvent>(e =>
        // {
        //     switch (e)
        //     {
        //         case EntityEvent.FinishTurn:
        //             Console.WriteLine("Bronya Talent LVL 10");
        //             ActionAdvance(.3f);
        //             return false;
        //         default:
        //             return true;
        //     }
        // }));
        finishTurnEvent += TalentEvent;
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

public class Tingyun : Character
{
    private Option<Character> target = new Option<Character>.None();

    public Option<Character> Target
    {
        get { return target; }
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
        if (entities.Length >= 1 && Target is Option<Character>.None && entities[0] is Character character)
        {
            character.Atk.AddStatusEffect(new ConstantStatusEffect(StatusEffectId.Benediction,
                new StatModifier(percentageBonus: float.Max(0.5f, Atk.GetFinalValue() * .25f))));
            // Remove Benediction on previous allies
            RemovePreviousBenediction();
            // 20% SPD buff if A2 Ascension Passive is active
            // TODO: Traces and Ascension Passive System (Tree type or just named since there are only three ascension passives and 6 other traces)
            // if (Ascension2.Active) {
                Speed.AddStatusEffect(
                    new ConstantStatusEffect(StatusEffectId.BenedictionSpdBuff, new StatModifier(percentageBonus: .2f)));
            // }
        }
        else if (Target is Option<Character>.Some validTarget)
        {
            validTarget.Value.Atk.AddStatusEffect(new ConstantStatusEffect(StatusEffectId.Benediction,
                new StatModifier(percentageBonus: float.Max(0.5f, Atk.GetFinalValue() * .25f))));
            // TODO: Get rid of repetition here
            RemovePreviousBenediction();
        }
    }

    public Tingyun(uint level = 80) : base(CharacterID.Tingyun, level)
    {
    }

    public override string ToString()
    {
        return "Tingyun";
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

public class Blade : Character
{
    public Blade(uint level = 80) : base(CharacterID.Blade, level)
    {
    }

    public override string ToString()
    {
        return "Blade";
    }
}