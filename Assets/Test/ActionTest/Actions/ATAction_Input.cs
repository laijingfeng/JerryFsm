using UnityEngine;
using Jerry;

public class ATAction_Input : Action
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("ATAction_Input Enter");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("ATAction_Input Tri");
            MyFsm.ChangeState(ATFsm.StateID.Idle1.GetHashCode());
            //Finish();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("ATAction_Input Exit");
    }
}