项目 | 内容
---|---
标题 | JerryFsm
目录 | Unity/AI
标签 | fsm、状态机、JerryFsm、AI
备注 | 无
最近更新 | 2017-01-01 22:45:34

## 总述

状态机，类似于PlayMaker

- Action 行为，一般作为State的子节点，不是必要的
    - 功能
        - CurState
        - Action可以有一些是通用的，不对具体State或者Fsm依赖
    - 接口
        - MyState 当Action属于SubFsm时MyState为空
        - MyFsm
        - MyAIMgr
        - OnEnter() 进入
        - OnUpdate() 更新
        - OnExit() 退出
        - Finish()
- Transition 转换条件，不是必要，按需要可以自己用`Fsm.ChangeState`跳转
    - 功能
        - State
        - NextID
    - 接口
        - MyState 当Transition属于SubFsm时MyState为空
        - MyFsm
        - MyAIMgr
        - NextID 下一个状态的ID 
        - Check() 检查条件是否满足 
- State 状态，可以充当Action
    - 功能
        - Fsm 包含Fsm主要是方便使用一些公用的信息和函数
        - 维护Action列表
        - 维护个性特征，例如：路径
    - 接口
        - ID 获取ID
        - MyFsm
        - MyAIMgr
        - SetActionSequnce(bool sequnce)
        - AddTransition(Transition t)
        - AddAction(Action a)
        - OnEnter() 进入
        - OnUpdate() 更新
        - OnExit() 退出
        - OnDraw()
        - OnDrawSelected()
- SubFsm 子状态机，为了管理局部State而诞生
    - 功能
        - 拥有Fsm和State的大部分功能
        - 底下的State或者SubFsm更新前，会先更新当前SubFsm
    - 接口
        - MyFsm
        - MyAIMgr
        - SetActionSequnce(bool sequnce)
        - AddSubFsm(SubFsm subFsm)
        - AddState(State state)
        - AddTransition(Transition t)
        - AddAction(Action a)
        - OnEnter() 进入
        - OnUpdate() 更新
        - OnExit() 退出
        - OnDraw()
        - OnDrawSelected() 
- Fsm 管理驱动State
    - 功能
        - 负责转换状态（ChangeState）
        - 维护状态列表（StateList）
        - 维护共有特征，例如：路径
        - 持有AIMgr，为了方便State拿到Transform和使用协程
            - 跨脚本启动协程不能用`methodName`的方式，这样`stop`就有问题了，协程还是用`TaskManager`吧
    - 接口
        - Running
        - MyAIMgr
        - m_DoDraw
        - m_DoDrawSelected
        - Pause()
        - Resume()
        - AddSubFsm(SubFsm subFsm)
        - AddState(State state)
        - ChangeState(int stateID)
        - OnStart()
        - OnUpdate()
        - OnReume()
        - OnPause()
        - OnDraw()
        - OnDrawSelected()
- AIMgr 构建和驱动Fsm，继承`MonoBehaviour`
    - 功能
        - Fsm
    - 接口
        - CurFsm
        - MakeFsm() 构建        
        - StartFsm() 开始
        - PauseFsm() 暂停
        - ResumeFsm()
        - OnStart() 初始化，执行构建
        - OnUpdate() 更新
        - OnDraw()
        - OnDrawSelected()
        - GetGraph 导出关系图

> 备注：数据在AIMgr收集分配，可能存储于Fsm或State，看具体的需求，例如路径：
> 
> 如果这一类对象行为（Fsm）有2种，一种需要路径，一种不需要路径，路径是个性特征，数据可以放在具体的State，从AIMgr传入，Fsm是可以共用的。
>
> 如果这一类对象的多种行为（Fsm）都要用到路径，路径是共有特征，可以放到Fsm。

关系图：

```
digraph JerryFsm {
State[shape = box, color = blue]
GameObject->AIMgr
AIMgr->Fsm
Fsm->SubFsm
Fsm->State
SubFsm->SubFsm
SubFsm->State
SubFsm->State[label = "Transition"]
SubFsm->Action
State->State[label = "Transition"]
State->Action
Action->Action
}
```

