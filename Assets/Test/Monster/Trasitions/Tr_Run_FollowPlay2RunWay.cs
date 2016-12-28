﻿using UnityEngine;
using Jerry;

public class Tr_Run_FollowPlay2RunWay : Transition
{
    public Tr_Run_FollowPlay2RunWay(int nextID) : base(nextID) { }

    public override bool Check()
    {
        MonsterFsm fsm = CurState.MyFsm as MonsterFsm;
        if (Vector3.Distance(fsm.Player.position, fsm.MyAIMgr.transform.position) >= 2)
        {
            return true;
        }
        return false;
    }
}