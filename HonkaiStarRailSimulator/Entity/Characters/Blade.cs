namespace HonkaiStarRailSimulator.Characters;

public class Blade : Character
{
    public Blade(int level = 80) : base(CharacterId.Blade, level)
    {
    }

    public override string ToString()
    {
        return "Blade";
    }
}