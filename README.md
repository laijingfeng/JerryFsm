��Ŀ | ����
---|---
���� | fsm
Ŀ¼ | Unity/AI
��ǩ | fsm��״̬����JerryFsm��AI
��ע | ��
������� | 2016-12-29 01:21:59

## ����

״̬����������PlayMaker

- Action ��Ϊ��State���ӽڵ㣬���Ǳ�Ҫ��
    - ����
        - CurState
        - Action������һЩ��ͨ�õģ����Ծ���State����Fsm����
    - �ӿ�
        - Reset() ����״̬
        - Enter() ����
        - Update() ����
        - Exit() �˳�
- Transition ת������
    - ����
        - State
        - NextID
    - �ӿ�
        - NextID ��һ��״̬��ID 
        - Check() ��������Ƿ����� 
- State ״̬
    - ����
        - Fsm ����Fsm��Ҫ�Ƿ���ʹ��һЩ���õ���Ϣ�ͺ���
        - ά��Action�б�
        - ά���������������磺·��
        - ע�⣺���е�Actionִ���꣬State�Ż����Լ���Transition
            - Ҳ����State�൱���Դ�һ��Action�����ִ��
    - �ӿ�
        - ID ��ȡID
        - CurFsm
        - Draw() ����
        - SetActionSequnce(bool sequnce)
        - AddTransition(Transition t)
        - AddAction(Action a)
        - Enter() ����
        - Update() ����
        - Exit() �˳�
- Fsm ��������State
    - ����
        - ����ת��״̬��ChangeState��
        - ά��״̬�б�StateList��
        - ά���������������磺·��
        - ����AIMgr��Ϊ�˷���State�õ�Transform��ʹ��Э��
            - ��ű�����Э�̲�����`methodName`�ķ�ʽ������`stop`���������ˣ�Э�̻�����`TaskManager`��
    - �ӿ�
        - Running
        - m_DoDraw
        - m_DoDrawSelected
        - AddState(State state)
        - ChangeState(int stateID)
- AIMgr ����������Fsm���̳�`MonoBehaviour`
    - ����
        - Fsm
    - �ӿ�
        - Start() ��ʼ����ִ�й���
        - MakeFsm() ����
        - Update() ����
        - StartFsm() ��ʼ
        - StopFsm() ��ͣ
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

## ʹ��

�½���
- AIMgr���Ƽ�������`XXXAIMgr_YYY`���磺`MonsterAIMgr_Run`
- Fsm���Ƽ�����`XXXFsm`��`XXXFsm_YYY`���磺`MonsterFsm`
    - StateID��ö��Ҳ��������
- States
    - `XXXState_ZZZ`���磺`MonsterState_FollowPlayer`
- Actions
    - `XXXAction_ZZZ`���磺`MonsterAction_Input`
- Transitions
    - `XXXTr_YYY_ZZZ2ZZZ`��`XXXTr_ZZZ2ZZZ`��`XXXTr_Any2ZZZ`
        - �磺`MonsterTr_Run_Idle2FollowPlayer`

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