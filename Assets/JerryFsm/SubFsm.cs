using System.Collections.Generic;

namespace Jerry
{
    public abstract class SubFsm
    {
        private bool m_Running;
        /// <summary>
        /// 运行中，内部调用
        /// </summary>
        public bool Running { get { return m_Running; } }

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
            m_States = new List<State>();
            m_SubFsms = new List<SubFsm>();
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

        public void AddSubFsm(SubFsm subFsm)
        {
            if (subFsm == null)
            {
                return;
            }

            subFsm.SetSubFsm(this);

            if (m_SubFsms.Contains(subFsm) == false)
            {
                m_SubFsms.Add(subFsm);
            }
        }

        public void AddState(State state)
        {
            if (state == null)
            {
                return;
            }

            state.SetSubFsm(this);

            if (m_States.Contains(state) == false)
            {
                m_States.Add(state);
            }
        }

        /// <summary>
        /// 检查是否有状态切换，内部调用
        /// </summary>
        /// <param name="stateID"></param>
        /// <returns></returns>
        public bool CheckChangeState(int stateID)
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

            return false;
        }

        /// <summary>
        /// 状态切换，内部调用
        /// </summary>
        public void DoChangeState()
        {
            if (m_CurState != null)
            {
                ExitLastState();
                m_CurState.State_Enter();
                return;
            }

            if(m_CurSubFsm != null)
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
                m_CurSubFsm.DoChangeState();
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
            if (m_CurState != null)
            {
                m_CurState.State_Enter();
            }
            if (m_CurSubFsm != null)
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
    }
}