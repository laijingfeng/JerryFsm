using UnityEngine;
using Jerry;

public class ATState_Idle1 : State
{
    public ATState_Idle1(int id) : base(id) { }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ATState_Idle1 Enter");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("ATState_Idle1 Exit");
    }
}