namespace HonkaiStarRailSimulator;

public readonly struct CharacterLevel
{
    public int Level { get; }

    public int AscensionLevel { get; }

    public int MaxLevel => 20 + AscensionLevel * 10;

    public int MinLevel => (AscensionLevel + 1) * 10 + (AscensionLevel == 0 ? 1 : 0);
    
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
        Level = level;
        AscensionLevel = GetAscensionLevel(level, ascended);
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
    TopazAndNumby = 1112,
    Qingque = 1201,
    Tingyun = 1202,
    Luocha = 1203,
    JingYuan = 1204,
    Blade = 1205,
    Sushang = 1206,
    Yukong = 1207,
    FuXuan = 1208,
    Yanqing = 1209,
    Guinaifen = 1210,
    Bailu = 1211,
    Jingliu = 1212,
    DanHengImbibitorLunae = 1213,
    PhysicalDestructionTrailblazerM = 8001,
    PhysicalDestructionTrailblazerF = 8002,
    FirePreservationTrailblazerM = 8003,
    FirePreservationTrailblazerF = 8004,
};

public enum DamageBonusType
{
    All,
    BasicAttack,
    Skill,
    Talent,
    FollowUp,
}

public enum DamageResType
{
    All
}

public abstract class Character : Entity
{
    public class Trace
    {
        private bool _active;
        private Trace? _parent;
        private List<Trace> _children = new();

