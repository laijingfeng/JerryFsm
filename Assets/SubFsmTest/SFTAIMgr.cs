using Jerry;

public class SFTAIMgr : AIMgr
{
    public override void MakeFsm()
    {
        CurFsm = new SFTFsm();
    }

    public override void OnStart()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void OnDraw()
    {
    }

    public override void OnDrawSelected()
    {
    }
}