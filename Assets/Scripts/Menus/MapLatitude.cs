/// <summary>
/// Implements the MapLatLong class to display the latitude of the map center with proper
/// formatting.
/// </summary>
public class MapLatitude : MapLatLong
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Update();
    }

    // Update is called once per frame
    protected override void Update()
    {
        text.text = locationHandler.WriteLatitude(mapRendererBase.Center.LatitudeInDegrees);
    }
}
