public class WaterHandler : Singleton<WaterHandler>
{
    public float waterLevel { get => transform.position.y; }
}
