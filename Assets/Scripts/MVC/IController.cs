namespace MVC
{
    public interface IController
    {
        IModel Model { get; }
        void Initialize(IModel model);
    }
}