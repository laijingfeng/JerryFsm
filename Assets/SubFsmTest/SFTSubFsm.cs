using Jerry;
using UnityEngine;

public class SFTSubFsm : SubFsm
{
    public string Flag;
    private Task task;
    private System.Action<Fsm> callback;

    public SFTSubFsm(string flag, System.Action<Fsm> cb = null)
    {
        Flag = flag;
        callback = cb;
    }

    public override void OnEnter()
    {
        JerryDebug.Inst.LogError("Enter " + Flag);
        task = new Task(SFTAIMgr.Print(Flag), true);
    }

    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (callback != null)
            {
                callback(MyFsm);
            }
        }
    }

    public override void OnExit()
    {
        JerryDebug.Inst.LogError("Exit " + Flag);
        if (task != null)
        {
            task.Stop();
            task = null;
        }
    }

    public override void OnDraw()
    {
    }

    public override void OnDrawSelected()
    {
    }
}