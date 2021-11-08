namespace Banks.Services.UserInterface.Start
{
    public interface IBankCommand
    {
        string GetName();
        void Execute();
    }
}