namespace HonkaiStarRailSimulator.Lightcones;

public class ButTheBattleIsntOver : Lightcone
{
    private readonly StatusEffect _errBoost;
    public ButTheBattleIsntOver(int level = 80) : base(LightconeId.ButTheBattleIsnTOver, level)
    {
        
    }

    public override void AttachTo(Character c)
    {
        base.AttachTo(c);
    }

    public override void DetachFrom(Character c)
    {
        base.DetachFrom(c);
    }
}