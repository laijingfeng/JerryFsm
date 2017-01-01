��Ŀ | ����
---|---
���� | JerryFsm
Ŀ¼ | Unity/AI
��ǩ | fsm��״̬����JerryFsm��AI
��ע | ��
������� | 2017-01-01 14:09:11

## ����

״̬����������PlayMaker

- Action ��Ϊ��һ����ΪState���ӽڵ㣬���Ǳ�Ҫ��
    - ����
        - CurState
        - Action������һЩ��ͨ�õģ����Ծ���State����Fsm����
    - �ӿ�
        - MyState ��Action����SubFsmʱMyStateΪ��
        - MyFsm
        - MyAIMgr
        - OnEnter() ����
        - OnUpdate() ����
        - OnExit() �˳�
        - Finish()
- Transition ת�����������Ǳ�Ҫ������Ҫ�����Լ���`Fsm.ChangeState`��ת
    - ����
        - State
        - NextID
    - �ӿ�
        - MyState ��Transition����SubFsmʱMyStateΪ��
        - MyFsm
        - MyAIMgr
        - NextID ��һ��״̬��ID 
        - Check() ��������Ƿ����� 
- State ״̬�����Գ䵱Action
    - ����
        - Fsm ����Fsm��Ҫ�Ƿ���ʹ��һЩ���õ���Ϣ�ͺ���
        - ά��Action�б�
        - ά���������������磺·��
    - �ӿ�
        - ID ��ȡID
        - MyFsm
        - MyAIMgr
        - SetActionSequnce(bool sequnce)
        - AddTransition(Transition t)
        - AddAction(Action a)
        - OnEnter() ����
        - OnUpdate() ����
        - OnExit() �˳�
        - OnDraw()
        - OnDrawSelected()
- SubFsm ��״̬����Ϊ�˹���ֲ�State������
    - ����
        - ӵ��Fsm��State�Ĵ󲿷ֹ���
        - ���µ�State����SubFsm����ǰ�����ȸ��µ�ǰSubFsm
    - �ӿ�
        - MyFsm
        - MyAIMgr
        - SetActionSequnce(bool sequnce)
        - AddSubFsm(SubFsm subFsm)
        - AddState(State state)
        - AddTransition(Transition t)
        - AddAction(Action a)
        - OnEnter() ����
        - OnUpdate() ����
        - OnExit() �˳�
        - OnDraw()
        - OnDrawSelected() 
- Fsm ��������State
    - ����
        - ����ת��״̬��ChangeState��
        - ά��״̬�б�StateList��
        - ά���������������磺·��
        - ����AIMgr��Ϊ�˷���State�õ�Transform��ʹ��Э��
            - ��ű�����Э�̲�����`methodName`�ķ�ʽ������`stop`���������ˣ�Э�̻�����`TaskManager`��
    - �ӿ�
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
- AIMgr ����������Fsm���̳�`MonoBehaviour`
    - ����
        - Fsm
    - �ӿ�
        - CurFsm
        - MakeFsm() ����        
        - StartFsm() ��ʼ
        - PauseFsm() ��ͣ
        - ResumeFsm()
        - OnStart() ��ʼ����ִ�й���
        - OnUpdate() ����
        - OnDraw()
        - OnDrawSelected()
        - GetGraph ������ϵͼ��ʾ�������渽��

> ��ע��������AIMgr�ռ����䣬���ܴ洢��Fsm��State�����������������·����
> 
> �����һ�������Ϊ��Fsm����2�֣�һ����Ҫ·����һ�ֲ���Ҫ·����·���Ǹ������������ݿ��Է��ھ����State����AIMgr���룬Fsm�ǿ��Թ��õġ�
>
> �����һ�����Ķ�����Ϊ��Fsm����Ҫ�õ�·����·���ǹ������������Էŵ�Fsm��

��ϵͼ��

```
graph TB
GameObject
AIMgr
Fsm
SubFsm
State((State))
Action
GameObject---AIMgr
AIMgr---Fsm
Fsm-->SubFsm
Fsm-->State
SubFsm-->State
State-->Action
SubFsm-->SubFsm
SubFsm-->Action
Action-.->Action
State-->|Transition|State
SubFsm-->|Transition|State
```

