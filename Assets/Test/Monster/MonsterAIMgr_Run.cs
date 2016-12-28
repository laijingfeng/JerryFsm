using UnityEngine;
using Jerry;

public class MonsterAIMgr_Run : AIMgr
{
    public Transform[] path;
    public Transform player;

    public override void Start()
    {
        base.Start();

        StartFsm();
    }

    public override void MakeFsm()
    {
        CurFsm = new MonsterFsm(player);
        CurFsm.m_DoDraw = true;

        MonsterState_RunWay run = new MonsterState_RunWay(MonsterStateID.RunWay.GetHashCode(), path);
        run.AddTransition(new Tr_Run_RunWay2FollowPlay(MonsterStateID.FollowPlayer.GetHashCode()));
        CurFsm.AddState(run);

        MonsterState_FollowPlayer follow = new MonsterState_FollowPlayer(MonsterStateID.FollowPlayer.GetHashCode());
        follow.AddTransition(new Tr_Run_FollowPlay2RunWay(MonsterStateID.RunWay.GetHashCode()));
        CurFsm.AddState(follow);
    }
}