using UnityEngine;
using Jerry;

public class MonsterState_FollowPlayer : State
{
    public MonsterState_FollowPlayer(int id) : base(id) { }

    float time = 0f;
    float dur = 1f;

    public override void OnUpdate()
    {
        base.OnUpdate();

        time += Time.deltaTime;
        if (time > dur)
        {
            time = 0f;
            //Debug.Log("MonsterState_FollowPlayer");
        }
        
        MonsterFsm fsm = MyFsm as MonsterFsm;

        Vector3 moveDir = fsm.Player.position - MyAIMgr.transform.position;
        if (moveDir.magnitude < 0.1f)
        {
            return;
        }
        MyAIMgr.transform.rotation = Quaternion.LookRotation(moveDir);
        MyAIMgr.transform.position = MyAIMgr.transform.position + MyAIMgr.transform.forward * 0.01f;
    }
}