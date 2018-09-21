using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    public Dictionary<int,List<AITransition>> AiTransitionDic = new Dictionary<int, List<AITransition>>(); 
    public List<AIActionBase> AISingleUpdateActionList = new List<AIActionBase>();
    
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
public class AIManager : MonoBehaviour {


}
