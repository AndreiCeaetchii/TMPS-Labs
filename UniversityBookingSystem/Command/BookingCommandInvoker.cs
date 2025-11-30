namespace UniversityBookingSystem.Command;

public class BookingCommandInvoker
{
    private readonly Stack<IBookingCommand> _commandHistory = new();
    private readonly Stack<IBookingCommand> _undoneCommands = new();

    public void ExecuteCommand(IBookingCommand command)
    {
        command.Execute();
        _commandHistory.Push(command);
        _undoneCommands.Clear();
        Console.WriteLine($"[INVOKER] Command executed: {command.CommandName}");
    }

    public void Undo()
    {
        if (_commandHistory.Count == 0)
        {
            Console.WriteLine("[INVOKER] No commands to undo");
            return;
        }

        var command = _commandHistory.Pop();
        if (command.CanUndo)
        {
            command.Undo();
            _undoneCommands.Push(command);
            Console.WriteLine($"[INVOKER] Command undone: {command.CommandName}");
        }
        else
        {
            Console.WriteLine($"[INVOKER] Command cannot be undone: {command.CommandName}");
        }
    }

    public void Redo()
    {
        if (_undoneCommands.Count == 0)
        {
            Console.WriteLine("[INVOKER] No commands to redo");
            return;
        }

        var command = _undoneCommands.Pop();
        command.Execute();
        _commandHistory.Push(command);
        Console.WriteLine($"[INVOKER] Command redone: {command.CommandName}");
    }

    public void ShowHistory()
    {
        Console.WriteLine("\n=== Command History ===");
        if (_commandHistory.Count == 0)
        {
            Console.WriteLine("No commands in history");
            return;
        }

        var commands = _commandHistory.ToList();
        commands.Reverse();
        for (int i = 0; i < commands.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {commands[i].CommandName}");
        }
    }

    public int HistoryCount => _commandHistory.Count;
    public int UndoneCount => _undoneCommands.Count;
}
