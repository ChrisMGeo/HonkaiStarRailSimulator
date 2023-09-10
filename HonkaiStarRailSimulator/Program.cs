using HonkaiStarRailSimulator;
using HonkaiStarRailSimulator.Characters;

var ts = new TurnSystem();
var bronya = new Bronya();
{
    var newBronyaSpeed = new Stat(bronya.Speed);
    newBronyaSpeed.PercentageBonus += .06f;
    newBronyaSpeed.FlatBonus += 25 + 8 + 7 + 4 + 5;
    bronya.Speed =
        newBronyaSpeed; // Setters only work when reference is changed not just value, might work if I use INotifyPropertyChanged
}
ts.AddEntity(bronya);
var blade = (Blade)ts.AddEntity(new Blade());
bronya.Target = (Option<MovableEntity>)blade;
const int cycles = 7;
// while (ts.TotalAv < 150 + (cycles - 1) * 100)
// {
//     ts.Display();
//     ts.MoveToNextTurn();
//     if (ts.TotalAv >= 150 + (cycles - 1) * 100) break;
//     Console.WriteLine("MOVED TO NEXT TURN");
//     ts.Display();
//     ts.DoAction();
// }

for (var i = 0; i < cycles; i++)
{
    ts.RunCycle();
}

Console.WriteLine($"Blade turns: {blade.Turns}");