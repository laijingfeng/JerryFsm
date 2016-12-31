using UnityEngine;
using Jerry;

public class ATAction_Input2 : Action
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ATAction_Input2 Enter");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("ATAction_Input2 Tri");
            MyState.MyFsm.ChangeState(ATFsm.StateID.Idle1.GetHashCode());
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("ATAction_Input2 Exit");
    }
}