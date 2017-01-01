��Ŀ | ����
---|---
���� | JerryFsm
Ŀ¼ | Unity/AI
��ǩ | fsm��״̬����JerryFsm��AI
��ע | ��
������� | 2017-01-01 22:45:34

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
        - GetGraph ������ϵͼ

> ��ע��������AIMgr�ռ����䣬���ܴ洢��Fsm��State�����������������·����
> 
> �����һ�������Ϊ��Fsm����2�֣�һ����Ҫ·����һ�ֲ���Ҫ·����·���Ǹ������������ݿ��Է��ھ����State����AIMgr���룬Fsm�ǿ��Թ��õġ�
>
> �����һ�����Ķ�����Ϊ��Fsm����Ҫ�õ�·����·���ǹ������������Էŵ�Fsm��

��ϵͼ��

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

## �ܻ�����

**���**

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

**Ϊ�˷ḻ�͹���State����Ϊ����Action**

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

**Ϊ�˷ḻ�͹�����ת��������Trasition**

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

**Ϊ�˿��Լ�ֲܾ�����SubFsm**

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

## ����

### SubFsmTest

![image](http://odk2uwdl8.bkt.clouddn.com/2016-07-26-jerry-fsm_05.png)

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