        public bool Active
        {
            get => _active;
            set
            {
                var prevActive = _active;
                _active = (_parent == null || _parent.Active) && PreRequisites() && value;
                if (_active == prevActive) return;
                if (_active)
                    OnActivation();
                else
                {
                    OnDeactivation();
                    foreach (var child in _children)
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
                trace._parent = this;
                _children.Add(trace);
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
            _active = active;
            PreRequisites = preRequisites;
            OnActivation = onActivation;
            OnDeactivation = onDeactivation;
        }
    }

    private int _eidolon = 0;
    public int Eidolon
    {
        get => _eidolon;
        set
        {
            value = int.Min(int.Max(value, 0), 6);
            if (value < _eidolon)
            {
                for (var i = _eidolon; i > value; --i)
                {
                    switch (i)
                    {
                        case 6:
                            DeactivateEidolon6();
                            break;
                        case 5:
                            DeactivateEidolon5();
                            break;
                        case 4:
                            DeactivateEidolon4();
                            break;
                        case 3:
                            DeactivateEidolon3();
                            break;
                        case 2:
                            DeactivateEidolon2();
                            break;
                        case 1:
                            DeactivateEidolon1();
                            break;
                    }
                }
            } else if (value > _eidolon)
            {
                for (var i = _eidolon + 1; _eidolon <= value; ++i)
                {
                    switch (i)
                    {
                        case 1:
                            ActivateEidolon1();
                            break;
                        case 2:
                            ActivateEidolon2();
                            break;
                        case 3:
                            ActivateEidolon3();
                            break;
                        case 4:
                            ActivateEidolon4();
                            break;
                        case 5:
                            ActivateEidolon5();
                            break;
                        case 6:
                            ActivateEidolon6();
                            break;
                    }
                }
            }
            _eidolon = value;
        }
    }

    protected virtual void DeactivateEidolon1() {}
    protected virtual void DeactivateEidolon2() {}
    protected virtual void DeactivateEidolon3() {}
    protected virtual void DeactivateEidolon4() {}
    protected virtual void DeactivateEidolon5() {}
    protected virtual void DeactivateEidolon6() {}
    protected virtual void ActivateEidolon1() {}
    protected virtual void ActivateEidolon2() {}
    protected virtual void ActivateEidolon3() {}
    protected virtual void ActivateEidolon4() {}
    protected virtual void ActivateEidolon5() {}
    protected virtual void ActivateEidolon6() {}
    public CharacterLevel CharacterLevel { get; init; }

    private IOption<Lightcone> _lightcone = new None<Lightcone>();

    public IOption<Lightcone> Lightcone
    {
        get => _lightcone;
        set
        {
            _lightcone.Match(
                onNone: () => { },
                onSome: (lc) => {lc.DetachFrom(this);}
            );
            _lightcone = value;
            _lightcone.Match(
                onNone: () => { },
                onSome: (lc) => {lc.AttachTo(this);}
            );
        }
    }

    public class OnSkillArgs : EventArgs
    {
    }

    public event HSREventHandler<Character, OnSkillArgs> OnSkill;
    public class OnNormalAttackArgs : EventArgs
    {
    }
    public event HSREventHandler<Character, OnNormalAttackArgs> OnNormalAttack;
    public class OnUltimateArgs : EventArgs
    {
    }
    public event HSREventHandler<Character, OnUltimateArgs> OnUltimate;

    private int _basicAttackLevel = 1;
    private int _basicAttackLevelModifier = 0;
    public int BasicAttackLevel
    {
        get => _basicAttackLevel + _basicAttackLevelModifier;
        set
        {
            _basicAttackLevel = int.Min(int.Max(value, _basicAttackLevelModifier+1), 6+_basicAttackLevelModifier)-_basicAttackLevelModifier;
        }
    }

    public int UnmodifiedBasicAttackLevel
    {
        get => _basicAttackLevel;
        set => _basicAttackLevel = int.Min(int.Max(value, 1), 6);
    }
    
    private int _skillLevel = 1;
    private int _skillLevelModifier = 0;
    public int SkillLevel
    {
        get => _skillLevel + _skillLevelModifier;
        set
        {
            _skillLevel = int.Min(int.Max(value, _skillLevelModifier+1), 10+_skillLevelModifier)-_skillLevelModifier;
        }
    }

    public int UnmodifiedSkillLevel
    {
        get => _skillLevel;
        set => _skillLevel = int.Min(int.Max(value, 1), 10);
    }
    
    private int _ultimateLevel = 1;
    private int _ultimateLevelModifier = 0;
    public int UltimateLevel
    {
        get => _ultimateLevel + _ultimateLevelModifier;
        set
        {
            _ultimateLevel = int.Min(int.Max(value, _ultimateLevelModifier+1), 10+_ultimateLevelModifier)-_ultimateLevelModifier;
        }
    }

    public int UnmodifiedUltimateLevel
    {
        get => _ultimateLevel;
        set => _ultimateLevel = int.Min(int.Max(value, 1), 10);
    }
    
    private int _talentLevel = 1;
    private int _talentLevelModifier = 0;
    public int TalentLevel
    {
        get => _talentLevel + _talentLevelModifier;
        set
        {
            _talentLevel = int.Min(int.Max(value, _talentLevelModifier+1), 10+_talentLevelModifier)-_talentLevelModifier;
        }
    }

    public int UnmodifiedTalentLevel
    {
        get => _talentLevel;
        set => _talentLevel = int.Min(int.Max(value, 1), 10);
    }

    public Trace Ascension2 { get; init; } = new();
    public Trace Ascension4 { get; init; } = new();
    public Trace Ascension6 { get; init; } = new();
    public Trace StatBoost1 { get; init; } = new();
    public Trace StatBoost2 { get; init; } = new();
    public Trace StatBoost3 { get; init; } = new();
    public Trace StatBoost4 { get; init; } = new();
    public Trace StatBoost5 { get; init; } = new();
    public Trace StatBoost6 { get; init; } = new();
    public Trace StatBoost7 { get; init; } = new();
    public Trace StatBoost8 { get; init; } = new();
    public Trace StatBoost9 { get; init; } = new();
    public Trace StatBoost10 { get; init; } = new();

    public override void FinishTurn()
    {
        base.FinishTurn();
        CritRate.ExhaustStatusEffects();
        CritDamage.ExhaustStatusEffects();
        BreakEffect.ExhaustStatusEffects();
        OutgoingHealingBoost.ExhaustStatusEffects();
        EnergyRegenerationRate.ExhaustStatusEffects();
        foreach (var (key, _) in ElementalDamageBoost)
        {
            ElementalDamageBoost[key].ExhaustStatusEffects();
        }
        foreach (var (key, _) in DamageBonuses)
        {
            DamageBonuses[key].ExhaustStatusEffects();
        }
    }

    public CharacterId Id { get; }
    public CharacterPath Path { get; }
    public Stat CritRate { get; set; } = new(baseValue: .05f);
    public Stat CritDamage { get; set; } = new(baseValue: .5f);
    public Stat BreakEffect { get; set; } = new();
    public Stat OutgoingHealingBoost { get; set; } = new();
    public float MaxEnergy { get; set; }
    public Stat EnergyRegenerationRate { get; set; } = new();

    public Dictionary<Element, Stat> ElementalDamageBoost { get; set; } = Enum.GetValues(typeof(Element)).Cast<Element>()
        .ToDictionary(d => d, _ => new Stat());

    public Dictionary<DamageBonusType, Stat> DamageBonuses { get; set; } = Enum.GetValues(typeof(DamageBonusType))
        .Cast<DamageBonusType>().ToDictionary(d => d, _ => new Stat());

    public float GetTotalDamageBoost(IOption<Element> element, params DamageBonusType[] damageBonusTypes)
    {
        float res = 0;
        element.Match(
            onSome: someElement => res+=ElementalDamageBoost[someElement].GetFinalValue(),
            onNone: () => { }
        );
        foreach (var damageBonusType in damageBonusTypes)
        {
            res += DamageBonuses[damageBonusType].GetFinalValue();
        }
        return res;
    }

    protected Character(CharacterId id, int level) : base(GetCharacterSpeed(id),
        GetCharacterMaxHp(id, int.Max(int.Min(level, 80), 1)),
        GetCharacterAtk(id, int.Max(int.Min(level, 80), 1)), GetCharacterDef(id, int.Max(int.Min(level, 80), 1)))
    {
        CharacterLevel = new CharacterLevel(level);
        Id = id;
        MaxEnergy = GetCharacterMaxEnergy(id);
        Path = GetCharacterPath(id);
        OnSkill = (_, _, _) => { };
        OnNormalAttack = (_, _, _) => { };
        OnUltimate = (_, _, _) => { };
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
        return Globals.CharacterInfo[id].Stats.SpeedBase;
    }

    public CharacterPath GetCharacterPath(CharacterId id)
    {
        return Globals.CharacterInfo[id].Path;
    }

    public static float GetCharacterMaxEnergy(CharacterId id)
    {
        return Globals.CharacterInfo[id].Stats.MaxEnergy;
    }

    public static float GetCharacterMaxHp(CharacterId id, int level)
    {
        var ascension = CharacterLevel.GetAscensionLevel(level);
        return (level - 1) * Globals.CharacterInfo[id].Stats.HpAdd +
               Globals.CharacterInfo[id].Stats.HpBase[ascension];
    }

    public static float GetCharacterAtk(CharacterId id, int level)
    {
        var ascension = CharacterLevel.GetAscensionLevel(level);
        return (level - 1) * Globals.CharacterInfo[id].Stats.AttackAdd +
               Globals.CharacterInfo[id].Stats.AttackBase[ascension];
    }

    public static float GetCharacterDef(CharacterId id, int level)
    {
        var ascension = CharacterLevel.GetAscensionLevel(level);
        return (level - 1) * Globals.CharacterInfo[id].Stats.DefenceAdd +
               Globals.CharacterInfo[id].Stats.DefenceBase[ascension];
    }

    // TODO: Implement abstract record or simple enum to denote if NA/Skill/Ult ends turn (they return this type)
    public virtual void BasicAttack(params MovableEntity[] entities)
    {
    }

    protected List<float> _getBasicAttackScalings()
    {
        return Globals.CharacterInfo[Id].Scalings.BasicAttack[BasicAttackLevel - 1];
    } 

    public virtual void Skill(params MovableEntity[] entities)
    {
    }
    protected List<float> _getSkillScalings()
    {
        return Globals.CharacterInfo[Id].Scalings.Skill[SkillLevel - 1];
    } 

    public virtual void Ultimate(params MovableEntity[] entities)
    {
    }
    protected List<float> _getUltimateScalings()
    {
        return Globals.CharacterInfo[Id].Scalings.Ultimate[UltimateLevel - 1];
    } 
    protected List<float> _getTalentScalings()
    {
        return Globals.CharacterInfo[Id].Scalings.Talent[TalentLevel - 1];
    } 

    public override string ToString()
    {
        return Globals.CharacterInfo[Id].Name;
    }
}