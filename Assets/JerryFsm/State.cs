using System.Collections.Generic;

namespace Jerry
{
    /// <summary>
    /// <para>状态</para>
    /// </summary>
    public abstract class State
    {
        private List<Transition> m_Transitions;
        private List<Action> m_Actions;
        private bool m_SequnceAction;

        private bool m_Running;
        /// <summary>
        /// 运行中，内部调用
        /// </summary>
        public bool Running { get { return m_Running; } }

        public bool m_ReadyToEnter;

        private SubFsm m_SubFsm;

        private Fsm m_Fsm;
        /// <summary>
        /// Fsm
        /// </summary>
        public Fsm MyFsm
        {
            get
            {
                if (m_Fsm == null && m_SubFsm != null)
                {
                    m_Fsm = m_SubFsm.MyFsm;
                }
                return m_Fsm;
            }
        }

        private AIMgr m_AIMgr;
        public AIMgr MyAIMgr
        {
            get
            {
                if (m_AIMgr == null && MyFsm != null)
                {
                    m_AIMgr = MyFsm.MyAIMgr;
                }
                return m_AIMgr;
            }
        }

        /// <summary>
        /// 设置Action是否是序列Action
        /// </summary>
        /// <param name="sequnce"></param>
        public void SetSequnceAction(bool sequnce)
        {
            m_SequnceAction = sequnce;
        }

        /// <summary>
        /// 设置Fsm，内部调用
        /// </summary>
        /// <param name="fsm"></param>
        public void SetFsm(Fsm fsm)
        {
            m_Fsm = fsm;
        }

        /// <summary>
        /// 设置SubFsm，内部调用
        /// </summary>
        /// <param name="subFsm"></param>
        public void SetSubFsm(SubFsm subFsm)
        {
            m_SubFsm = subFsm;
        }

        public State(int id)
        {
            m_ID = id;
            m_Transitions = null;
            m_Actions = null;
            m_SequnceAction = false;
            m_Running = false;
            m_ReadyToEnter = false;
        }

        private int m_ID;
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get { return m_ID; } }

        /// <summary>
        /// 进入，内部调用
        /// </summary>
        public void State_Enter()
        {
            m_Running = true;
            OnEnter();

            if (m_Running == false) { return; }

            if (m_Actions != null)
            {
                foreach (Action ac in m_Actions)
                {
                    ac.Action_Reset();
                }

                if (m_SequnceAction == false)
                {
                    foreach (Action ac in m_Actions)
                    {
                        if (ac.Started == false)
                        {
                            ac.Action_Enter();
                            if (m_Running == false) { return; }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 更新，内部调用
        /// </summary>
        public void State_Update()
        {
            if (MyFsm == null) { return; }

            if (m_ReadyToEnter)
            {
                m_ReadyToEnter = false;
                State_Enter();
                return;//隔开一帧，因为Enter里面可能继续发跳转变化
            }

            if (m_Running == false) { return; }
            OnUpdate();

            if (m_Actions != null)
            {
                if (m_SequnceAction == false)
                {
                    foreach (Action ac in m_Actions)
                    {
                        if (!m_Running) { return; }
                        if (ac.Finished == false)
                        {
                            ac.Action_Update();
                        }
                    }
                }
                else
                {
                    foreach (Action ac in m_Actions)
                    {
                        if (!m_Running) { return; }
                        if (ac.Finished == false)
                        {
                            if (ac.Started == false)
                            {
                                ac.Action_Enter();
                            }
                            else//Enter和Update隔开一帧，因为Enter可能把它结束了，方便控制状态
                            {
                                ac.Action_Update();
                            }
                            break;
                        }
                    }
                }
            }

            if (m_Transitions != null)
            {
                foreach (Transition tr in m_Transitions)
                {
                    if (!m_Running) { return; }

                    if (tr != null && tr.Check())
                    {
                        MyFsm.ChangeState(tr.NextID);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 退出，内部调用
        /// </summary>
        public void State_Exit()
        {
            m_Running = false;
            if (m_Actions != null)
            {
                foreach (Action ac in m_Actions)
                {
                    if (ac.Started == true && ac.Finished == false)
                    {
                        ac.Finish();
                    }
                }
            }
            OnExit();
        }

        /// <summary>
        /// 进入
        /// </summary>
        public virtual void OnEnter() { }
        /// <summary>
        /// <para>更新</para>
        /// </summary>
        public virtual void OnUpdate() { }
        /// <summary>
        /// 退出时
        /// </summary>
        public virtual void OnExit() { }
        public virtual void OnDraw() { }
        public virtual void OnDrawSelected() { }

        public State AddAction(Action a)
        {
            if (a == null)
            {
                return this;
            }

            if (m_Actions == null)
            {
                a.SetState(this);
                m_Actions = new List<Action>() { a };
            }
            else if (m_Actions.Contains(a) == false)
            {
                a.SetState(this);
                m_Actions.Add(a);
            }
            return this;
        }

        public State AddTransition(Transition t)
        {
            if (t == null)
            {
                return this;
            }

            if (m_Transitions == null)
            {
                t.SetState(this);
                m_Transitions = new List<Transition>() { t };
            }
            else if (m_Transitions.Contains(t) == false)
            {
                t.SetState(this);
                m_Transitions.Add(t);
            }
            return this;
        }

        public void State_Draw()
        {
            OnDraw();
        }

        public void State_DrawSelected()
        {
            OnDrawSelected();
        }

        #region Graph

        public string GetNode()
        {
            string action = string.Empty;
            if (m_Actions != null)
            {
                foreach (Action ac in m_Actions)
                {
                    action += string.Format("|{0}", (m_SequnceAction && string.IsNullOrEmpty(action)) ? "{" : "") + ac.GetNodeName();
                }
                if (m_SequnceAction)
                {
                    action += "}";
                }
            }
            return string.Format("{0} [shape = record, label = \"{1}.{2}{3}\", color = blue];\n", GetNodeName(), this.GetType(), GetNodeName(), action);
        }

        public string GetNodeName()
        {
            return string.Format("{0}", ID);
        }

        public string GetLinks()
        {
            string ret = "";
            if (m_Transitions != null)
            {
                foreach (Transition tr in m_Transitions)
                {
                    ret += string.Format("{0}->{1} [label = \"{2}\"];\n", GetNodeName(), tr.GetNextNodeName(), tr.GetNodeName());
                }
            }
            return ret;
        }

        #endregion Graph
    }
}