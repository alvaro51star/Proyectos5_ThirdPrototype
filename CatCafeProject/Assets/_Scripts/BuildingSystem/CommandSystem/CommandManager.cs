using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandSystem
{
    /// <summary>
    /// Simple command pattern implementation that includes Undo functionality
    /// </summary>
    public class CommandManager
    {
        private Stack<ICommand> commands = new Stack<ICommand>();

        public void Invoke(ICommand commandToExecute)
        {
            if (commandToExecute.CanExecute() == false)
                return;
            commands.Push(commandToExecute);
            commandToExecute.Execute();

        }

        public bool Undo()
        {
            if (commands.Count <= 0)
                return false;
            ICommand command = commands.Pop();
            command.Undo();
            return true;
        }

        public void ClearCommandsList()
            => commands.Clear();

        public int GetCommandsCount() => commands.Count;
    }
}