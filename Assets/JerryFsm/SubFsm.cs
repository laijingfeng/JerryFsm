using System.Collections.Generic;

namespace Jerry
{
    public abstract class SubFsm
    {
        private int m_SubFsmID;
        private static int SubFsmIDGen = 0;

        private bool m_Running;
        /// <summary>
        /// 运行中，内部调用
        /// </summary>
        public bool Running { get { return m_Running; } }

        private List<Transition> m_Transitions;
        private List<Action> m_Actions;
        private bool m_SequnceAction;

        private List<State> m_States;
        private List<SubFsm> m_SubFsms;

        private SubFsm m_LastSubFsm;
        private SubFsm m_CurSubFsm;
        protected SubFsm CurSubFsm { get { return m_CurSubFsm; } }

        private State m_LastState;
        private State m_CurState;
        protected State CurState { get { return m_CurState; } }

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

        public SubFsm()
        {
            m_Running = false;
            m_CurState = null;
            m_CurSubFsm = null;
            m_LastState = null;
            m_LastSubFsm = null;
            m_States = null;
            m_SubFsms = null;
            m_Transitions = null;
            m_Actions = null;
            m_SequnceAction = false;
            m_SubFsmID = SubFsmIDGen++;
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
        /// 设置Action是否是序列Action
        /// </summary>
        /// <param name="sequnce"></param>
        public void SetSequnceAction(bool sequnce)
        {
            m_SequnceAction = sequnce;
        }

        /// <summary>
        /// 设置SubFsm，内部调用
        /// </summary>
        /// <param name="subFsm"></param>
        public void SetSubFsm(SubFsm subFsm)
        {
            m_SubFsm = subFsm;
        }

        public SubFsm AddAction(Action a)
        {
            if (a == null)
            {
                return this;
            }

            if (m_Actions == null)
            {
                a.SetSubFsm(this);
                m_Actions = new List<Action>() { a };
            }
            else if (m_Actions.Contains(a) == false)
            {
                a.SetSubFsm(this);
                m_Actions.Add(a);
            }
            return this;
        }

        public SubFsm AddTransition(Transition t)
        {
            if (t == null)
            {
                return this;
            }

            if (m_Transitions == null)
            {
                t.SetSubFsm(this);
                m_Transitions = new List<Transition>() { t };
            }
            else if (m_Transitions.Contains(t) == false)
            {
                t.SetSubFsm(this);
                m_Transitions.Add(t);
            }
            return this;
        }

        public SubFsm AddSubFsm(SubFsm subFsm)
        {
            if (subFsm == null)
            {
                return subFsm;
            }

            subFsm.SetSubFsm(this);

            if (m_SubFsms == null)
            {
                m_SubFsms = new List<SubFsm>() { subFsm };
            }
            else if (m_SubFsms.Contains(subFsm) == false)
            {
                m_SubFsms.Add(subFsm);
            }
            return subFsm;
        }

        public State AddState(State state)
        {
            if (state == null)
            {
                return state;
            }

            state.SetSubFsm(this);

            if (m_States == null)
            {
                m_States = new List<State>() { state };
            }
            else if (m_States.Contains(state) == false)
            {
                m_States.Add(state);
            }

            return state;
        }

        /// <summary>
        /// 检查是否有状态切换，内部调用
        /// </summary>
        /// <param name="stateID"></param>
        /// <returns></returns>
        public bool CheckChangeState(int stateID)
        {
            if (m_States != null)
            {
                foreach (State state in m_States)
                {
                    if (state.ID == stateID)
                    {
                        m_LastState = m_CurState;
                        m_CurState = state;
                        return true;
                    }
                }
            }
            
            if (m_SubFsms != null)
            {
                foreach (SubFsm subFsm in m_SubFsms)
                {
                    if (subFsm.CheckChangeState(stateID))
                    {
                        if (subFsm != m_CurSubFsm)
                        {
                            m_LastSubFsm = m_CurSubFsm;
                            m_CurSubFsm = subFsm;
                        }
                        return true;
                    }
                }
            }
            
            return false;
        }

        /// <summary>
        /// 状态切换，内部调用
        /// </summary>
        public void DoChangeState()
        {
            if (m_CurState != null)
            {
                if (m_CurState == m_LastState)
                {
                    ExitLastState();
                    m_CurState.m_ReadyToEnter = true;
                }
                else
                {
                    ExitLastState();
                    m_CurState.State_Enter();
                }
                return;
            }

            if (m_CurSubFsm != null)
            {
                ExitLastState(true);
                if (m_CurSubFsm != m_LastSubFsm)
                {
                    if (m_LastSubFsm != null && m_LastSubFsm.Running)
                    {
                        m_LastSubFsm.SubFsm_Exit();
                    }
                    m_LastSubFsm = null;
                    m_CurSubFsm.SubFsm_Enter();
                }
                else
                {
                    m_CurSubFsm.DoChangeState();
                }
            }
        }

        private void ExitLastState(bool onlyState = false)
        {
            if (m_LastState != null && m_LastState.Running)
            {
                m_LastState.State_Exit();
            }
            m_LastState = null;

            if (!onlyState)
            {
                if (m_LastSubFsm != null && m_LastSubFsm.Running)
                {
                    m_LastSubFsm.SubFsm_Exit();
                }
                m_LastSubFsm = null;
            }
        }

        /// <summary>
        /// 进入，内部调用
        /// </summary>
        public void SubFsm_Enter()
        {
            m_Running = true;
            OnEnter();

            if (m_Actions != null)
            {
                if (m_Running == false) { return; }

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
            
            if (m_Running == false) { return; }

            if (m_CurState != null)
            {
                m_CurState.State_Enter();
            }
            else if (m_CurSubFsm != null)
            {
                m_CurSubFsm.SubFsm_Enter();
            }
        }

        /// <summary>
        /// 更新，内部调用
        /// </summary>
        public void SubFsm_Update()
        {
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

            if (!m_Running) { return; }

            if (m_CurState != null)
            {
                m_CurState.State_Update();
            }
            else if (m_CurSubFsm != null)
            {
                m_CurSubFsm.SubFsm_Update();
            }
        }

        /// <summary>
        /// 退出，内部调用
        /// </summary>
        public void SubFsm_Exit()
        {
            m_Running = false;

            if (m_CurState != null && m_CurState.Running)
            {
                m_CurState.State_Exit();
            }
            m_CurState = null;

            if (m_CurSubFsm != null && m_CurSubFsm.Running)
            {
                m_CurSubFsm.SubFsm_Exit();
            }
            m_CurSubFsm = null;

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

        public void SubFsm_Draw()
        {
            OnDraw();
        }

        public void SubFsm_DrawSelected()
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
            return string.Format("{0} [shape = record, label = \"{1}.{2}{3}\", color = green];\n", GetNodeName(), this.GetType(), GetNodeName(), action);
        }

        public string GetNodeName()
        {
            return string.Format("SubFsm{0}", m_SubFsmID);
        }

        public string GetNodes()
        {
            string ret = "";
            ret += string.Format("{0}", GetNode());
            if (m_States != null)
            {
                foreach (State s in m_States)
                {
                    ret += string.Format("{0}", s.GetNode());
                }
            }
            if (m_SubFsms != null)
            {
                foreach (SubFsm sub in m_SubFsms)
                {
                    ret += string.Format("{0}", sub.GetNodes());
                }
            }
            return ret;
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
            if (m_States != null)
            {
                bool fi = true;
                foreach (State s in m_States)
                {
                    ret += string.Format("{0}->{1} [style = {2} color = pink];\n", GetNodeName(), s.GetNodeName(), fi ? "filled" : "dotted");
                    fi = false;
                }
            }
            if (m_SubFsms != null)
            {
                bool fi = true;
                foreach (SubFsm sub in m_SubFsms)
                {
                    ret += string.Format("{0}->{1} [style = {2} color = pink];\n", GetNodeName(), sub.GetNodeName(), fi ? "filled" : "dotted");
                    fi = false;
                }
            }
            if (m_States != null)
            {
                foreach (State s in m_States)
                {
                    ret += string.Format("{0}", s.GetLinks());
                }
            }
            if (m_SubFsms != null)
            {
                foreach (SubFsm sub in m_SubFsms)
                {
                    ret += sub.GetLinks();
                }
            }
            return ret;
        }

        #endregion Graph
    }
}