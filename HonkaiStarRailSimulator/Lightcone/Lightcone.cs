namespace HonkaiStarRailSimulator;

public enum LightconeId
{
    Arrows = 20000,
    Cornucopia = 20001,
    CollapsingSky = 20002,
    Amber = 20003,
    Void = 20004,
    Chorus = 20005,
    DataBank = 20006,
    DartingArrow = 20007,
    FineFruit = 20008,
    ShatteredHome = 20009,
    Defense = 20010,
    Loop = 20011,
    MeshingCogs = 20012,
    Passkey = 20013,
    Adversarial = 20014,
    Multiplication = 20015,
    MutualDemise = 20016,
    Pioneering = 20017,
    HiddenShadow = 20018,
    Mediation = 20019,
    Sagacity = 20020,
    PostOpConversation = 21000,
    GoodNightAndSleepWell = 21001,
    DayOneOfMyNewLife = 21002,
    OnlySilenceRemains = 21003,
    MemoriesOfThePast = 21004,
    TheMolesWelcomeYou = 21005,
    TheBirthOfTheSelf = 21006,
    SharedFeeling = 21007,
    EyesOfThePrey = 21008,
    LandauSChoice = 21009,
    Swordplay = 21010,
    PlanetaryRendezvous = 21011,
    ASecretVow = 21012,
    MakeTheWorldClamor = 21013,
    PerfectTiming = 21014,
    ResolutionShinesAsPearlsOfSweat = 21015,
    TrendOfTheUniversalMarket = 21016,
    SubscribeForMore = 21017,
    DanceDanceDance = 21018,
    UnderTheBlueSky = 21019,
    GeniusesRepose = 21020,
    QuidProQuo = 21021,
    Fermata = 21022,
    WeAreWildfire = 21023,
    RiverFlowsInSpring = 21024,
    PastAndFuture = 21025,
    WoofWalkTime = 21026,
    TheSeriousnessOfBreakfast = 21027,
    WarmthShortensColdNights = 21028,
    WeWillMeetAgain = 21029,
    ThisIsMe = 21030,
    ReturnToDarkness = 21031,
    CarveTheMoonWeaveTheClouds = 21032,
    NowhereToRun = 21033,
    TodayIsAnotherPeacefulDay = 21034,
    BeforeTheTutorialMissionStarts = 22000,
    NightOnTheMilkyWay = 23000,
    InTheNight = 23001,
    SomethingIrreplaceable = 23002,
    ButTheBattleIsnTOver = 23003,
    InTheNameOfTheWorld = 23004,
    MomentOfVictory = 23005,
    PatienceIsAllYouNeed = 23006,
    IncessantRain = 23007,
    EchoesOfTheCoffin = 23008,
    TheUnreachableSide = 23009,
    BeforeDawn = 23010,
    SheAlreadyShutHerEyes = 23011,
    SleepLikeTheDead = 23012,
    TimeWaitsForNoOne = 23013,
    IShallBeMyOwnSword = 23014,
    BrighterThanTheSun = 23015,
    WorrisomeBlissful = 23016,
    OnTheFallOfAnAeon = 24000,
    CruisingInTheStellarSea = 24001,
    TextureOfMemories = 24002,
    SolitaryHealing = 24003,
}

public abstract class Lightcone
{ // TODO: Add an object for storing lightcone scalings for each superimposition
    public static CharacterPath GetLightconePath(LightconeId id)
    {
        return Globals.LightconeInfo[id].Path;
    }

    public static float GetLightconeMaxHp(LightconeId id, int level)
    {
        var ascension = CharacterLevel.GetAscensionLevel(level);
        return (level - 1) * Globals.LightconeInfo[id].Stats.HpAdd +
               Globals.LightconeInfo[id].Stats.HpBase[ascension];
    }

    public static float GetLightconeAtk(LightconeId id, int level)
    {
        var ascension = CharacterLevel.GetAscensionLevel(level);
        return (level - 1) * Globals.LightconeInfo[id].Stats.AttackAdd +
               Globals.LightconeInfo[id].Stats.AttackBase[ascension];
    }

