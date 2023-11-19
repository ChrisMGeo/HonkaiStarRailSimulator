namespace HonkaiStarRailSimulator.Characters;

public class Blade : Character
{
    public Blade(int level = 80) : base(CharacterId.Blade, level)
    {
    }

    public override void DoAction()
    {
        Console.WriteLine($"DMG Boost: {GetTotalDamageBoost(Some<Element>.Of(Element.Wind), DamageBonusType.All, DamageBonusType.BasicAttack)}");
        // Do Nothing
        FinishTurn();
    }
}