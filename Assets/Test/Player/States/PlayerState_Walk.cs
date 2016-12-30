﻿using UnityEngine;
using Jerry;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerState_Walk : State
{
    public int frame;

    public PlayerState_Walk(int id) : base(id) { }

    public override void OnEnter()
    {
        base.OnEnter();
        frame = 0;
    }

    public override void OnDraw()
    {
        base.OnDraw();
#if UNITY_EDITOR
        Handles.Label(MyFsm.MyAIMgr.transform.position, "Walk");
#endif
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        frame++;

        PlayerFsm fsm = MyFsm as PlayerFsm;

        if (fsm.path == null || fsm.path.Length <= 0)
        {
            return;
        }

        Vector3 moveDir = fsm.path[fsm.curIdx].position - fsm.MyAIMgr.transform.position;
        if (moveDir.magnitude < 0.1f)
        {
            fsm.curIdx = (fsm.curIdx + 1) % fsm.path.Length;
            moveDir = fsm.path[fsm.curIdx].position - fsm.MyAIMgr.transform.position;
        }
        fsm.MyAIMgr.transform.rotation = Quaternion.LookRotation(moveDir);
        fsm.MyAIMgr.transform.position = fsm.MyAIMgr.transform.position + fsm.MyAIMgr.transform.forward * 0.015f;
    }
}