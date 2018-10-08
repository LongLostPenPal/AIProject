using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AIGroup(AI组，通常为一整个功能，如自动挂机、寻路)
/// </summary>
public class AIGroup
{
    public AIGroup(int groupId)
    {
        this.groupId = groupId;
    }
    private int groupId;
    public Dictionary<int,AIActionBase> AIActionDic = new Dictionary<int, AIActionBase>();
    /// <summary>
    /// key 值为 AITransiton 的 _aiActionFromId，其实状态ID ,一个状态 id唯一， 有许多个 AITransition ,都是从自己出发，检测往别的Action 迁移的可能
    /// </summary>
    public Dictionary<int,List<AITransition>> AiTransitionDic = new Dictionary<int, List<AITransition>>(); 
//    使用药水等逻辑更新
//    public List<AIActionBase> AISingleUpdateActionList = new List<AIActionBase>();

}


/// <summary>
/// AI管理器基类
/// 负责统筹管理、提供更新、以及基础接口
/// 被统筹的对象为:
/// AIGroup(AI组，通常为一整个功能，如自动挂机、寻路)
/// AIActionBase(AI行为，一个具体的行为，如自动释放技能、自动走到目标点)
/// AITransition(AI转换器，负责连接各个行为，使行为之间可以转换)
/// AIManger相当于之前写的FSMMgr 里面存着主角对象 这个状态机服务的对象
/// </summary>
public abstract class AIManager : MonoBehaviour
{
    /// <summary>
    /// 所属对象-控制的对象
    /// </summary>
    public BaseEntity entity;
    /// <summary>
    /// 当前所处于的 Group
    /// </summary>
    protected AIGroup currentAIGroup;
    /// <summary>
    /// 当前所处于的 Action
    /// </summary>
    protected AIActionBase currentAIAction=null;
    /// <summary>
    /// 一些对比值存贮，比如挂机起始点，寻路终点位置等
    /// </summary>
    public Dictionary<string,object>  currentValueDic = new Dictionary<string, object>();
    /// <summary>
    /// 所有的 AIGroup
    /// </summary>
    protected Dictionary<int,AIGroup> aiGroupDic = new Dictionary<int, AIGroup>();

    private void Awake()
    {
        entity = GetComponent<BaseEntity>();
    }
    
    public void InitAIManager()
    {
        InitAllAIAction();
        InitAllAITransition();
    }

    /// <summary>
    /// 开启一个AI
    /// </summary>
    /// <param name="aiGroupId"></param>
    /// <param name="aiActionID"></param>
    public void StartAI(int aiGroupId,int aiActionID)
    {
        StopAI();
        //TODO 判断角色死亡

        if (!aiGroupDic.ContainsKey(aiGroupId))
            return;
        currentValueDic.Clear();
        currentAIAction = null;
        currentAIGroup = aiGroupDic[aiGroupId];

        OnAIStart(aiGroupId);

    }

    public void StopAI()
    {
        currentValueDic.Clear();
        currentAIAction = null;
    }
    /// <summary>
    /// 需抽象 由各个 AIManger 自行初始化相关AI状态与迁移
    /// </summary>
    public abstract void InitAllAIAction();
    /// <summary>
    /// 需抽象 由各个 AIManger 自行初始化相关AI状态与迁移
    /// </summary>
    public abstract void InitAllAITransition();

    public void SetCurrentValue(string valueName,object value)
    {
//        if(currentValueDic.ContainsKey(valueName))
//        {
            currentValueDic[valueName] = value;
//        }
//        else
//        {
//            currentValueDic.Add(valueName,value);
//        }
        //https://blog.csdn.net/MAOMAOXIAOHUO/article/details/51598056
    }
    public object GetCurrentValue(string valueName)
    {
        if(currentValueDic.ContainsKey(valueName))
        {
            return currentValueDic[valueName];
        }
        return null;
    }

