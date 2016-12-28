using UnityEngine;

//version: 2016-12-28-02
namespace Jerry
{
    public abstract class AIMgr : MonoBehaviour
    {
        protected Fsm CurFsm;

        public virtual void Start()
        {
            MakeFsm();
            if (CurFsm != null)
            {
                CurFsm.SetMgr(this);
            }
        }

        public void StartFsm()
        {
            if (CurFsm != null)
            {
                CurFsm.Start();
            }
        }

        public void StopFsm()
        {
            if (CurFsm != null)
            {
                CurFsm.Stop();
            }
        }

        public abstract void MakeFsm();
        
        public virtual void Update()
        {
            if (CurFsm != null)
            {
                CurFsm.Update();
            }
        }

        public virtual void OnDrawGizmosSelected()
        {
            if (CurFsm != null)
            {
                CurFsm.DrawSelected();
            }
        }

        public virtual void OnDrawGizmos()
        {
            if (CurFsm != null)
            {
                CurFsm.Draw();
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
            ret += string.Format("{0}\n", GetNode());
            ret += string.Format("{0}", CurFsm.GetNodes());
            return ret;
        }

        public string GetSubGraph()
        {
            return CurFsm.GetSubGraph();
        }

        public string GetLinks()
        {
            string ret = "";
            ret += string.Format("{0}-->{1}\n", GetNodeName(), CurFsm.GetNodeName());
            ret += CurFsm.GetLinks();
            return ret;
        }

        public string GetGraph()
        {
            string ret = "";
            ret += string.Format("graph TB\n");
            ret += GetNodes();
            ret += GetSubGraph();
            ret += GetLinks();
            return ret;
        }

        #endregion Graph
    }
}