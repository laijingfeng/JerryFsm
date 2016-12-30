using UnityEngine;
using Jerry;

public class PlayerAIMgr_Move : AIMgr
{
    public Transform[] path;

    public override void OnStart()
    {
        base.OnStart();

        StartFsm();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (CurFsm.Running)
            {
                PauseFsm();
            }
            else
            {
                StartFsm();
            }
        }
    }

    public override void MakeFsm()
    {
        CurFsm = new PlayerFsm(path);
        CurFsm.m_DoDrawSelected = true;

        PlayerState_Walk walk = new PlayerState_Walk(PlayerStateID.Walk.GetHashCode());
        walk.AddTransition(new Tr_Move_Walk2Run(PlayerStateID.Run.GetHashCode()));
        CurFsm.AddState(walk);

        PlayerState_Run run = new PlayerState_Run(PlayerStateID.Run.GetHashCode());
        run.AddTransition(new Tr_Move_Run2Walk(PlayerStateID.Walk.GetHashCode()));
        CurFsm.AddState(run);
    }
}