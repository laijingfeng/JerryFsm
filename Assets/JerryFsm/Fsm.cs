using System.Collections.Generic;

namespace Jerry
{
    public abstract class Fsm
    {
        private List<State> m_States;

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
            m_States = new List<State>();
        }

        /// <summary>
        /// 内部调用
        /// </summary>
        public void Fsm_Start()
        {
            if (m_CurState == null)
            {
                if (m_States.Count > 0)
                {
                    m_CurState = m_States[0];
                    m_CurState.State_Enter();
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
                m_CurState.State_Update();
            }
        }

        public virtual void OnResume() { }
        public virtual void OnPause() { }
        public virtual void OnStart() { }
        public virtual void OnUpdate() { }
        public virtual void OnDraw() { }
        public virtual void OnDrawSelected() { }

        public void AddState(State state)
        {
            if (state == null)
            {
                return;
            }

            state.SetFsm(this);

            if (m_States.Contains(state) == false)
            {
                m_States.Add(state);
            }
        }

        /// <summary>
        /// 可供外部调用，用来强制跳转
        /// </summary>
        /// <param name="stateID"></param>
        public void ChangeState(int stateID)
        {
            foreach (State state in m_States)
            {
                if (state.ID == stateID)
                {
                    //加if是为了防止在OnExit使用跳转导致死循环
                    if (m_CurState.Running)
                    {
                        m_CurState.State_Exit();
                    }
                    m_CurState = state;
                    m_CurState.State_Enter();
                    break;
                }
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