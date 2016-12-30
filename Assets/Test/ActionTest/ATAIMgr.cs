using Jerry;

public class ATAIMgr : AIMgr
{
    public override void OnStart()
    {
        base.OnStart();
        StartFsm();
    }

    public override void MakeFsm()
    {
        CurFsm = new ATFsm();

        ATState_Idle idle = new ATState_Idle(ATFsm.StateID.Idle.GetHashCode());
        idle.SetSequnceAction(false);
        idle.AddAction(new ATAction_Input());
        idle.AddAction(new ATAction_Input2());
        CurFsm.AddState(idle);

        ATState_Idle1 idle1 = new ATState_Idle1(ATFsm.StateID.Idle1.GetHashCode());
        CurFsm.AddState(idle1);
    }
}