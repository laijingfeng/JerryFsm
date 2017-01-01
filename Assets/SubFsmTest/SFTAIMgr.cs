using Jerry;
using System.Collections;

public class SFTAIMgr : AIMgr
{
    public override void MakeFsm()
    {
        CurFsm = new SFTFsm();
        CurFsm.AddState(new SFTState(SFTFsm.StateID.S1.GetHashCode(), "Fsm_Sta1", (fsm) =>
        {
            fsm.ChangeState(SFTFsm.StateID.S2.GetHashCode());
        }));

        SFTSubFsm sub1 = new SFTSubFsm("Sub1");
        SFTState sub1_sat1 = (SFTState)sub1.AddState(new SFTState(SFTFsm.StateID.S2.GetHashCode(), "Sub1_Sta1", (fsm) =>
        {
            fsm.ChangeState(SFTFsm.StateID.S5.GetHashCode());
        }));
        sub1_sat1.AddAction(new SFTAction("Sub1_Sta1_Ac1"));
        sub1_sat1.AddAction(new SFTAction("Sub1_Sta1_Ac2"));
        sub1_sat1.SetSequnceAction(true);
        sub1.AddState(new SFTState(SFTFsm.StateID.S3.GetHashCode(), "Sub1_Sta2", (fsm) =>
        {
            fsm.ChangeState(SFTFsm.StateID.S4.GetHashCode());
        }));

        SFTSubFsm sub2 = new SFTSubFsm("Sub2");
        sub2.AddState(new SFTState(SFTFsm.StateID.S4.GetHashCode(), "Sub2_Sta1", (fsm) =>
        {
            fsm.ChangeState(SFTFsm.StateID.S1.GetHashCode());
        }));
        sub2.AddState(new SFTState(SFTFsm.StateID.S5.GetHashCode(), "Sub2_Sta2", (fsm) =>
        {
            fsm.ChangeState(SFTFsm.StateID.S3.GetHashCode());
        }));

        CurFsm.AddSubFsm(sub1);
        CurFsm.AddSubFsm(sub2);
    }

    public override void OnStart()
    {
        JerryDebug.Inst.Set(true, false, false, true, false);
        StartFsm();
    }

    public override void OnUpdate()
    {
    }

    public override void OnDraw()
    {
    }

    public override void OnDrawSelected()
    {
    }

    public static IEnumerator Print(string str)
    {
        while (true)
        {
            JerryDebug.Inst.LogInfo(str);
            yield return Yielders.GetWaitForSeconds(1f);
        }
    }
}