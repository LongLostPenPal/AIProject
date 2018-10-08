

/// <summary>
/// 判断类型
/// </summary>
public enum AIConditionType
{
        Equals,
        NotEquals,
        Greater,
        Less,
        GreaterEquals,
        LessEquals,
}

/// <summary>
/// 条件基类
/// 类似于动画状态机的 条件
/// </summary>
public abstract class AIConditionBase
{

    private AIConditionType aiCnditionType;

    public AIConditionBase(AIConditionType aiConditionType)
    {
        this.aiCnditionType = aiConditionType;
    }

    /// <summary>
    /// 外部统一调用的判定方法
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool IsSuccess(object obj)
    {
        switch (aiCnditionType)
        {
                case AIConditionType.Equals:
                return IsEquals(obj);
                case AIConditionType.NotEquals:
                return IsNotEquals(obj);
                case AIConditionType.Greater:
                return IsGreater(obj);
                case AIConditionType.Less:
                return IsLess(obj);
                case AIConditionType.GreaterEquals:
                return IsGreaterEquals(obj);
                case AIConditionType.LessEquals:
                return IsLessEquals(obj);
        }
        return false;
    }

    protected abstract bool IsEquals(object obj);
    protected abstract bool IsNotEquals(object obj);
    protected abstract bool IsGreater(object obj);
    protected abstract bool IsLess(object obj);
    protected abstract bool IsGreaterEquals(object obj);
    protected abstract bool IsLessEquals(object obj);


}