    /// <summary>
    /// AI是不会自动切换 AIGroup 的 ，用于在同一 Group 中 不同的 Action 之间的切换
    /// </summary>
    public void ChangeAIActionByID(int actionId)
    {
        if (currentAIAction == null || actionId == currentAIAction.acitonId)
            return;

        if (currentAIGroup ==null || !currentAIGroup.AIActionDic.ContainsKey(actionId))return;

        currentAIAction.OnActionEnd();
        currentAIAction = currentAIGroup.AIActionDic[actionId];
        currentAIAction.OnActionStart();

        if (currentAIGroup.AiTransitionDic.ContainsKey(actionId))
        {
            var transitionList = currentAIGroup.AiTransitionDic[actionId];
            for(int i = 0;i < transitionList.Count;i++)
            {
                transitionList[i].OnAiTransitionStart();
            }
        }
    }

    /// <summary>
    /// AI开始 外部统一调用 StartAI ，此处给子类重写使用
    /// </summary>
    /// <param name="groupId"></param>
    protected virtual void OnAIStart(int groupId)
    {
    }
    /// <summary>
    /// AI开始 外部统一调用 StopAI ，此处给子类重写使用
    /// </summary>
    /// <param name="groupId"></param>
    protected virtual void OnAIStop(int groupId)
    {
    }

    /// <summary>
    /// 初始化当前AIManager 的Group
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    protected AIGroup GetAddAIGroup(int groupId)
    {
        if(aiGroupDic.ContainsKey(groupId))
            return aiGroupDic[groupId];
        AIGroup aiGroup = new AIGroup(groupId);
        aiGroupDic.Add(groupId,aiGroup);
        return aiGroup;
    }

    private void Update()
    {
        if(currentAIGroup == null)
            return;

        if(currentAIAction == null)
            return;

        List<AITransition> transitionList = null;
        currentAIGroup.AiTransitionDic.TryGetValue(currentAIAction.acitonId,out transitionList);
        if(transitionList!=null)
        {
            for (int i = 0; i < transitionList.Count; i++)
            {
                //不需要等待当前状态结束
                if (transitionList[i].IsNeedActionOver == false)
                {
                    //迁移成立
                    if (transitionList[i].OnAiTransitionUpdate())
                    {
                        transitionList[i].OnAiTransitionEnd();
                        //改变状态  
                        ChangeAIActionByID(transitionList[i].AiActionToId);
                        return;
                    }    
                }
            }
        }

        //需要等待当前状态结束的迁移 ， 在当前状态没有 Runing 情况下进行判断
        if(currentAIAction.RunningType != AIActionRunningType.Running)
        {
            if(transitionList != null)
            {
                for(int i = 0;i < transitionList.Count;++i)
                {
                    if(transitionList[i].IsNeedActionOver == true)
                    {
                        //每个状态有很多去往不同状态的迁移 满足一个就迁移到改状态
                        if(transitionList[i].OnAiTransitionUpdate())
                        {
                            transitionList[i].OnAiTransitionEnd();
                            ChangeAIActionByID(transitionList[i].AiActionToId);
                            return;
                        }
                    }
                }
            }
        }

        //Update CurrentAction
        if(currentAIAction != null)
        {
            currentAIAction.OnActionUpdate();
        }
    }
    /// <summary>
    /// 往一个AIGroup中添加行为
    /// </summary>
    /// <param name="group">目的AI组</param>
    /// <param name="action">具体行为</param>
    /// <param name="id">行为ID</param>
    protected void AddActionToGroupDic(AIGroup group,AIActionBase action,int id)
    {
        action.OnActionInit(this,id);
        group.AIActionDic.Add(id,action);
    }
    /// <summary>
    /// 向一个AIGroup中添加一个完整的迁移
    /// </summary>
    /// <param name="group">目的AI组</param>
    /// <param name="transition">完整的迁移</param>
    protected void AddTransitionToGroupDic(AIGroup group,AITransition transition)
    {
        if(group.AiTransitionDic.ContainsKey(transition.AiActionFromId)==false)
        {
            group.AiTransitionDic.Add(transition.AiActionFromId,new List<AITransition>());
        }
        group.AiTransitionDic[transition.AiActionFromId].Add(transition);
    }

}
