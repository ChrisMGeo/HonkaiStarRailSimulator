namespace HonkaiStarRailSimulator;

public class CharacterInfo
{
    public string Name { get; set; } = "Unnamed";
    public CharacterPath Path { get; set; } = CharacterPath.TheHunt;
    public CharacterStatInfo Stats { get; set; } = new();
}

public class CharacterStatInfo
{
    public float SpeedBase { get; set; } = 100;
    public float BaseAggro { get; set; } = 100;
    public List<float> HpBase { get; set; } = new();
    public float HpAdd { get; set; }
    public List<float> AttackBase { get; set; } = new();
    public float AttackAdd { get; set; }
    public List<float> DefenceBase { get; set; } = new();
    public float DefenceAdd { get; set; }
    public float MaxEnergy { get; set; }
}

public class CharacterScalingInfo
{
    public List<List<float>> Skill { get; set; } = new();
    public List<List<float>> BasicAttack { get; set; } = new();
    public List<List<float>> Ultimate { get; set; } = new();
    public List<List<float>> Talent { get; set; } = new();
}