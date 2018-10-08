
public class AIBoolCondition:AIConditionBase
{
    private bool targetValue;
    public AIBoolCondition(AIConditionType aiConditionType,bool result) : base(aiConditionType)
    {
        targetValue = result;
    }

    protected override bool IsEquals(object obj)
    {
        bool currentValue = (bool) obj;
        return currentValue == targetValue;
    }

    protected override bool IsNotEquals(object obj)
    {
        bool currentValue = (bool)obj;
        return currentValue != targetValue;
    }

    protected override bool IsGreater(object obj)
    {
        return false;
    }

    protected override bool IsLess(object obj)
    {
        return false;
    }

    protected override bool IsGreaterEquals(object obj)
    {
        return false;
    }

    protected override bool IsLessEquals(object obj)
    {
        return false;
    }
}