## �ܻ�����

**���**

```
graph TB
Fsm
State1((State1))
State2((State2))
Fsm-->State1
Fsm-.->State2
State1-->State2
State2-->State1
```

**Ϊ�˷ḻ�͹���State����Ϊ����Action**

```
graph TB
Fsm
State1((State1))
State2((State2))
Fsm-->State1
Fsm-.->State2
State1-->State2
State2-->State1
State1-->Action1
State1-->Action2
```

**Ϊ�˷ḻ�͹�����ת��������Trasition**

```
graph TB
Fsm
State1((State1))
State2((State2))
Fsm-->State1
Fsm-.->State2
State1-->|Transition1|State2
State2-->|Transition1|State1
```

**Ϊ�˿��Լ�ֲܾ�����SubFsm**

```
graph TB
Fsm
State1((State1))
State2((State2))
State3((State3))
Fsm-->State1
Fsm-.->SubFsm1
State1-->State2
State2-->State1
SubFsm1-->State2
SubFsm1-.->State3
```

## ʹ��

�½����������ܻ����̣��������ӵĶ����Ǳ���ģ�
- AIMgr���Ƽ�������`XXXAIMgr_YYY`���磺`MonsterAIMgr_Run`
- Fsm���Ƽ�����`XXXFsm`��`XXXFsm_YYY`���磺`MonsterFsm`
- SubFsm
- States
    - `XXXState_ZZZ`���磺`MonsterState_FollowPlayer`
- Actions
    - `XXXAction_ZZZ`���磺`MonsterAction_Input`
- Transitions
    - `XXXTr_YYY_ZZZ2ZZZ`��`XXXTr_ZZZ2ZZZ`��`XXXTr_Any2ZZZ`
        - �磺`MonsterTr_Run_Idle2FollowPlayer`
- StateID��ö�ٿ��Է���Fsm��AIMgr���棬Ҳ���Է����ⲿ

## ��ע

�Ѿ�������State��Action��Exitһ����ִ�У�Ҳ����Enter��Exit�ɶԣ����Ա�֤����Ҫ������

## ��¼

Graphʾ����

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

## ����

### SubFsmTest

```
graph TB
Fsm_Sta1[1]
Sub1_Sta1[2]
Sub1_Sta2[3]
Sub2_Sta1[4]
Sub2_Sta2[5]
Fsm-->Fsm_Sta1
Fsm-.->Sub1
Sub1-->Sub1_Sta1
Sub1_Sta1-->Sub1_Sta1_Ac1
Sub1_Sta1-->Sub1_Sta1_Ac2
Sub1-->Sub1_Sta2
Fsm-.->Sub2
Sub2-->Sub2_Sta1
Sub2-->Sub2_Sta2
Fsm_Sta1-.->Sub1_Sta1
Sub1_Sta1-.->Sub2_Sta2
Sub2_Sta2-.->Sub1_Sta2
Sub1_Sta2-.->Sub2_Sta1
Sub2_Sta1-.->Fsm_Sta1
```

## TODO

- [ ] Ⱥ��AI
- [x] Transition����ȥ����
    - ��ȥ������State���Ը��ã�Stateͬ��������ͬ
- [x] ���Action������State��ͬʱ������ת������Ӧ��ִֻ�е�һ���������ĸ�״̬�����߼���ֹ������Exit��
- [x] AIMgr��Fsm�������Action��
    - ��Ҫ 
- [x] ѵ��ģʽ���Ե��������������������Ҫ��ÿһ���ӽڵ�Ӽ������ܷ��оֲ����ڵ㣿
    - ������SubFsm
- [ ] GetGraph��Ҫ����SubFsm

## ϸ�ڱ���

### State��ת���Լ�

һ��״̬��ת�������Լ��������������£�
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

Ҫע��`2...`������������һ��״̬�ģ�Ӧ����������Щ��������ڻ���ת�ĵط���Start/Update/Exit

�������������һ��IsReadyToEnter��ת��ͬһ��״̬���Ӻ�һ֡Enter