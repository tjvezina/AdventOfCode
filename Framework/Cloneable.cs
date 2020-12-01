namespace AdventOfCode
{
    public interface IDeepCloneable<T>
    {
        T DeepClone();
    }

    public interface IShallowCloneable<T>
    {
        T ShallowClone();
    }
}
