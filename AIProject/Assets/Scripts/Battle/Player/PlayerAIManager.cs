using System.Collections.Generic;

public enum PlayerAIGroup
{
    AutoFindPath,//自动寻路
    GuaJi,//挂机
}

public  class PlayerAIManager:AIManager
{
    public override void InitAllAIAction()
    {
    //寻路组 行为添加
       AIGroup aiGroup= GetAddAIGroup((int) PlayerAIGroup.AutoFindPath);
       AIActionBase aiAction = new AIPlayerFindPath();
       //类似于 在动画状态机界面 添加一个新的状态，id为1
       AddActionToGroupDic(aiGroup,aiAction,1);
        aiAction = new AIPlayerFindPathOver();
        //类似于 在动画状态机界面 添加一个新的状态，id为2
        AddActionToGroupDic(aiGroup,aiAction,2);

        //挂机组 行为添加
//        aiGroup = GetAddAIGroup((int)PlayerAIGroup.GuaJi);

    }

    public override void InitAllAITransition()
    {
        //寻路组 迁移(一个完整的迁移包括前后状态ID，以及一组迁移器) 添加
        AIGroup aiGroup = GetAddAIGroup((int)PlayerAIGroup.AutoFindPath);
        List<AIConnectorBase> connectorList = new List<AIConnectorBase>();
        //创建一组迁移器（一个迁移器用有一个迁移条件），添加 【检查是否到达目标点】 的迁移器 并以条件 [等于真] 进行初始化
        connectorList.Add(new AICheckIsReachFindPath().InitConnector(new AIBoolCondition(AIConditionType.Equals,true)));
        //创建一个完整的迁移：上面的迁移器成立 则状态进行 1=>2 的转变
        AITransition aiTransition = new AITransition(this,1,2,connectorList);
        AddTransitionToGroupDic(aiGroup,aiTransition);

        //挂机组 行为添加
    }

    
}
