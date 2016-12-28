项目 | 内容
---|---
标题 | fsm
目录 | Unity/AI
标签 | fsm、状态机、JerryFsm、AI
备注 | 无
最近更新 | 2016-12-29 01:21:59

## 介绍

状态机，类似于PlayMaker

- Action 行为，State的子节点，不是必要的
    - 功能
        - CurState
        - Action可以有一些是通用的，不对具体State或者Fsm依赖
    - 接口
        - Reset() 重置状态
        - Enter() 进入
        - Update() 更新
        - Exit() 退出
- Transition 转换条件
    - 功能
        - State
        - NextID
    - 接口
        - NextID 下一个状态的ID 
        - Check() 检查条件是否满足 
- State 状态
    - 功能
        - Fsm 包含Fsm主要是方便使用一些公用的信息和函数
        - 维护Action列表
        - 维护个性特征，例如：路径
        - 注意：所有的Action执行完，State才会检查自己的Transition
            - 也就是State相当于自带一个Action，最后执行
    - 接口
        - ID 获取ID
        - CurFsm
        - Draw() 绘制
        - SetActionSequnce(bool sequnce)
        - AddTransition(Transition t)
        - AddAction(Action a)
        - Enter() 进入
        - Update() 更新
        - Exit() 退出
- Fsm 管理驱动State
    - 功能
        - 负责转换状态（ChangeState）
        - 维护状态列表（StateList）
        - 维护共有特征，例如：路径
        - 持有AIMgr，为了方便State拿到Transform和使用协程
            - 跨脚本启动协程不能用`methodName`的方式，这样`stop`就有问题了，协程还是用`TaskManager`吧
    - 接口
        - Running
        - m_DoDraw
        - m_DoDrawSelected
        - AddState(State state)
        - ChangeState(int stateID)
- AIMgr 构建和驱动Fsm，继承`MonoBehaviour`
    - 功能
        - Fsm
    - 接口
        - Start() 初始化，执行构建
        - MakeFsm() 构建
        - Update() 更新
        - StartFsm() 开始
        - StopFsm() 暂停
        - GetGraph 导出关系图，示例看后面附加

> 备注：数据在AIMgr收集分配，可能存储于Fsm或State，看具体的需求，例如路径：
> 
> 如果这一类对象行为（Fsm）有2种，一种需要路径，一种不需要路径，路径是个性特征，数据可以放在具体的State，从AIMgr传入，Fsm是可以共用的。
>
> 如果这一类对象的多种行为（Fsm）都要用到路径，路径是共有特征，可以放到Fsm。

关系图：

```
graph TB
GameObject
AIMgr
Fsm

State1.Action1[Action1]
State1.ActionN[Action...]
State1.Action2[Action2]
State1

State2.Action1[Action1]
State2.Action2[Action2]
State2.ActionN[Action...]
State2

StateN[State...]

GameObject-->AIMgr
AIMgr-->Fsm
Fsm-->State1

subgraph State1
State1.Action1[Action1]
State1.Action2[Action2]
State1.ActionN[Action...]
State1
State1.Action1-.->State1
State1.Action2-.->State1
State1.ActionN-.->State1
end

subgraph State2
State2.Action1
State2.Action2
State2.ActionN
State2
State2.Action1-->State2.Action2
State2.Action2-->State2.ActionN
State2.ActionN-->State2
end

State1-->|Transition1|State2
State2.Action1-->StateN
```

## 使用

新建：
- AIMgr，推荐命名，`XXXAIMgr_YYY`，如：`MonsterAIMgr_Run`
- Fsm，推荐命名`XXXFsm`或`XXXFsm_YYY`，如：`MonsterFsm`
    - StateID的枚举也放在这里
- States
    - `XXXState_ZZZ`，如：`MonsterState_FollowPlayer`
- Actions
    - `XXXAction_ZZZ`，如：`MonsterAction_Input`
- Transitions
    - `XXXTr_YYY_ZZZ2ZZZ`或`XXXTr_ZZZ2ZZZ`或`XXXTr_Any2ZZZ`
        - 如：`MonsterTr_Run_Idle2FollowPlayer`

## 备注

已经启动的State和Action，Exit一定有执行，也就是Enter和Exit成对，可以保证做必要的清理

## 附录

Graph示例：

```
graph TB
MonsterAIMgr_Run[MonsterAIMgr_Run]
MonsterFsm[MonsterFsm]

2[MonsterState_RunWay]

1[MonsterState_FollowPlayer]

subgraph MonsterState_RunWay
2
end

subgraph MonsterState_FollowPlayer
1
end

MonsterAIMgr_Run-->MonsterFsm
MonsterFsm-->2
2-->|Tr_Run_RunWay2FollowPlay|1
1-->|Tr_Run_FollowPlay2RunWay|2
```