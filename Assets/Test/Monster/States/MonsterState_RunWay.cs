using UnityEngine;
using Jerry;

public class MonsterState_RunWay : State
{
    private int curIdx;
    private Transform[] path;

    public MonsterState_RunWay(int id, Transform[] p)
        : base(id)
    {
        path = p;
        curIdx = 0;
    }

    float time = 0f;
    float dur = 1f;

    public override void OnUpdate()
    {
        base.OnUpdate();

        time += Time.deltaTime;
        if (time > dur)
        {
            time = 0f;
            //Debug.Log("MonsterState_RunWay");
        }

        if (path == null || path.Length <= 0)
        {
            return;
        }

        MonsterFsm mgr = MyFsm as MonsterFsm;

        Vector3 moveDir = path[curIdx].position - mgr.MyAIMgr.transform.position;
        if (moveDir.magnitude < 0.1f)
        {
            curIdx = (curIdx + 1) % path.Length;
            moveDir = path[curIdx].position - mgr.MyAIMgr.transform.position;
        }
        mgr.MyAIMgr.transform.rotation = Quaternion.LookRotation(moveDir);
        mgr.MyAIMgr.transform.position = mgr.MyAIMgr.transform.position + mgr.MyAIMgr.transform.forward * 0.01f;
    }
}