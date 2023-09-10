namespace HonkaiStarRailSimulator;

// public abstract record BetterCharacterLevel
// {
//     public record Ascended(uint Level) : BetterCharacterLevel;
//
//     public record Unascended(uint Level) : BetterCharacterLevel;
//
//     public uint AscensionLevel()
//     {
//         return this switch
//         {
//             Ascended ascended => (uint)double.Ceiling(((int)ascended.Level - 20) / 10.0 + 1),
//             Unascended unascended => (uint)double.Ceiling(((int)unascended.Level - 20) / 10.0 + 1) + 1,
//             _ => throw new ArgumentOutOfRangeException()
//         };
//     }
//     public BetterCharacterLevel(uint level, bool ascended = false) {}
//
//     public BetterCharacterLevel()
//     {
//     }
// }

public class CharacterLevel
{
    public uint Level { get; private set; }
    public uint MaxLevel => 20 + (AscensionLevel - 1) * 10;

    public uint AscensionLevel
    {
        get;
        private set;
    }

    public CharacterLevel(uint level, bool ascended=false)
    {
        level = uint.Max(uint.Min(level, 80), 0);
        Level = level;
        AscensionLevel = (uint)double.Ceiling(((int)level - 20) / 10.0 + 1);
        if (level == MaxLevel && ascended)
        {
            AscensionLevel = uint.Min(AscensionLevel+1, 7);
        }
    }
}

public class Trace
{
    private bool active = false;
    public bool Active
    {
        get => preRequisites() && active;
        set
        {
            var prevActive = Active;
            active = value;
            if (Active != prevActive)
                if (Active)
                    onActivation();
                else
                    onDeactivation();
            if (!Active)
                active = false;

        }
    }

    private Func<bool> preRequisites;
    private Action onActivation;
    private Action onDeactivation;

    public Trace(bool active = false, Func<bool>? preRequisites = null, Action? onActivation= null, Action? onDeactivation=
        null)
    {
        preRequisites ??= () => true;
        onActivation ??= () => { };
        onDeactivation ??= () => { };
        this.active = active;
        this.preRequisites = preRequisites;
        this.onActivation = onActivation;
        this.onDeactivation = onDeactivation;
    }
}

public enum CharacterPath
{
    Destruction,
    Hunt,
    Erudition,
    Harmony,
    Nihility,
    Preservation,
    Abundance
}


public enum CharacterID
{
    Bronya,
    Blade,
    Tingyun
}

public abstract class Character : Entity
{
    public CharacterLevel CharacterLevel { get; init; }
    private int eidolon = 0;

    public int Eidolon
    {
        get
        {
            return eidolon;
        }
        set
        {
            var prevEidolon = eidolon;
            var newEidolon = int.Max(int.Min(6, value), 0);
            eidolon = newEidolon;
            for (var e = prevEidolon; e != newEidolon; e += int.Sign(newEidolon - prevEidolon))
            {
                // Disable/Enable Eidolon
            }
        }
    }

    public event EventHandler onSkill; 
    public event EventHandler onNormalAttack;
    public event EventHandler onUltimate;

    public Trace Ascension2 { get; set; }
    public Trace Ascension4 { get; set; }
    public Trace Ascension6 { get; set; }
    public Trace StatBoost1 { get; set; }
    public Trace StatBoost2 { get; set; }
    public Trace StatBoost3 { get; set; }
    public Trace StatBoost4 { get; set; }
    public Trace StatBoost5 { get; set; }
    public Trace StatBoost6 { get; set; }
    public Trace StatBoost7 { get; set; }
    public Trace StatBoost8 { get; set; }
    public Trace StatBoost9 { get; set; }
    public Trace StatBoost10 { get; set; }

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
    public CharacterPath Path { get; }
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

