using HonkaiStarRailSimulator;
using HonkaiStarRailSimulator.Characters;

var ts = new TurnSystem();
var bronya = new Bronya();
{
    var newBronyaSpeed = new Stat(bronya.Speed);
    newBronyaSpeed.PercentageBonus += .06f;
    newBronyaSpeed.FlatBonus += 25.032f + 7.2f + 4.3f + 4.3f + 7.2f + 7.8f;
    bronya.Speed =
        newBronyaSpeed; // Setters only work when reference is changed not just value, might work if I use INotifyPropertyChanged
    bronya.MaxHp.BaseValue += 1164;
    bronya.MaxHp.FlatBonus += 705 + 33 + 38 + 42;
    bronya.MaxHp.PercentageBonus += .082f + .125f + .432f;
    bronya.Def.BaseValue += 463;
    bronya.Def.FlatBonus += 52;
    bronya.Def.PercentageBonus += 0.048f + 0.054f;
    bronya.Atk.BaseValue += 529;
    bronya.Atk.FlatBonus += 352 + 40 + 21;
    bronya.Atk.PercentageBonus += 0.077f + 0.043f + 0.125f;
}
var blade = (Blade)ts.AddEntity(new Blade());
ts.AddEntity(bronya);
bronya.Target = Some<MovableEntity>.Of(blade);
const int cycles = 7;

for (var i = 0; i < cycles; i++)
{
    ts.RunCycle();
}

ts.Display();

Console.WriteLine($"Blade turns : {blade.Turns}");
Console.WriteLine($"Blade HP    : {blade.MaxHp.GetFinalValue()}");
Console.WriteLine($"Blade ATK   : {blade.Atk.GetFinalValue()}");
Console.WriteLine($"Blade DEF   : {blade.Def.GetFinalValue()}");
Console.WriteLine($"Blade LVL   : {blade.CharacterLevel.Level}/{blade.CharacterLevel.MaxLevel}");
Console.WriteLine();
Console.WriteLine($"Bronya HP   : {bronya.MaxHp.GetFinalValue()}");
Console.WriteLine($"Bronya ATK  : {bronya.Atk.GetFinalValue()}");
Console.WriteLine($"Bronya DEF  : {bronya.Def.GetFinalValue()}");
Console.WriteLine($"Bronya SPD  : {bronya.Speed.GetFinalValue()}");