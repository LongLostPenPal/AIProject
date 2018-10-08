
/// <summary>
/// 当前AI状态类型枚举
/// </summary>
public enum AIActionRunningType
{
    Running,//正在执行
    Success,
    Failure,
}

/// <summary>
/// 具体的一个状态 ex：移动、静止、攻击、受击等
/// 类似于动画状态机的 一个状态（连着几条线(AIConnectorBae)到其他状态 条件(AIConnditionBase)满足则迁移）
/// </summary>
public abstract class AIActionBase
{
    public int acitonId;

    protected AIManager aiManager;
    public AIActionRunningType RunningType { get; set; }

    public virtual void OnActionInit(AIManager aiManager,int actionId)
    {
        this.aiManager = aiManager;
        this.acitonId = actionId;
    }

    public virtual void OnActionStart()
    {
        
    }
    public virtual void OnActionUpdate()
    {
        /*
         * 主要在这里使用 aiManager.GetCurrentValue  aiManager.SetCurrentValue  用来检测挂机点等
         * 
         */
    }
    public virtual void OnActionEnd()
    {

    }
}