    protected Character(CharacterID id, uint level) : base(GetCharacterSpeed(id), GetCharacterMaxHp(id, uint.Max(uint.Min(level, 80),0)),
        GetCharacterAtk(id, uint.Max(uint.Min(level, 80),0)), GetCharacterDef(id, uint.Max(uint.Min(level, 80),0)))
    {
        
        CharacterLevel = new CharacterLevel(level);
        Id = id;
        MaxEnergy = GetCharacterMaxEnergy(id);
        Path = GetCharacterPath(id);
        onSkill = (sender, args) => { };
        onNormalAttack = (sender, args) => { };
        onUltimate = (sender, args) => { };
        switch (Path)
        {
            case CharacterPath.Destruction:
                StatBoost1 = new Trace();
                
                Ascension2 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=2);
                StatBoost2 = new Trace(preRequisites:() => this.Ascension2.Active);
                StatBoost3 = new Trace(preRequisites: () => this.StatBoost2.Active && this.CharacterLevel.AscensionLevel>=3);
                StatBoost4 = new Trace(preRequisites: () => this.StatBoost3.Active);

                Ascension4 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=4);
                StatBoost5 = new Trace(preRequisites: () => this.Ascension4.Active);
                StatBoost6 = new Trace(preRequisites: () => this.StatBoost5.Active && this.CharacterLevel.AscensionLevel>=5);
                StatBoost7 = new Trace(preRequisites: () => this.StatBoost6.Active);

