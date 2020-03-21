using CalculatorApp.ViewModel.Commands;
using CalculatorApp.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorApp.ViewModel
{
	/* Only the operation buttons can change the index from 0 to 1.
	 AC button - resets the application
	 +/- button - if number at current index is 0, then sets display to 0. Otherwise, negates this number and updates the display.
	 % button k- if index is 0, then divides operands[0] by 100, stores the result back into operands[0]. 
				If index is 1, then calculates multiplies operands[0] by (operands[1]/100) and stores result in operands[1]
	 Operation buttons - Set selected operation. If index is 0, set the index to 1. 
						If index is 1 and the last button pressed was NOT an operation button, then calculate expression and store in operands[0].
	 = button - Evaluate expression and store in operands[0]*/
	public class CalculatorVM : INotifyPropertyChanged
    {
		private string displayString;

		public string DisplayString
		{
			get 
			{
				//return operands[index].ToString();
				return displayString; 
			}
			set 
			{ 
				displayString = value;
				OnPropertyChanged("DisplayString");
			}
		}

		private double[] operands;
		private int index;
		private Operator selectedOperator;
		private bool errorState;
		private bool operationPressedLast;

		public event PropertyChangedEventHandler PropertyChanged;

		public ACButtonCommand ACButtonCommand { get; set; }
		public DecimalButtonCommand DecimalButtonCommand { get; set; }
		public EqualsButtonCommand EqualsButtonCommand { get; set; }
		public NegativeButtonCommand NegativeButtonCommand { get; set; }
		public NumberButtonCommand NumberButtonCommand { get; set; }
		public OperatorButtonCommand OperatorButtonCommand { get; set; }
		public PercentButtonCommand PercentButtonCommand { get; set; }

		public CalculatorVM()
		{
			operands = new double[2];

			ACButtonClick();

			ACButtonCommand = new ACButtonCommand(this);
			DecimalButtonCommand = new DecimalButtonCommand(this);
			EqualsButtonCommand = new EqualsButtonCommand(this);
			NegativeButtonCommand = new NegativeButtonCommand(this);
			NumberButtonCommand = new NumberButtonCommand(this);
			OperatorButtonCommand = new OperatorButtonCommand(this);
			PercentButtonCommand = new PercentButtonCommand(this);
		}

		public void NumberButtonClick(object parameter)
		{
			//MessageBox.Show(parameter.ToString());
			//MessageBox.Show("---" + DisplayString + "---");

			string number = parameter as string;

			if (DisplayString == "0" || operationPressedLast)
			{
				DisplayString = number;
			}
			else
			{
				DisplayString = DisplayString + number;
			}

			operationPressedLast = false;

			//MessageBox.Show("---" + DisplayString + "---");
		}

		public void DecimalButtonClick()
		{
			if (operationPressedLast)
			{
				DisplayString = $"0.";
			}
			else if (!DisplayString.Contains("."))
			{
				DisplayString = $"{DisplayString}.";
			}

			operationPressedLast = false;
		}

		public void ACButtonClick()
		{
			DisplayString = "0";
			operands[0] = operands[1] = 0.0;
			index = 0;
			selectedOperator = Operator.Undefined;
			errorState = false;
			operationPressedLast = false;
		}

		public void NegativeButtonClick()
		{
			double number;

			if (!operationPressedLast && double.TryParse(DisplayString, out number))
			{
				number *= -1;
				DisplayString = number.ToString();
			}
		}

		public void PercentButtonClick()
		{
			double number;

			if (!operationPressedLast && double.TryParse(DisplayString, out number))
			{
				if (index == 0)
				{
					number /= 100;
				}
				else
				{
					number = operands[0] * number / 100;
				}

				DisplayString = number.ToString();
			}
		}

		public void OperatorButtonClick(object parameter)
		{
			if (index == 1 && selectedOperator != Operator.Undefined)
			{
				EqualsButtonClick();
			}


			if (!errorState)
			{
				string op = parameter as string;

				switch (op)
				{
					case "+":
						selectedOperator = Operator.Addition;
						break;
					case "-":
						selectedOperator = Operator.Subtraction;
						break;
					case "*":
						selectedOperator = Operator.Multiplication;
						break;
					case "/":
						selectedOperator = Operator.Division;
						break;
				}

				double number;

				if (double.TryParse(DisplayString, out number))
				{
					operands[index] = number;
				}

				operationPressedLast = true;
				index = 1;
			}
		}

		public void EqualsButtonClick()
		{
			if (index == 1 && double.TryParse(DisplayString, out operands[1]))
			{
				switch (selectedOperator)
				{
					case Operator.Addition:
						operands[0] = BasicMath.Add(operands[0], operands[1]);
						break;
					case Operator.Subtraction:
						operands[0] = BasicMath.Subtract(operands[0], operands[1]);
						break;
					case Operator.Multiplication:
						operands[0] = BasicMath.Multiply(operands[0], operands[1]);
						break;
					case Operator.Division:
						if (operands[1] == 0)
							errorState = true;
						else
							operands[0] = BasicMath.Divide(operands[0], operands[1]);
						break;
				}
				if (errorState)
				{
					DisplayString = "Error";
				}
				else
				{
					DisplayString = $"{operands[0]}";
				}
			}
		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public enum Operator
	{
		Undefined,
		Addition,
		Subtraction,
		Multiplication,
		Division
	}
}
