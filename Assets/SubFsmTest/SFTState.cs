using Jerry;
using UnityEngine;

public class SFTState : State
{
    public string Flag;
    private Task task;
    private System.Action<Fsm> callback;

    public SFTState(int id, string flag, System.Action<Fsm> cb = null)
        : base(id)
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
        //if (Flag == "Sub1_Sta1")
        //{
        //    if (Input.GetKeyDown(KeyCode.N))
        //    {
        //        if (callback != null)
        //        {
        //            callback(MyFsm);
        //        }
        //    }
        //}
        //else
        //{
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (callback != null)
                {
                    callback(MyFsm);
                }
            }
        //}
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