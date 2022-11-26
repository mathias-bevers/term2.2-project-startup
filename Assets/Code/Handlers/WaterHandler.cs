public class WaterHandler : InstancedSingleton<WaterHandler>
{
    public float waterLevel { get => transform.position.y; }
}
