namespace BitterECS.Core
{
    internal static class ComponentTypeMap
    {
        internal static int Count;
    }

    internal static class ComponentTypeMap<T>
    {
        internal static readonly int Id = ComponentTypeMap.Count++;
    }
}
