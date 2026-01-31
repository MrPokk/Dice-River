using BitterECS.Integration;

public struct ProtectiveComponent
{
    public int region;
}

public class ProtectiveComponentProvider : ProviderEcs<ProtectiveComponent> { }
