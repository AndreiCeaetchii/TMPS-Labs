namespace UniversityBookingSystem.Command;

public interface IBookingCommand
{
    string CommandName { get; }
    void Execute();
    void Undo();
    bool CanUndo { get; }
}
