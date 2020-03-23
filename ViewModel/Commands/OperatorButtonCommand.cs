using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalculatorApp.ViewModel.Commands
{
    /// <summary>
    /// Command class to implement the command for the operator buttons.
    /// </summary>
    public class OperatorButtonCommand : ICommand
    {
        public CalculatorVM VM { get; set; }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public OperatorButtonCommand(CalculatorVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.OperatorButtonClick(parameter);
        }
    }
}
