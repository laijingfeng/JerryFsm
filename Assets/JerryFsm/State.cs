using System.Collections.Generic;

namespace Jerry
{
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

        private Fsm m_Fsm;
        /// <summary>
        /// Fsm
        /// </summary>
        public Fsm MyFsm { get { return m_Fsm; } }

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

        public State(int id)
        {
            m_ID = id;
            m_Transitions = new List<Transition>();
            m_Actions = new List<Action>();
            m_SequnceAction = false;
            m_Running = false;
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
                    }
                }
            }
        }

        /// <summary>
        /// 更新，内部调用
        /// </summary>
        public void State_Update()
        {
            if (m_Fsm == null) { return; }

            if (!m_Running) { return; }
            OnUpdate();

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
                        else
                        {
                            ac.Action_Update();
                        }
                        break;
                    }
                }
            }

            foreach (Transition tr in m_Transitions)
            {
                if (!m_Running) { return; }

                if (tr != null && tr.Check())
                {
                    m_Fsm.ChangeState(tr.NextID);
                    return;
                }
            }
        }

        public void State_Exit()
        {
            m_Running = false;
            foreach (Action ac in m_Actions)
            {
                if (ac.Started == true && ac.Finished == false)
                {
                    ac.Finish();
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

        public void AddAction(Action a)
        {
            if (a == null)
            {
                return;
            }

            if (m_Actions.Contains(a) == false)
            {
                a.SetState(this);
                m_Actions.Add(a);
            }
        }

        public void AddTransition(Transition t)
        {
            if (t == null)
            {
                return;
            }

            if (m_Transitions.Contains(t) == false)
            {
                t.SetState(this);
                m_Transitions.Add(t);
            }
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
            return string.Format("{0}[{1}]", GetNodeName(), this.GetType());
        }

        public string GetNodeName()
        {
            return string.Format("{0}", ID);
        }

        public string GetNodes()
        {
            string ret = "";
            ret += string.Format("{0}\n", GetNode());
            foreach (Action ac in m_Actions)
            {
                ret += string.Format("{0}\n", ac.GetNode());
            }
            return ret;
        }

        public string GetSubGraph()
        {
            string ret = string.Format("subgraph {0}\n", this.GetType());
            ret += string.Format("{0}\n", GetNodeName());
            foreach (Action ac in m_Actions)
            {
                ret += string.Format("{0}\n", ac.GetNodeName());
            }
            if (m_SequnceAction)
            {
                string preName = GetNodeName();
                foreach (Action ac in m_Actions)
                {
                    ret += string.Format("{0}-->{1}\n", preName, ac.GetNodeName());
                    preName = ac.GetNodeName();
                }
            }
            ret += "end\n";
            return ret;
        }

        public string GetLinks()
        {
            string ret = "";
            foreach (Transition tr in m_Transitions)
            {
                ret += string.Format("{0}-->|{1}|{2}\n", GetNodeName(), tr.GetNodeName(), tr.GetNextNodeName());
            }
            return ret;
        }

        #endregion Graph
    }
}