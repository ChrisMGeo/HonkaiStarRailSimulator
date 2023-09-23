namespace HonkaiStarRailSimulator;

public abstract record CharacterLevel
{
    public static int GetAscensionLevel(int level, bool ascended = false)
    {
        return int.Min(
            int.Max(
                (level - 20) / 10 + (level is >= 20 and < 80 ? ((level - 20) % 10 == 0 ? (ascended ? 1 : 0) : 1) : 0),
                0), 6);
    }

    private record Ascended(int AscendedLevel) : CharacterLevel; // 20/30, 30/40,...70/80

    public record Unascended(int UnascendedLevel) : CharacterLevel; // 1-20/20, 21-30/30,...71-80/80

    public int AscensionLevel => this switch
    {
        Unascended unascended => GetAscensionLevel(unascended.UnascendedLevel),
        Ascended ascended => GetAscensionLevel(ascended.AscendedLevel, true),
        _ => throw new ArgumentOutOfRangeException()
    };

    public int MaxLevel => 20 + AscensionLevel * 10;

    public int Level => this switch
    {
        Unascended unascended => unascended.UnascendedLevel, Ascended ascended => ascended.AscendedLevel,
        _ => throw new ArgumentOutOfRangeException()
    };
}

public class Trace
{
    private bool active = false;
    private Trace? parent = null;
    private List<Trace> children = new List<Trace>();

    public bool Active
    {
        get => active;
        set
        {
            var prevActive = active;
            active = (parent == null || parent.Active) && PreRequisites() && value;
            if (active == prevActive) return;
            if (active)
                OnActivation();
            else
            {
                OnDeactivation();
                foreach (var child in children)
                {
                    child.Active = false;
                }
            }
        }
    }

    public void AddChild(params Trace[] traces)
    {
        foreach (var trace in traces)
        {
            trace.Active = false;
            trace.parent = this;
            children.Add(trace);
        }
    }

    public Func<bool> PreRequisites { get; protected set; }
    public Action OnActivation { get; protected set; }
    public Action OnDeactivation { get; protected set; }

    public Trace(bool active = false, Func<bool>? preRequisites = null, Action? onActivation = null,
        Action? onDeactivation =
            null)
    {
        preRequisites ??= () => true;
        onActivation ??= () => { };
        onDeactivation ??= () => { };
        this.active = active;
        PreRequisites = preRequisites;
        OnActivation = onActivation;
        OnDeactivation = onDeactivation;
    }
}

public enum CharacterPath
{
    Destruction,
    TheHunt,
    Erudition,
    Harmony,
    Nihility,
    Preservation,
    Abundance
}

public enum CharacterId
{
    March7Th = 1001,
    DanHeng = 1002,
    Himeko = 1003,
    Welt = 1004,
    Kafka = 1005,
    SilverWolf = 1006,
    Arlan = 1008,
    Asta = 1009,
    Herta = 1013,
    Bronya = 1101,
    Seele = 1102,
    Serval = 1103,
    Gepard = 1104,
    Natasha = 1105,
    Pela = 1106,
    Clara = 1107,
    Sampo = 1108,
    Hook = 1109,
    Lynx = 1110,
    Luka = 1111,
    Qingque = 1201,
    Tingyun = 1202,
    Luocha = 1203,
    JingYuan = 1204,
    Blade = 1205,
    Sushang = 1206,
    Yukong = 1207,
    FuXuan = 1208,
    Yanqing = 1209,
    Bailu = 1211,
    DanHengImbibitorLunae = 1213,
    // Nickname = 8001,
    // Nickname = 8002,
    // Nickname = 8003,
    // Nickname = 8004,
}

public abstract class Character : Entity
{
    // private CharacterLevel _characterLevel;
    public CharacterLevel CharacterLevel { get; init; }
    public event EventHandler OnSkill;
    public event EventHandler OnNormalAttack;
    public event EventHandler OnUltimate;

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

    public CharacterId Id { get; }
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

