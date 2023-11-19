using HonkaiStarRailSimulator;
using HonkaiStarRailSimulator.Characters;
using HonkaiStarRailSimulator.Lightcones;

var ts = new TurnSystem();
var bronya = new Bronya();
{
    bronya.TalentLevel = 10;
    bronya.SkillLevel = 10;
    bronya.UltimateLevel = 10;
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
var momentOfVictory = new MomentOfVictory();
// momentOfVictory.SuperImposition = 5;
fuXuan.Lightcone = Some<Lightcone>.Of(momentOfVictory);
{
    fuXuan.MaxHp.FlatBonus += 705.6f + 33.87f;
    fuXuan.MaxHp.PercentageBonus += 0.07343999999999999f + 3*0.432f +.12f + .18f;
    // var finalHp = fuXuan.MaxHp.GetFinalValue();
    // fuXuan.MaxHp.AddStatusEffect(new ConditionalStatusEffect(StatusEffectId.WeaponStatsBuff, ()=>new StatModifier(flatBonus: finalHp*0.06f)));
    fuXuan.Atk.PercentageBonus += 7.344f / 100 + 3.888f/100;
    fuXuan.Atk.FlatBonus += 352.8f;
    fuXuan.Def.FlatBonus += 33.87f + 16.935f;
    fuXuan.Def.PercentageBonus += 0.1836f;
    var newFuXuanSpeed = new Stat(fuXuan.Speed);
    // newFuXuanSpeed.FlatBonus += 25.032f;
    newFuXuanSpeed.PercentageBonus += 0.06f;
    newFuXuanSpeed.FlatBonus += 4 + 8 + 7.2f + 7.2f +.3f*6;
    fuXuan.Speed = newFuXuanSpeed;
}
var guinaifen = new Guinaifen();
var goodNightAndSleepWell = new GoodNightAndSleepWell();
guinaifen.Lightcone = Some<Lightcone>.Of(goodNightAndSleepWell);
{
    guinaifen.MaxHp.PercentageBonus += (11.664f)/100;
    guinaifen.MaxHp.FlatBonus+=135.48f+705.6f;
    
    guinaifen.Atk.PercentageBonus += (11.664f+8.208f+43.2f*2+12f+4.3f)/100;
    guinaifen.Atk.FlatBonus += 352.8f + 16.935f+33.87f +21.168754f;
    
    guinaifen.Def.PercentageBonus += (4.32f+5.4f)/100;
    guinaifen.Def.FlatBonus += 19.051877f*2+57.155631f;

    guinaifen.EffectHitRate.FlatBonus += (11.664f*2+10.8f+12.096f+6.912f+10f+4f)/100;
    
    guinaifen.Atk.AddStatusEffect(new ConditionalStatusEffect(StatusEffectId.PermanentStatBuff, ()=>new StatModifier(percentageBonus:.25f*guinaifen.EffectHitRate.GetFinalValue())));
    
    var newGuinaifenSpeed = new Stat(guinaifen.Speed);
    newGuinaifenSpeed.PercentageBonus += 0.06f;
    newGuinaifenSpeed.FlatBonus += 25.032f + 4 + 4 +.3f*3;
    guinaifen.Speed = newGuinaifenSpeed;
}
ts.AddEntity(bronya);
bronya.Target = Some<MovableEntity>.Of(blade);
const int cycles = 7;

for (var i = 0; i < cycles; i++)
{
    ts.RunCycle();
}

ts.Display();

Console.WriteLine($"Blade turns    : {blade.Turns}");
Console.WriteLine($"Blade HP       : {blade.MaxHp.GetFinalValue()}");
Console.WriteLine($"Blade ATK      : {blade.Atk.GetFinalValue()}");
Console.WriteLine($"Blade DEF      : {blade.Def.GetFinalValue()}");
Console.WriteLine($"Blade LVL      : {blade.CharacterLevel.Level}/{blade.CharacterLevel.MaxLevel}");
Console.WriteLine();
Console.WriteLine($"Bronya HP      : {bronya.MaxHp.GetFinalValue()}");
Console.WriteLine($"Bronya ATK     : {bronya.Atk.GetFinalValue()}");
Console.WriteLine($"Bronya DEF     : {bronya.Def.GetFinalValue()}");
Console.WriteLine($"Bronya SPD     : {bronya.Speed.GetFinalValue()}");
Console.WriteLine();
Console.WriteLine($"Fu Xuan HP     : {fuXuan.MaxHp.GetFinalValue()}");
Console.WriteLine($"Fu Xuan ATK    : {fuXuan.Atk.GetFinalValue()}");
Console.WriteLine($"Fu Xuan DEF    : {fuXuan.Def.GetFinalValue()}");
Console.WriteLine($"Fu Xuan SPD    : {fuXuan.Speed.GetFinalValue()}");
Console.WriteLine($"Fu Xuan EHR    : {fuXuan.EffectHitRate.GetFinalValue()}");
Console.WriteLine();
Console.WriteLine($"Guinaifen HP   : {guinaifen.MaxHp.GetFinalValue()}");
Console.WriteLine($"Guinaifen ATK  : {guinaifen.Atk.GetFinalValue()}");
Console.WriteLine($"Guinaifen DEF  : {guinaifen.Def.GetFinalValue()}");
Console.WriteLine($"Guinaifen SPD  : {guinaifen.Speed.GetFinalValue()}");
Console.WriteLine($"Guinaifen EHR  : {guinaifen.EffectHitRate.GetFinalValue()}");