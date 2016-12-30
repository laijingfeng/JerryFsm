using UnityEngine;
using Jerry;

public class ATState_Idle : State
{
    public ATState_Idle(int id) : base(id){}

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ATState_Idle Enter");
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("ATState_Idle Exit");
    }
}