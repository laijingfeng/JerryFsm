using UnityEngine;
using Jerry;

public class Tr_Idle_Idle2RunWay : Transition
{
    public Tr_Idle_Idle2RunWay(int nextID) : base(nextID) { }

    public override bool Check()
    {
        MonsterFsm fsm = MyState.MyFsm as MonsterFsm;
        if (Vector3.Distance(fsm.Player.position, fsm.MyAIMgr.transform.position) < 2)
        {
            return true;
        }
        return false;
    }
}