
public class AIIntCondition : AIConditionBase
{
    private int value;
    public AIIntCondition(AIConditionType aiConditionType,int intValue) : base(aiConditionType)
    {
        value = intValue;
    }
    protected override bool IsEquals(object obj)
    {
        int targetValue = (int) obj;
        return value == targetValue;
    }
    protected override bool IsNotEquals(object obj)
    {
        int targetValue = (int)obj;
        return value != targetValue;
    }
    protected override bool IsGreater(object obj)
    {
        int targetValue = (int)obj;
        return value > targetValue;
    }
    protected override bool IsLess(object obj)
    {
        int targetValue = (int)obj;
        return value < targetValue;
    }
    protected override bool IsGreaterEquals(object obj)
    {
        int targetValue = (int)obj;
        return value >= targetValue;
    }
    protected override bool IsLessEquals(object obj)
    {
        int targetValue = (int)obj;
        return value >= targetValue;
    }
}
