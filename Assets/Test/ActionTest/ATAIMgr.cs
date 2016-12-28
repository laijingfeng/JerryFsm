using Jerry;

public class ATAIMgr : AIMgr
{
    public override void Start()
    {
        base.Start();
        StartFsm();
    }

    public override void MakeFsm()
    {
        CurFsm = new ATFsm();

        ATState_Idle idle = new ATState_Idle(ATStateID.Idle.GetHashCode());
        idle.SetSequnceAction(true);
        idle.AddAction(new ATAction_Input());
        idle.AddAction(new ATAction_Input2());
        CurFsm.AddState(idle);

        ATState_Idle1 idle1 = new ATState_Idle1(ATStateID.Idle1.GetHashCode());
        CurFsm.AddState(idle1);
    }
}