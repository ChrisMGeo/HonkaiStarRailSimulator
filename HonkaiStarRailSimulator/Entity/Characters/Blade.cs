namespace HonkaiStarRailSimulator.Characters;

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