﻿using Jerry;
using UnityEngine;
using System.Collections;

public class MonsterState_Idle : State
{
    public MonsterState_Idle(int id) : base(id) { }

    private Task _task;

    public override void OnEnter()
    {
        base.OnEnter();
        //Debug.LogWarning("Enter");
        _task = new Task(this.IE_Idle());
    }

    public override void OnExit()
    {
        base.OnExit();
        if (_task != null)
        {
            _task.Stop();
        }
        //Debug.LogWarning("Exit");
    }

    private IEnumerator IE_Idle()
    {
        while (true)
        {
            //Debug.Log("idle");
            yield return new WaitForSeconds(0.5f);
        }
    }
}