                Ascension6 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=6);
                StatBoost8 = new Trace(preRequisites: () => this.Ascension6.Active);
                StatBoost9 = new Trace(preRequisites: () => this.StatBoost8.Active && this.CharacterLevel.Level>=75);
                StatBoost10 = new Trace(preRequisites: () => this.StatBoost8.Active && this.CharacterLevel.Level >= 80);
                break;
            case CharacterPath.Hunt:
                StatBoost1 = new Trace();
                
                Ascension2 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=2);
                StatBoost2 = new Trace(preRequisites:() => this.Ascension2.Active);
                StatBoost3 = new Trace(preRequisites: () => this.StatBoost2.Active && this.CharacterLevel.AscensionLevel>=3);

                StatBoost4 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 3);
                
                Ascension4 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=4);
                StatBoost5 = new Trace(preRequisites: () => this.Ascension4.Active);
                StatBoost6 = new Trace(preRequisites: () => this.StatBoost5.Active && this.CharacterLevel.AscensionLevel>=5);
                
                StatBoost7 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel>=5);
                
                Ascension6 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=6);
                StatBoost8 = new Trace(preRequisites: () => this.Ascension6.Active);
                StatBoost9 = new Trace(preRequisites: () => this.StatBoost8.Active && this.CharacterLevel.Level>=75);
                StatBoost10 = new Trace(preRequisites: () => this.StatBoost8.Active && this.CharacterLevel.Level >= 80);
                break;
            case CharacterPath.Erudition:
                StatBoost1 = new Trace();
                
                Ascension2 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=2);
                StatBoost2 = new Trace(preRequisites:() => this.Ascension2.Active);
                StatBoost3 = new Trace(preRequisites: () => this.StatBoost2.Active && this.CharacterLevel.AscensionLevel>=3);
                StatBoost4 = new Trace(preRequisites: () => this.StatBoost2.Active && this.CharacterLevel.AscensionLevel>=3);
                
                Ascension4 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=4);
                StatBoost5 = new Trace(preRequisites: () => this.Ascension4.Active);
                StatBoost6 = new Trace(preRequisites: () => this.StatBoost5.Active && this.CharacterLevel.AscensionLevel>=5);
                StatBoost7 = new Trace(preRequisites: () => this.StatBoost5.Active && this.CharacterLevel.AscensionLevel>=5);
                
                Ascension6 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=6);
                StatBoost8 = new Trace(preRequisites: () => this.Ascension6.Active);
                StatBoost9 = new Trace(preRequisites: () => this.Ascension6.Active && this.CharacterLevel.Level>=75);

                StatBoost10 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 80);
                break;
            case CharacterPath.Harmony:
                StatBoost1 = new Trace();
                
                Ascension2 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=2);
                StatBoost2 = new Trace(preRequisites:() => this.Ascension2.Active);
                StatBoost3 = new Trace(preRequisites: () => this.StatBoost2.Active && this.CharacterLevel.AscensionLevel>=3);

                StatBoost4 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 3 && this.StatBoost1.Active);
                
                Ascension4 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=4);
                StatBoost5 = new Trace(preRequisites: () => this.Ascension4.Active);
                StatBoost6 = new Trace(preRequisites: () => this.StatBoost5.Active && this.CharacterLevel.AscensionLevel>=5);

                StatBoost7 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 5 && this.StatBoost1.Active);
                
                Ascension6 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=6);
                StatBoost8 = new Trace(preRequisites: () => this.Ascension6.Active);
                StatBoost9 = new Trace(preRequisites: () => this.StatBoost8.Active && this.CharacterLevel.Level>=75);
                StatBoost10 = new Trace(preRequisites: () => this.StatBoost8.Active && this.CharacterLevel.Level >= 80);
                break;
            case CharacterPath.Nihility:
                StatBoost1 = new Trace();
                
                Ascension2 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=2);
                StatBoost2 = new Trace(preRequisites:() => this.Ascension2.Active);
                StatBoost3 = new Trace(preRequisites: () => this.StatBoost2.Active && this.CharacterLevel.AscensionLevel>=3);
                StatBoost4 = new Trace(preRequisites: () => this.StatBoost3.Active);
                
                Ascension4 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=4);
                StatBoost5 = new Trace(preRequisites: () => this.Ascension4.Active);
                StatBoost6 = new Trace(preRequisites: () => this.StatBoost5.Active && this.CharacterLevel.AscensionLevel>=5);
                StatBoost7 = new Trace(preRequisites: () => this.StatBoost6.Active);
                
                Ascension6 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=6);
                StatBoost8 = new Trace(preRequisites: () => this.Ascension6.Active);
                StatBoost9 = new Trace(preRequisites: () => this.Ascension6.Active && this.CharacterLevel.Level >= 75);
                
                StatBoost10 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 80 && this.StatBoost1.Active);
                break;
            case CharacterPath.Preservation:
                StatBoost1 = new Trace();
                
                Ascension2 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=2 && this.StatBoost1.Active);
                StatBoost2 = new Trace(preRequisites:() => this.Ascension2.Active);
                StatBoost3 = new Trace(preRequisites: () => this.StatBoost2.Active && this.CharacterLevel.AscensionLevel>=3);

                StatBoost4 = new Trace(preRequisites: () => CharacterLevel.AscensionLevel >= 3);
                
                Ascension4 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=4 && this.StatBoost1.Active);
                StatBoost5 = new Trace(preRequisites: () => this.Ascension4.Active);
                StatBoost6 = new Trace(preRequisites: () => this.StatBoost5.Active && this.CharacterLevel.AscensionLevel>=5);

                StatBoost7 = new Trace(preRequisites: () => CharacterLevel.AscensionLevel >= 5);
                
                Ascension6 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=6);
                StatBoost8 = new Trace(preRequisites: () => this.Ascension6.Active);
                StatBoost9 = new Trace(preRequisites: () => this.StatBoost8.Active && this.CharacterLevel.Level>=75);
                StatBoost10 = new Trace(preRequisites: () => this.StatBoost8.Active && this.CharacterLevel.Level >= 80);
                break;
            case CharacterPath.Abundance:
                StatBoost1 = new Trace();
                
                Ascension2 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=2);
                StatBoost2 = new Trace(preRequisites:() => this.Ascension2.Active);
                StatBoost3 = new Trace(preRequisites: () => this.StatBoost2.Active && this.CharacterLevel.AscensionLevel>=3);
                StatBoost4 = new Trace(preRequisites: () => this.StatBoost3.Active);
                
                Ascension4 = new Trace(preRequisites:()=>this.CharacterLevel.AscensionLevel>=4);
                StatBoost5 = new Trace(preRequisites: () => this.Ascension4.Active);
                StatBoost6 = new Trace(preRequisites: () => this.StatBoost5.Active && this.CharacterLevel.AscensionLevel>=5);
                StatBoost7 = new Trace(preRequisites: () => this.StatBoost6.Active);

                Ascension6 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 6);
                StatBoost8 = new Trace(preRequisites: () => this.Ascension6.Active);
                StatBoost9 = new Trace(preRequisites: () => this.Ascension6.Active && this.CharacterLevel.Level>=75);

                StatBoost10 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 80);
                break;
        }
    }

    public static float GetCharacterSpeed(CharacterID id)
    {
        return id switch
        {
            CharacterID.Bronya => 99,
            CharacterID.Blade => 97,
            CharacterID.Tingyun => 112
        };
    }

    public CharacterPath GetCharacterPath(CharacterID id)
    {
        return id switch
        {
            CharacterID.Bronya or CharacterID.Tingyun => CharacterPath.Harmony,
            CharacterID.Blade => CharacterPath.Destruction
        };
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