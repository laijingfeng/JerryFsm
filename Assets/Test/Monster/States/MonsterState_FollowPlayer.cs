using UnityEngine;
using Jerry;

public class MonsterState_FollowPlayer : State
{
    public MonsterState_FollowPlayer(int id) : base(id) { }

    public override void Update()
    {
        base.Update();

        MonsterFsm fsm = MyFsm as MonsterFsm;

        Vector3 moveDir = fsm.Player.position - fsm.MyAIMgr.transform.position;
        if (moveDir.magnitude < 0.1f)
        {
            return;
        }
        fsm.MyAIMgr.transform.rotation = Quaternion.LookRotation(moveDir);
        fsm.MyAIMgr.transform.position = fsm.MyAIMgr.transform.position + fsm.MyAIMgr.transform.forward * 0.01f;
    }
}
