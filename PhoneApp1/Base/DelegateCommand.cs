using System;
using System.Windows.Input;

namespace PhoneApp1
{
    /// <summary>
    ///     ICommand pattern wrapper
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Predicate<Object> _canExecute;
        private readonly Action<Object> _execute;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DelegateCommand" /> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public DelegateCommand(
            Action<Object> execute,
            Predicate<Object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        ///     Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can
        ///     be set to null.
        /// </param>
        /// <returns>
        ///     true if this command can be executed; otherwise, false.
        /// </returns>
        public Boolean CanExecute(Object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        /// <summary>
        ///     Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        ///     Data used by the command.  If the command does not require data to be passed, this object can
        ///     be set to null.
        /// </param>
        public void Execute(Object parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        ///     Raises the can execute changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}