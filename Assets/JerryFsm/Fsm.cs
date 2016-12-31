using System.Collections.Generic;

namespace Jerry
{
    public abstract class Fsm
    {
        private List<State> m_States;
        private List<SubFsm> m_SubFsms;

        private SubFsm m_CurSubFsm;
        protected SubFsm CurSubFsm { get { return m_CurSubFsm; } }

        private State m_CurState;
        protected State CurState { get { return m_CurState; } }

        private AIMgr m_AIMgr;
        public AIMgr MyAIMgr { get { return m_AIMgr; } }

        /// <summary>
        /// 运行中
        /// </summary>
        private bool m_Running;
        public bool Running { get { return m_Running; } }

        public bool m_DoDraw;
        public bool m_DoDrawSelected;

        /// <summary>
        /// 设置Mgr，内部调用
        /// </summary>
        /// <param name="aiMgr"></param>
        public void SetMgr(AIMgr aiMgr)
        {
            m_AIMgr = aiMgr;
        }

        public Fsm()
        {
            m_Running = false;
            m_DoDraw = false;
            m_DoDrawSelected = false;
            m_CurState = null;
            m_CurSubFsm = null;
            m_States = new List<State>();
            m_SubFsms = new List<SubFsm>();
        }

        /// <summary>
        /// 内部调用
        /// </summary>
        public void Fsm_Start()
        {
            if (m_States.Count > 0)
            {
                m_CurState = m_States[0];
                m_CurState.State_Enter();
            }
            else
            {
                if (m_SubFsms.Count > 0)
                {
                    m_CurSubFsm = m_SubFsms[0];
                    m_CurSubFsm.SubFsm_Enter();
                }
            }

            m_Running = true;
            OnStart();
        }

        public void Resume()
        {
            m_Running = true;
            OnResume();
        }

        public void Pause()
        {
            m_Running = false;
            OnPause();
        }

        /// <summary>
        /// 更新，内部调用
        /// </summary>
        public void Fsm_Update()
        {
            if (m_Running == false) { return; }

            OnUpdate();

            if (m_CurState != null)
            {
                if (m_Running == false) { return; }
                m_CurState.State_Update();
            }

            if (m_CurSubFsm != null)
            {
                if (m_Running == false) { return; }
                m_CurSubFsm.SubFsm_Update();
            }
        }

        public virtual void OnResume() { }
        public virtual void OnPause() { }
        public virtual void OnStart() { }
        public virtual void OnUpdate() { }
        public virtual void OnDraw() { }
        public virtual void OnDrawSelected() { }

        public SubFsm AddSubFsm(SubFsm subFsm)
        {
            if (subFsm == null)
            {
                return subFsm;
            }

            subFsm.SetFsm(this);

            if (m_SubFsms.Contains(subFsm) == false)
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

            state.SetFsm(this);

            if (m_States.Contains(state) == false)
            {
                m_States.Add(state);
            }
            return state;
        }

        /// <summary>
        /// 可供外部调用，用来强制跳转
        /// </summary>
        /// <param name="stateID"></param>
        public void ChangeState(int stateID)
        {
            JerryDebug.Inst.LogWarn("ChangeState " + (SFTFsm.StateID)stateID);

            foreach (State state in m_States)
            {
                if (state.ID == stateID)
                {
                    ExitLastState();
                    m_CurState = state;
                    m_CurState.State_Enter();
                    return;
                }
            }

            foreach (SubFsm subFsm in m_SubFsms)
            {
                if (subFsm.CheckChangeState(stateID))
                {
                    ExitLastState(true);
                    if (subFsm != m_CurSubFsm)
                    {
                        JerryDebug.Inst.LogWarn("ddddxx");
                        if (m_CurSubFsm != null && m_CurSubFsm.Running)
                        {
                            JerryDebug.Inst.LogWarn("dddd");
                            m_CurSubFsm.SubFsm_Exit();
                        }

                        m_CurSubFsm = subFsm;
                        m_CurSubFsm.SubFsm_Enter();
                    }
                    else
                    {
                        JerryDebug.Inst.LogWarn("ddddxxdddd " + (m_CurSubFsm as SFTSubFsm).Flag + (subFsm as SFTSubFsm).Flag);
                        m_CurSubFsm.DoChangeState();
                    }
                    return;
                }
            }
        }

        private void ExitLastState(bool onlyState = false)
        {
            if (m_CurState != null && m_CurState.Running)
            {
                m_CurState.State_Exit();
            }
            m_CurState = null;

            if (!onlyState)
            {
                if (m_CurSubFsm != null && m_CurSubFsm.Running)
                {
                    m_CurSubFsm.SubFsm_Exit();
                }
                m_CurSubFsm = null;
            }
        }

        public void Fsm_DrawSelected()
        {
            if (m_DoDrawSelected == false)
            {
                return;
            }
            OnDrawSelected();
            if (m_CurState != null)
            {
                m_CurState.State_DrawSelected();
            }
            if (m_CurSubFsm != null)
            {
                m_CurSubFsm.SubFsm_DrawSelected();
            }
        }

        public void Fsm_Draw()
        {
            if (m_DoDraw == false)
            {
                return;
            }
            OnDraw();
            if (m_CurState != null)
            {
                m_CurState.State_Draw();
            }
            if (m_CurSubFsm != null)
            {
                m_CurSubFsm.SubFsm_Draw();
            }
        }

        #region Graph

        public string GetNode()
        {
            return string.Format("{0}[{1}]", GetNodeName(), this.GetType());
        }

        public string GetNodeName()
        {
            return string.Format("{0}", this.GetType());
        }

        public string GetNodes()
        {
            string ret = "";
            ret += string.Format("{0}\n\n", GetNode());
            foreach (State s in m_States)
            {
                ret += string.Format("{0}\n", s.GetNodes());
            }
            return ret;
        }

        public string GetSubGraph()
        {
            string ret = "";
            foreach (State s in m_States)
            {
                ret += string.Format("{0}\n", s.GetSubGraph());
            }
            return ret;
        }

        public string GetLinks()
        {
            string ret = "";
            foreach (State s in m_States)
            {
                ret += string.Format("{0}-->{1}\n", GetNodeName(), s.GetNodeName());
                break;
            }
            foreach (State s in m_States)
            {
                ret += string.Format("{0}", s.GetLinks());
            }
            return ret;
        }

        #endregion Graph
    }
}