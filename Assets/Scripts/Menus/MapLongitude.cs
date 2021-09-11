/// <summary>
/// Implements the MapLatLong class to display the longitude of the map center with proper
/// formatting.
/// </summary>
public class MapLongitude : MapLatLong
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
        text.text = locationHandler.WriteLongitude(mapRendererBase.Center.LongitudeInDegrees);
    }
}