    protected Character(CharacterId id, int level) : base(GetCharacterSpeed(id),
        GetCharacterMaxHp(id, int.Max(int.Min(level, 80), 1)),
        GetCharacterAtk(id, int.Max(int.Min(level, 80), 1)), GetCharacterDef(id, int.Max(int.Min(level, 80), 1)))
    {
        CharacterLevel = new CharacterLevel.Unascended(level);
        Id = id;
        MaxEnergy = GetCharacterMaxEnergy(id);
        Path = GetCharacterPath(id);
        OnSkill = (sender, args) => { };
        OnNormalAttack = (sender, args) => { };
        OnUltimate = (sender, args) => { };
        switch (Path)
        {
            case CharacterPath.Destruction:
                StatBoost1 = new Trace();

                Ascension2 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 2);
                StatBoost2 = new Trace();
                Ascension2.AddChild(StatBoost2);
                StatBoost3 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 3);
                StatBoost2.AddChild(StatBoost3);
                StatBoost4 = new Trace();
                StatBoost3.AddChild(StatBoost4);

                Ascension4 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 4);
                StatBoost5 = new Trace();
                Ascension4.AddChild(StatBoost5);
                StatBoost6 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 5);
                StatBoost5.AddChild(StatBoost6);
                StatBoost7 = new Trace();
                StatBoost6.AddChild(StatBoost7);

                Ascension6 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 6);
                StatBoost8 = new Trace();
                Ascension6.AddChild(StatBoost8);
                StatBoost9 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 75);
                StatBoost10 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 80);
                StatBoost8.AddChild(StatBoost9, StatBoost10);
                break;
            case CharacterPath.TheHunt:
                StatBoost1 = new Trace();

                Ascension2 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 2);
                StatBoost2 = new Trace();
                Ascension2.AddChild(StatBoost2);
                StatBoost3 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 3);
                StatBoost2.AddChild(StatBoost3);

                StatBoost4 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 3);

                Ascension4 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 4);
                StatBoost5 = new Trace();
                Ascension4.AddChild(StatBoost5);
                StatBoost6 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 5);
                StatBoost5.AddChild(StatBoost6);

                StatBoost7 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 5);

                Ascension6 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 6);
                StatBoost8 = new Trace();
                Ascension6.AddChild(StatBoost8);
                StatBoost9 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 75);
                StatBoost10 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 80);
                StatBoost8.AddChild(StatBoost9, StatBoost10);
                break;
            case CharacterPath.Erudition:
                StatBoost1 = new Trace();

                Ascension2 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 2);
                StatBoost2 = new Trace();
                Ascension2.AddChild(StatBoost2);
                StatBoost3 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 3);
                StatBoost4 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 3);
                StatBoost2.AddChild(StatBoost3, StatBoost4);

                Ascension4 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 4);
                StatBoost5 = new Trace();
                Ascension4.AddChild(StatBoost5);
                StatBoost6 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 5);
                StatBoost7 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 5);
                StatBoost5.AddChild(StatBoost6, StatBoost7);

                Ascension6 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 6);
                StatBoost8 = new Trace();
                StatBoost9 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 75);
                Ascension6.AddChild(StatBoost8, StatBoost9);

                StatBoost10 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 80);
                break;
            case CharacterPath.Harmony:
                StatBoost1 = new Trace();

                Ascension2 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 2);
                StatBoost2 = new Trace();
                Ascension2.AddChild(StatBoost2);
                StatBoost3 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 3);
                StatBoost2.AddChild(StatBoost3);

                StatBoost4 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 3);
                StatBoost1.AddChild(StatBoost4);

                Ascension4 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 4);
                StatBoost5 = new Trace();
                Ascension4.AddChild(StatBoost5);
                StatBoost6 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 5);
                StatBoost5.AddChild(StatBoost6);

                StatBoost7 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 5);
                StatBoost1.AddChild(StatBoost7);

                Ascension6 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 6);
                StatBoost8 = new Trace();
                Ascension6.AddChild(StatBoost8);
                StatBoost9 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 75);
                StatBoost10 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 80);
                StatBoost8.AddChild(StatBoost9, StatBoost10);
                break;
            case CharacterPath.Nihility:
                StatBoost1 = new Trace();

                Ascension2 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 2);
                StatBoost2 = new Trace();
                Ascension2.AddChild(StatBoost2);
                StatBoost3 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 3);
                StatBoost2.AddChild(StatBoost3);
                StatBoost4 = new Trace();
                StatBoost3.AddChild(StatBoost4);

                Ascension4 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 4);
                StatBoost5 = new Trace();
                Ascension4.AddChild(StatBoost5);
                StatBoost6 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 5);
                StatBoost5.AddChild(StatBoost6);
                StatBoost7 = new Trace();
                StatBoost6.AddChild(StatBoost7);

                Ascension6 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 6);
                StatBoost8 = new Trace();
                StatBoost9 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 75);
                Ascension6.AddChild(StatBoost8, StatBoost9);

                StatBoost10 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 80);
                StatBoost1.AddChild(StatBoost10);
                break;
            case CharacterPath.Preservation:
                StatBoost1 = new Trace();

                Ascension2 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 2);
                StatBoost2 = new Trace();
                Ascension2.AddChild(StatBoost2);
                StatBoost3 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 3);
                StatBoost2.AddChild(StatBoost3);

                StatBoost4 = new Trace(preRequisites: () => CharacterLevel.AscensionLevel >= 3);

                Ascension4 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 4);
                StatBoost5 = new Trace();
                Ascension4.AddChild(StatBoost5);
                StatBoost6 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 5);
                StatBoost5.AddChild(StatBoost6);

                StatBoost1.AddChild(Ascension2, Ascension4);

                StatBoost7 = new Trace(preRequisites: () => CharacterLevel.AscensionLevel >= 5);

                Ascension6 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 6);
                StatBoost8 = new Trace();
                Ascension6.AddChild(StatBoost8);
                StatBoost9 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 75);
                StatBoost10 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 80);
                StatBoost8.AddChild(StatBoost9, StatBoost10);
                break;
            case CharacterPath.Abundance:
                StatBoost1 = new Trace();

                Ascension2 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 2);
                StatBoost2 = new Trace();
                Ascension2.AddChild(StatBoost2);
                StatBoost3 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 3);
                StatBoost2.AddChild(StatBoost3);
                StatBoost4 = new Trace();
                StatBoost3.AddChild(StatBoost4);

                Ascension4 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 4);
                StatBoost5 = new Trace();
                Ascension4.AddChild(StatBoost5);
                StatBoost6 = new Trace(preRequisites: () =>
                    this.CharacterLevel.AscensionLevel >= 5);
                StatBoost5.AddChild(StatBoost6);
                StatBoost7 = new Trace();
                StatBoost6.AddChild(StatBoost7);

                Ascension6 = new Trace(preRequisites: () => this.CharacterLevel.AscensionLevel >= 6);
                StatBoost8 = new Trace();
                StatBoost9 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 75);
                Ascension6.AddChild(StatBoost8, StatBoost9);

                StatBoost10 = new Trace(preRequisites: () => this.CharacterLevel.Level >= 80);
                break;
        }
    }

    public static float GetCharacterSpeed(CharacterId id)
    {
        return Globals.CharacterInfo[(int)id].Stats.SpeedBase;
    }

    public CharacterPath GetCharacterPath(CharacterId id)
    {
        return Globals.CharacterInfo[(int)id].Path;
    }

    public static float GetCharacterMaxEnergy(CharacterId id)
    {
        // TODO: Implement
        return 100;
    }

    public static float GetCharacterMaxHp(CharacterId id, int level)
    {
        var ascension = CharacterLevel.GetAscensionLevel(level);
        return (level - 1) * Globals.CharacterInfo[(int)id].Stats.HPAdd +
               Globals.CharacterInfo[(int)id].Stats.HPBase[ascension];
    }

    public static float GetCharacterAtk(CharacterId id, int level)
    {
        var ascension = CharacterLevel.GetAscensionLevel(level);
        return (level - 1) * Globals.CharacterInfo[(int)id].Stats.AttackAdd +
               Globals.CharacterInfo[(int)id].Stats.AttackBase[ascension];
    }

    public static float GetCharacterDef(CharacterId id, int level)
    {
        var ascension = CharacterLevel.GetAscensionLevel(level);
        return (level - 1) * Globals.CharacterInfo[(int)id].Stats.DefenceAdd +
               Globals.CharacterInfo[(int)id].Stats.DefenceBase[ascension];
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

    public override string ToString()
    {
        return Globals.CharacterInfo[(int)Id].Name;
    }
}