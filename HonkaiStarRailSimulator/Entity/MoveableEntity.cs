namespace HonkaiStarRailSimulator;

public abstract class MovableEntity
{
    public IOption<TurnSystem> TurnSystem { get; set; } = new None<TurnSystem>();

    public delegate void HSREventHandler<TEntity, TEventArgs>(TEntity sender, IOption<TurnSystem> turnSystem,
        TEventArgs args)
        where TEntity : MovableEntity
        where TEventArgs : EventArgs;

    public class FinishTurnArgs : EventArgs
    {
    }
    public class BeginTurnArgs : EventArgs
    {
    }

    public event HSREventHandler<MovableEntity, FinishTurnArgs> FinishTurnEvent;
    public event HSREventHandler<MovableEntity, BeginTurnArgs> BeginTurnEvent;

    private Stat _speed;

    public Stat Speed
    {
        get => _speed;
        set
        {
            var avOld = ActionValue;
            var spdOld = this._speed.GetFinalValue();
            this._speed = value;
            ActionValue = avOld * spdOld / _speed.GetFinalValue();
        }
    }

    public int Turns { get; protected set; }

    public float ActionValue { get; set; }

    public void ModifySpeed(StatusEffect statusEffect)
    {
        var avOld = ActionValue;
        var spdOld = _speed.GetFinalValue();
        _speed.AddStatusEffect(statusEffect);
        ActionValue = avOld * spdOld / _speed.GetFinalValue();
    }

    public void ActionAdvance(float advancementPercentage)
    {
        ActionValue -= BaseActionValue * advancementPercentage;
        ActionValue = float.Max(ActionValue, 0);
    }

    public float BaseActionValue => CalcBaseActionValue(_speed.GetFinalValue());

    public static float CalcBaseActionValue(float spd)
    {
        return 10000 / spd;
    }

    public MovableEntity(float initialSpd)
    {
        this._speed = new Stat(baseValue: initialSpd);
        this.ActionValue = this.BaseActionValue;
        this.FinishTurnEvent = (_, _, _) => { };
        this.BeginTurnEvent = (_, _, _) => { };
    }

    public virtual void DoAction()
    {
        BeginTurnEvent?.Invoke(this, TurnSystem, new BeginTurnArgs());
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
        FinishTurnEvent?.Invoke(this, TurnSystem, new FinishTurnArgs());
    }

    public override string ToString()
    {
        return "Unnamed";
    }
}