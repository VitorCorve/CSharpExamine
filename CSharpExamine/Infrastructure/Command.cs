using System;
using System.Windows.Input;

namespace CSharpExamine.Infrastructure
{
    public class Command : ICommand
    {
        private Action<object> CommandExecute;
        private Func<object, bool> CanCommandExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            CommandExecute = execute;
            CanCommandExecute = canExecute; ;
        }
        public bool CanExecute(object? parameter) => this.CanCommandExecute == null || this.CanCommandExecute(parameter);
        /// <summary>
        /// Вызов функции <see cref="Action"/> <see cref="object"/> с передачей параметра.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object? parameter)
        {
            this.CommandExecute(parameter);
        }
    }
}
