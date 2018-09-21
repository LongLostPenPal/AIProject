using UnityEngine;

/// <summary>
/// 状态连接器间迁移的检测类型
/// </summary>
public enum AIConditionCheckType
{
    ActionRunningType,
    Bool,
    Int,
    Float,
}

/// <summary>
/// 状态迁移器基类 
/// 状态A 到状态B 之间的迁移类，是否迁移的判断条件为 AIConditionBase
/// </summary>
public abstract class AIConnectorBase
{
    private AIManager aiManager;
    /// <summary>
    /// 此 状态迁移器 用到的对比值
    /// </summary>
    private object connectorResult;
    /// <summary>
    /// 此 状态迁移器 成功的条件
    /// </summary>
    private AIConditionBase aiCondition;
    /// <summary>
    /// 此 状态迁移器 的检测类型
    /// </summary>
    private AIConditionCheckType aiConditionCheckType;
    public AIConnectorBase(AIConditionCheckType aiConditionCheckType)
    {
        this.aiConditionCheckType = aiConditionCheckType;
    }

    /// <summary>
    /// 生成一个 状态迁移器
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="aiCondition"></param>
    /// <returns></returns>
    public AIConnectorBase CreatConnector(AIManager manager,AIConditionBase aiCondition)
    {
        this.aiManager = manager;
        this.aiCondition = aiCondition;
        CheckAndResetCondition();
        OnInitConnector();
        return this;
    }
    /// <summary>
    /// 检测 迁移器 是否可以从本状态前往下一状态
    /// </summary>
    /// <returns></returns>
    public bool CheckCanConnector()
    {
        return aiCondition.IsSuccess(connectorResult);
    }
    protected virtual void OnInitConnector() { }
    public virtual void StartConnector() { }
    /// <summary>
    /// 跟新下状态迁移器最新状态
    /// </summary>
    public abstract void UpdateConnector();
    public virtual void OverConnector() { }


    /// <summary>
    /// 初始化各个 条件判断类 的初始值
    /// </summary>
    public void CheckAndResetCondition()
    {
        switch (aiConditionCheckType)
        {
                case AIConditionCheckType.ActionRunningType:
                break;
                case AIConditionCheckType.Bool:
                break;
                case AIConditionCheckType.Float:
                    CheckConditionIsFloat();
                break;
                case AIConditionCheckType.Int:
                    CheckConditionIsInt();
                break;
        }
    }
    private void CheckConditionIsInt()
    {
        if(aiCondition is AIIntCondition == false)
        {
            Debug.LogError("AI条件类型错误");
            return;
        }
        connectorResult = 0;
    }

    private void CheckConditionIsFloat()
    {
        if(aiCondition is AIFloatCondition == false)
        {
            Debug.LogError("AI条件类型错误");
            return;
        }
        connectorResult = 0.0f;
    }


}
