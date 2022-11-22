public class HarpoonInfo
{
    Harpoonable _entityHarpooned;
    Harpoonable _harpoonShooter;
    Harpoon _harpoon;

    public Harpoonable entityHarpooned { get => _entityHarpooned; }
    public Harpoonable harpoonShooter { get => _harpoonShooter; }
    public Harpoon harpoon { get => _harpoon; }

    public HarpoonInfo(Harpoon harpoon, Harpoonable shotFrom, Harpoonable shotHit) {
        _harpoon = harpoon;
        _entityHarpooned = shotHit;
        _harpoonShooter = shotFrom; 
    }
}
