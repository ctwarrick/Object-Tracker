/// <summary>
/// The vertical map is an overview map which will eventually display the bounds of the more
/// detailed horizontal map as well as its center.  It hangs on walls or other vertical surfaces.
/// </summary>
public class VerticalMap : Map
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        mapRendererBase.ZoomLevel = 1;
    }
    public override void MoveMap()
    {
        // We'll write this later
        throw new System.NotImplementedException();
    }
}
