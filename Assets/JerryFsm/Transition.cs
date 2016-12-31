
namespace Jerry
{
    /// <summary>
    /// 转换条件
    /// </summary>
    public abstract class Transition
    {
        /// <summary>
        /// 判条件的时候要用到state的信息
        /// </summary>
        private State m_State;
        protected State MyState { get { return m_State; } }

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

        /// <summary>
        /// 内部调用
        /// </summary>
        /// <param name="s"></param>
        public void SetState(State s)
        {
            m_State = s;
        }

        private int m_NextID;
        public int NextID { get { return m_NextID; } }

        public Transition(int nextID)
        {
            m_NextID = nextID;
        }

        public abstract bool Check();

        #region Graph

        public string GetNextNodeName()
        {
            return string.Format("{0}", NextID);
        }

        public string GetNodeName()
        {
            return string.Format("{0}", this.GetType());
        }

        #endregion Graph
    }
}