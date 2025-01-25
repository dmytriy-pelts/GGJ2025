namespace GumFly.Domain
{
    public interface IInitializable<in T>
    {
        void Initialize(T instance);
    }
}