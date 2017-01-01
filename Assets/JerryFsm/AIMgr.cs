using UnityEngine;

namespace Jerry
{
    public abstract class AIMgr : MonoBehaviour
    {
        protected Fsm CurFsm;

        void Start()
        {
            MakeFsm();
            if (CurFsm != null)
            {
                CurFsm.SetMgr(this);
            }
            OnStart();
        }

        void Update()
        {
            OnUpdate();
            if (CurFsm != null)
            {
                CurFsm.Fsm_Update();
            }
        }

        void OnDrawGizmosSelected()
        {
            OnDrawSelected();
            if (CurFsm != null)
            {
                CurFsm.Fsm_DrawSelected();
            }
        }

        void OnDrawGizmos()
        {
            OnDraw();
            if (CurFsm != null)
            {
                CurFsm.Fsm_Draw();
            }
        }

        public virtual void OnStart() { }
        public virtual void OnUpdate() { }
        public virtual void OnDraw() { }
        public virtual void OnDrawSelected() { }

        public void StartFsm()
        {
            if (CurFsm != null)
            {
                CurFsm.Fsm_Start();
            }
        }

        public void ResumeFsm()
        {
            if (CurFsm != null)
            {
                CurFsm.Resume();
            }
        }

        public void PauseFsm()
        {
            if (CurFsm != null)
            {
                CurFsm.Pause();
            }
        }

        public abstract void MakeFsm();
        
        #region Graph

        public string GetNode()
        {
            return string.Format("{0}[label = \"{1}\"];\n", GetNodeName(), this.GetType());
        }

        public string GetNodeName()
        {
            return string.Format("{0}", this.GetType());
        }

        public string GetNodes()
        {
            string ret = "";
            ret += string.Format("{0}", GetNode());
            ret += string.Format("{0}", CurFsm.GetNodes());
            return ret;
        }

        public string GetLinks()
        {
            string ret = "";
            ret += string.Format("{0}->{1};\n", GetNodeName(), CurFsm.GetNodeName());
            ret += CurFsm.GetLinks();
            return ret;
        }
        
        public string GetGraph()
        {
            string ret = "";
            ret += string.Format("digraph {0} ", GetNodeName()) + "{\n";
            ret += GetNodes();
            ret += GetLinks();
            ret += "}";
            return ret;
        }

        #endregion Graph
    }
}