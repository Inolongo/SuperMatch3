namespace MVC
{
    public interface IController
    {
        IModel CellModel { get; }
        void Initialize(IModel model);
    }
}