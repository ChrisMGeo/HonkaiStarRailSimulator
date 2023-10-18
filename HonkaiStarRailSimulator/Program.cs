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
var fuXuan = new FuXuan(level: 80);
{
    fuXuan.MaxHp.BaseValue += 1058;
    fuXuan.MaxHp.FlatBonus += 705.6f + 33.87f;
    fuXuan.MaxHp.PercentageBonus += 2*0.07343999999999999f + 3*0.432f +.12f+.1f;
    // var finalHp = fuXuan.MaxHp.GetFinalValue();
    // fuXuan.MaxHp.AddStatusEffect(new ConditionalStatusEffect(StatusEffectId.WeaponStatsBuff, ()=>new StatModifier(flatBonus: finalHp*0.06f)));
    fuXuan.Atk.BaseValue += 476;
    fuXuan.Atk.PercentageBonus += 7.344f / 100;
    fuXuan.Atk.FlatBonus += 352.8f + 19.051877f;
    fuXuan.Def.BaseValue += 595;
    fuXuan.Def.FlatBonus += 35.986877f + 16.935f;
    fuXuan.Def.PercentageBonus += 0.15660000000000002f + 0.1836f + .24f;
    var newFuXuanSpeed = new Stat(fuXuan.Speed);
    // newFuXuanSpeed.FlatBonus += 25.032f;
    newFuXuanSpeed.PercentageBonus += 0.06f;
    newFuXuanSpeed.FlatBonus += 4 + 2 + 8 + 7.2f + 0.3f*3;
    fuXuan.Speed = newFuXuanSpeed;
}
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
Console.WriteLine();
Console.WriteLine($"Fu Xuan HP   : {fuXuan.MaxHp.GetFinalValue()}");
Console.WriteLine($"Fu Xuan ATK  : {fuXuan.Atk.GetFinalValue()}");
Console.WriteLine($"Fu Xuan DEF  : {fuXuan.Def.GetFinalValue()}");
Console.WriteLine($"Fu Xuan SPD  : {fuXuan.Speed.GetFinalValue()}");