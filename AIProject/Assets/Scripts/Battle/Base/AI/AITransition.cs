using System.Collections.Generic;

/// <summary>
/// 一个完整的状态迁移，包括前状态，后状态，中间的N个迁移器AIConnectorBase
/// </summary>
public class AITransition
{
    private AIManager manager;
    private int _aiActionFromId;
    private int _aiActionToId;
    /// <summary>
    /// N个迁移器
    /// </summary>
    private List<AIConnectorBase> aiConnectorList;
    /// <summary>
    /// 防止多次重复开启
    /// </summary>
    private bool isStarted = false;
    public AITransition(AIManager aimanager,int aiActionFromId,int aiActionToId,List<AIConnectorBase> aiConnectorList)
    {
        this.manager = aimanager;
        this._aiActionFromId = aiActionFromId;
        this._aiActionToId = aiActionToId;
        this.aiConnectorList = aiConnectorList;
    }

    public void OnAiTransitionStart()
    {
        if (isStarted)
            return;
        isStarted = true;
        for (int i = 0; i < aiConnectorList.Count; i++)
        {
            aiConnectorList[i].StartConnector();
        }
    }
    /// <summary>
    /// 此 迁移 是否成立
    /// </summary>
    /// <returns></returns>
    public bool OnAiTransitionUpdate()
    {
        if (aiConnectorList.Count<=0)
        {
            return false;
        }
        for (int i = 0; i < aiConnectorList.Count; i++)
        {
            //更新状态
            aiConnectorList[i].UpdateConnector();
            //判断迁移器 迁移条件是否成立
            if (aiConnectorList[i].CheckCanConnector() == false)
            {
                return false;
            }
        }
        //如果迁移成立  重置下各个迁移状态
        for(int i = 0;i < aiConnectorList.Count;i++)
        {
            aiConnectorList[i].CheckAndResetCondition();
        }
        return true;
    }

    public void OnAiTransitionEnd()
    {
        isStarted = false;
        for(int i = 0;i < aiConnectorList.Count;i++)
        {
            aiConnectorList[i].OverConnector();
        }
    }

}
