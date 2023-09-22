using HonkaiStarRailSimulator;
using HonkaiStarRailSimulator.Characters;

var ts = new TurnSystem();
var bronya = new Bronya();
{
    var newBronyaSpeed = new Stat(bronya.Speed);
    newBronyaSpeed.PercentageBonus += .06f;
    newBronyaSpeed.FlatBonus += 25 + 8 + 7 + 4 + 7;
    bronya.Speed =
        newBronyaSpeed; // Setters only work when reference is changed not just value, might work if I use INotifyPropertyChanged
}
var blade = (Blade)ts.AddEntity(new Blade());
ts.AddEntity(bronya);
bronya.Target = (Option<MovableEntity>)blade;
const int cycles = 7;

for (var i = 0; i < cycles; i++)
{
    ts.RunCycle();
}

ts.Display();

Console.WriteLine($"Blade turns: {blade.Turns}");
Console.WriteLine($"Blade HP: {blade.MaxHp.GetFinalValue()}");
Console.WriteLine($"Blade ATK: {blade.Atk.GetFinalValue()}");
Console.WriteLine($"Blade DEF: {blade.Def.GetFinalValue()}");