![image](http://odk2uwdl8.bkt.clouddn.com/2016-07-26-jerry-fsm_00.png)

## 衍化过程

**最初**

```
digraph JerryFsm {
State1[shape = box, color = blue]
State2[shape = box, color = blue]
Fsm->State1
Fsm->State2
State1->State2
State2->State1
}
```

![image](http://odk2uwdl8.bkt.clouddn.com/2016-07-26-jerry-fsm_01.png)

**为了丰富和共用State的行为引入Action**

```
digraph JerryFsm {
State1[shape = box, color = blue]
State2[shape = box, color = blue]
Fsm->State1
Fsm->State2
State1->State2
State2->State1
State1->Action1
State1->Action2
}
```

![image](http://odk2uwdl8.bkt.clouddn.com/2016-07-26-jerry-fsm_02.png)

**为了丰富和共用跳转条件引入Trasition**

```
digraph JerryFsm {
State1[shape = box, color = blue]
State2[shape = box, color = blue]
Fsm->State1
Fsm->State2
State1->State2[label = "Transition1"]
State2->State1[label = "Transition1"]
}
```

![image](http://odk2uwdl8.bkt.clouddn.com/2016-07-26-jerry-fsm_03.png)

**为了可以监管局部引入SubFsm**

```
digraph JerryFsm {
State1[shape = box, color = blue]
State2[shape = box, color = blue]
State3[shape = box, color = blue]
Fsm->State1
Fsm->State2
Fsm->SubFsm1
State1->State2[label = "tr"]
State2->State1[label = "tr"]
SubFsm1->State2[label = "tr"]
SubFsm1->State3
}
```

![image](http://odk2uwdl8.bkt.clouddn.com/2016-07-26-jerry-fsm_04.png)

## 使用

新建：（根据衍化过程，后期增加的都不是必须的）
- AIMgr，推荐命名，`XXXAIMgr_YYY`，如：`MonsterAIMgr_Run`
- Fsm，推荐命名`XXXFsm`或`XXXFsm_YYY`，如：`MonsterFsm`
- SubFsm
- States
    - `XXXState_ZZZ`，如：`MonsterState_FollowPlayer`
- Actions
    - `XXXAction_ZZZ`，如：`MonsterAction_Input`
- Transitions
    - `XXXTr_YYY_ZZZ2ZZZ`或`XXXTr_ZZZ2ZZZ`或`XXXTr_Any2ZZZ`
        - 如：`MonsterTr_Run_Idle2FollowPlayer`
- StateID的枚举可以放在Fsm或AIMgr里面，也可以放在外部

## 备注

已经启动的State和Action，Exit一定有执行，也就是Enter和Exit成对，可以保证做必要的清理

## 样例

### SubFsmTest

![image](http://odk2uwdl8.bkt.clouddn.com/2016-07-26-jerry-fsm_05.png)

## TODO

- [ ] 群组AI
- [x] Transition可以去掉？
    - 不去，这样State可以复用，State同，条件不同
- [x] 多个Action（包括State）同时满足跳转条件，应该只执行第一个，后续的该状态所有逻辑终止（除了Exit）
- [x] AIMgr和Fsm可以添加Action？
    - 不要 
- [x] 训练模式可以点击，类似于这种需求，需要在每一个子节点加监听，能否有局部父节点？
    - 增加了SubFsm
- [ ] GetGraph需要补充SubFsm

## 细节备忘

### State跳转到自己

一个状态跳转到了它自己，可能流程如下：
- update_s
- 1...
- changeState_s
- exit
- running=false
- enter
- running=true
- changeState_e
- 2...
- update_e

要注意`2...`部分是属于上一个状态的，应该跳过，这些情况发生在会跳转的地方，Start/Update/Exit

解决方案：加了一个IsReadyToEnter，转到同一个状态，延后一帧Enter