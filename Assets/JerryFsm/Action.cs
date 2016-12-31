
namespace Jerry
{
    public abstract class Action
    {
        private State m_State;
        /// <summary>
        /// State
        /// </summary>
        public State MyState { get { return m_State; } }

        private Fsm m_Fsm;
        public Fsm MyFsm
        {
            get
            {
                if (m_Fsm == null && MyState != null)
                {
                    m_Fsm = MyState.MyFsm;
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

        private bool m_Started;
        /// <summary>
        /// 是否已经开始，内部调用
        /// </summary>
        public bool Started { get { return m_Started; } }

        private bool m_Finished;
        /// <summary>
        /// 是否已经结束，内部调用
        /// </summary>
        public bool Finished { get { return m_Finished; } }

        /// <summary>
        /// 设置State，内部调用
        /// </summary>
        /// <param name="state"></param>
        public void SetState(State state)
        {
            m_State = state;
            m_Finished = false;
            m_Started = false;
        }

        /// <summary>
        /// 重置，内部调用
        /// </summary>
        public void Action_Reset()
        {
            m_Finished = false;
            m_Started = false;
        }

        /// <summary>
        /// 进入，内部调用
        /// </summary>
        public void Action_Enter()
        {
            m_Started = true;
            OnEnter();
        }

        /// <summary>
        /// 更新，内部调用
        /// </summary>
        public void Action_Update()
        {
            if (MyState == null
                || m_Started == false
                || m_Finished == true)
            {
                return;
            }
            OnUpdate();
        }

        /// <summary>
        /// 进入，准备数据
        /// </summary>
        public virtual void OnEnter() { }

        /// <summary>
        /// <para>更新</para>
        /// </summary>
        public virtual void OnUpdate() { }

        /// <summary>
        /// 退出时，做清理
        /// </summary>
        public virtual void OnExit() { }

        /// <summary>
        /// 结束Action
        /// </summary>
        public void Finish()
        {
            m_Finished = true;
            OnExit();
        }

        #region Graph

        public string GetNode()
        {
            return string.Format("{0}>{1}]", GetNodeName(), this.GetType());
        }

        public string GetNodeName()
        {
            return string.Format("{0}.{1}", MyState.GetNodeName(), this.GetType());
        }

        #endregion Graph
    }
}