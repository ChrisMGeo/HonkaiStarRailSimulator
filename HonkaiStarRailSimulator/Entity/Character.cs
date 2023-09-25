namespace HonkaiStarRailSimulator;

public class CharacterLevel
{
    private int _level;

    public int Level
    {
        get => _level;
        set
        {
            var level = int.Min(int.Max(value, MinLevel), 80);
            _level = level;
            _ascensionLevel = GetAscensionLevel(level);
        }
    }

    private int _ascensionLevel;

    public int AscensionLevel
    {
        get => _ascensionLevel;
    }

    public int MaxLevel => 20 + _ascensionLevel * 10;

    public int MinLevel => (_ascensionLevel + 1) * 10 + (_ascensionLevel == 0 ? 1 : 0);

    //     _ascensionLevel switch
    // {
    //     0 => 1,
    //     1 => 20,
    //     2 => 30,
    //     3 => 40,
    // }
    public static int GetAscensionLevel(int level, bool ascended = false)
    {
        return int.Min(
            int.Max(
                (level - 20) / 10 + (level is >= 20 and < 80 ? ((level - 20) % 10 == 0 ? (ascended ? 1 : 0) : 1) : 0),
                0), 6);
    }

    public CharacterLevel(int level, bool ascended = false)
    {
        level = int.Min(int.Max(level, 1), 80);
        _level = level;
        _ascensionLevel = GetAscensionLevel(level, ascended);
    }

    // public void LevelUp()
    // {
    //     if (_level == 80) return;
    //     if (_level == MaxLevel)
    //     {
    //         _ascensionLevel++;
    //         return;
    //     }
    //
    //     _level++;
    // }
    //
    // public void LevelTo(int finalLevel)
    // {
    //     finalLevel = int.Max(int.Min(finalLevel, 80), _level);
    //     while (_level != finalLevel)
    //     {
    //         LevelUp();
    //     }
    // }
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
    // DestructionTrailblazerM = 8001,
    // DestructionTrailblazerF = 8002,
    // PreservationTrailblazerM = 8003,
    // PreservationTrailblazerF = 8004,
}

public abstract class Character : Entity
{
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

        public Func<bool> PreRequisites { get; set; }
        public Action OnActivation { get; set; }
        public Action OnDeactivation { get; set; }

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

    public CharacterLevel CharacterLevel { get; init; }
    public event EventHandler OnSkill;
    public event EventHandler OnNormalAttack;
    public event EventHandler OnUltimate;

    public Trace Ascension2 { get; init; } = new Trace();
    public Trace Ascension4 { get; init; } = new Trace();
    public Trace Ascension6 { get; init; } = new Trace();
    public Trace StatBoost1 { get; init; } = new Trace();
    public Trace StatBoost2 { get; init; } = new Trace();
    public Trace StatBoost3 { get; init; } = new Trace();
    public Trace StatBoost4 { get; init; } = new Trace();
    public Trace StatBoost5 { get; init; } = new Trace();
    public Trace StatBoost6 { get; init; } = new Trace();
    public Trace StatBoost7 { get; init; } = new Trace();
    public Trace StatBoost8 { get; init; } = new Trace();
    public Trace StatBoost9 { get; init; } = new Trace();
    public Trace StatBoost10 { get; init; } = new Trace();

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
        CharacterLevel = new CharacterLevel(level);
        Id = id;
        MaxEnergy = GetCharacterMaxEnergy(id);
        Path = GetCharacterPath(id);
        OnSkill = (sender, args) => { };
        OnNormalAttack = (sender, args) => { };
        OnUltimate = (sender, args) => { };
        Ascension2.PreRequisites = () => CharacterLevel.AscensionLevel >= 2;
        Ascension2.AddChild(StatBoost2);
        Ascension4.PreRequisites = () => CharacterLevel.AscensionLevel >= 4;
        Ascension4.AddChild(StatBoost5);
        Ascension6.PreRequisites = () => CharacterLevel.AscensionLevel >= 6;
        Ascension6.AddChild(StatBoost8);
        StatBoost2.AddChild(StatBoost3);
        StatBoost5.AddChild(StatBoost6);
        StatBoost3.PreRequisites = () => CharacterLevel.AscensionLevel >= 3;
        StatBoost6.PreRequisites = () => CharacterLevel.AscensionLevel >= 5;
        StatBoost9.PreRequisites = () => CharacterLevel.Level >= 75;
        StatBoost10.PreRequisites = () => CharacterLevel.Level >= 80;
        switch (Path)
        {
            case CharacterPath.Destruction:
                StatBoost3.AddChild(StatBoost4);
                StatBoost6.AddChild(StatBoost7);
                StatBoost8.AddChild(StatBoost9, StatBoost10);
                break;
            case CharacterPath.TheHunt:
                StatBoost4.PreRequisites = () => CharacterLevel.AscensionLevel >= 3;
                StatBoost7.PreRequisites = () => CharacterLevel.AscensionLevel >= 5;
                StatBoost8.AddChild(StatBoost9, StatBoost10);
                break;
            case CharacterPath.Erudition:
                StatBoost4.PreRequisites = () =>
                    CharacterLevel.AscensionLevel >= 3;
                StatBoost2.AddChild(StatBoost4);
                StatBoost7.PreRequisites = () =>
                    CharacterLevel.AscensionLevel >= 5;
                StatBoost5.AddChild(StatBoost7);
                Ascension6.AddChild(StatBoost9);
                break;
            case CharacterPath.Harmony:
                StatBoost4.PreRequisites = () =>
                    CharacterLevel.AscensionLevel >= 3;
                StatBoost1.AddChild(StatBoost4);
                StatBoost7.PreRequisites = () =>
                    CharacterLevel.AscensionLevel >= 5;
                StatBoost1.AddChild(StatBoost7);
                StatBoost8.AddChild(StatBoost9, StatBoost10);
                break;
            case CharacterPath.Nihility:
                StatBoost3.AddChild(StatBoost4);
                StatBoost6.AddChild(StatBoost7);
                Ascension6.AddChild(StatBoost9);
                StatBoost1.AddChild(StatBoost10);
                break;
            case CharacterPath.Preservation:
                StatBoost4.PreRequisites = () => CharacterLevel.AscensionLevel >= 3;
                StatBoost1.AddChild(Ascension2, Ascension4);
                StatBoost7.PreRequisites = () => CharacterLevel.AscensionLevel >= 5;
                StatBoost8.AddChild(StatBoost9, StatBoost10);
                break;
            case CharacterPath.Abundance:
                StatBoost3.AddChild(StatBoost4);
                StatBoost6.AddChild(StatBoost7);
                Ascension6.AddChild(StatBoost9);
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