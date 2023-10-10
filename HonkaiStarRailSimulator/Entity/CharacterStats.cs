namespace HonkaiStarRailSimulator;

public class CharacterInfo
{
    public string Name { get; set; } = "Unnamed";
    public CharacterPath Path { get; set; } = CharacterPath.TheHunt;
    public CharacterStatInfo Stats { get; set; } = new CharacterStatInfo();
}

public class CharacterStatInfo
{
    public float SpeedBase { get; set; } = 100;
    public float BaseAggro { get; set; } = 100;
    public List<float> HPBase { get; set; } = new List<float>();
    public float HPAdd { get; set; }
    public List<float> AttackBase { get; set; } = new List<float>();
    public float AttackAdd { get; set; }
    public List<float> DefenceBase { get; set; } = new List<float>();
    public float DefenceAdd { get; set; }
    public float MaxEnergy { get; set; }
}