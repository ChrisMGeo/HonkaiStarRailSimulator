namespace HonkaiStarRailSimulator;

public class MovableEntity
{

    public event EventHandler finishTurnEvent;
    public event EventHandler beginTurnEvent;

    private Stat speed;

    public Stat Speed
    {
        get => speed;
        set
        {
            var avOld = ActionValue;
            var spdOld = this.speed.GetFinalValue();
            this.speed = value;
            ActionValue = avOld * spdOld / speed.GetFinalValue();
        }
    }

    public int Turns { get; protected set; } = 0;

    public float ActionValue { get; set; }

    public void ModifySpeed(StatusEffect statusEffect)
    {
        var avOld = ActionValue;
        var spdOld = speed.GetFinalValue();
        speed.AddStatusEffect(statusEffect);
        ActionValue = avOld * spdOld / speed.GetFinalValue();
    }

    public void ActionAdvance(float advancementPercentage)
    {
        ActionValue -= BaseActionValue * advancementPercentage;
        ActionValue = float.Max(ActionValue, 0);
    }

    public float BaseActionValue => CalcBaseActionValue(speed.GetFinalValue());

    public static float CalcBaseActionValue(float spd)
    {
        return 10000 / spd;
    }

    public MovableEntity(float initialSpd)
    {
        this.speed = new Stat(baseValue: initialSpd);
        this.ActionValue = this.BaseActionValue;
        this.finishTurnEvent = (sender, args) => { };
        this.beginTurnEvent = (sender, args) => { };
    }

    public virtual void DoAction()
    {
        beginTurnEvent?.Invoke(this, EventArgs.Empty);
        // Do Nothing
        FinishTurn();
    }

    public virtual void FinishTurn()
    {
        Turns += 1;
        ActionValue = BaseActionValue;
        var spdOld = Speed.GetFinalValue();
        Speed.ExhaustStatusEffects();
        var spdNew = Speed.GetFinalValue();
        var avOld = ActionValue;
        ActionValue = avOld * spdOld / spdNew;
        finishTurnEvent?.Invoke(this, EventArgs.Empty);
    }

    public override string ToString()
    {
        return "Unnamed";
    }
}