    public static float GetLightconeDef(LightconeId id, int level)
    {
        var ascension = CharacterLevel.GetAscensionLevel(level);
        return (level - 1) * Globals.LightconeInfo[id].Stats.DefenceAdd +
               Globals.LightconeInfo[id].Stats.DefenceBase[ascension];
    }

    protected IOption<Character> _equippedCharacter = new None<Character>();
    protected StatusEffect _hpBoost;
    protected StatusEffect _defBoost;
    protected StatusEffect _atkBoost;
    public LightconeId Id { get; init; }
    public CharacterLevel Level { get; init; }
    public CharacterPath Path { get; init; }

    private int _superimposition = 1;

    public int SuperImposition
    {
        get => _superimposition;
        set
        {
            _superimposition = int.Max(int.Min(value, 5), 1);
        }
    }

    public virtual void AttachTo(Character c)
    {
        _equippedCharacter.Match(
            onNone: () => { },
            onSome: DetachFrom
        );
        c.Atk.AddStatusEffect(_atkBoost);
        c.MaxHp.AddStatusEffect(_hpBoost);
        c.Def.AddStatusEffect(_defBoost);
        _equippedCharacter = Some<Character>.Of(c);
    }

    public virtual void DetachFrom(Character c)
    {
        c.Atk.RemoveStatusEffect(_atkBoost);
        c.MaxHp.RemoveStatusEffect(_hpBoost);
        c.Def.RemoveStatusEffect(_defBoost);
        _equippedCharacter = new None<Character>();
    }

    public static void SwapWeapons(Character a, Character b)
    {
        a.Lightcone.Match(
            onSome: aLightcone =>
            {
                b.Lightcone.Match(
                    onSome: bLightcone =>
                    {
                        b.Lightcone = Some<Lightcone>.Of(aLightcone);
                        a.Lightcone = Some<Lightcone>.Of(bLightcone);
                    },
                    onNone: () =>
                    {
                        b.Lightcone = Some<Lightcone>.Of(aLightcone);
                        a.Lightcone = new None<Lightcone>();
                    }
                );
            },
            onNone: () =>
            {
                b.Lightcone.Match(
                    onSome: bLightcone =>
                    {
                        a.Lightcone = Some<Lightcone>.Of(bLightcone);
                        b.Lightcone = new None<Lightcone>();
                    },
                    onNone: () =>
                    {
                        a.Lightcone = new None<Lightcone>();
                        b.Lightcone = new None<Lightcone>();
                    }
                );
            }
        );
    }

    protected Lightcone(LightconeId id, int level = 80)
    {
        level = int.Max(int.Min(level, 80), 1);
        Level = new CharacterLevel(level);
        Id = id;
        Path = GetLightconePath(id);
        _hpBoost = new ConstantStatusEffect(StatusEffectId.PermanentStatBuff,
            new StatModifier(baseValue: GetLightconeMaxHp(id, level)));
        _atkBoost = new ConstantStatusEffect(StatusEffectId.PermanentStatBuff,
            new StatModifier(baseValue: GetLightconeAtk(id, level)));
        _defBoost = new ConstantStatusEffect(StatusEffectId.PermanentStatBuff,
            new StatModifier(baseValue: GetLightconeDef(id, level)));
    }
}

public class LightconeInfo
{
    public string Name { get; set; } = "Unnamed";
    public CharacterPath Path { get; set; } = CharacterPath.TheHunt;
    public LightconeStatInfo Stats { get; set; } = new();
}

public class LightconeStatInfo
{
    public List<float> HpBase { get; set; } = new();
    public float HpAdd { get; set; }
    public List<float> AttackBase { get; set; } = new();
    public float AttackAdd { get; set; }
    public List<float> DefenceBase { get; set; } = new();
    public float DefenceAdd { get; set; }
}