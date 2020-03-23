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
	public class CalculatorVM : INotifyPropertyChanged
    {
        #region Private Members
        private string displayString;

		private double result;							// Holds the final and intermediate computed results.
		private Operator selectedOperator;				// Holds the currently selected operator, from the most recently presses operation button.
		private ErrorState errorState;                  // Indicates whether the calculator is in an error state (eg. after dividing by 0).


		// In this application, dependency button is any button that can affect the behavior of another button later in the computation.
		private ButtonType lastDepButtonPressed;        // Indicates the type of the last dependency button pressed (see ButtonType enum below). 
														// This variable is updated for all button presses.
		private ButtonType lastDepButtonPressed2;       // Indicates the type of the last dependency button pressed (see ButtonType enum below).
														// This variable is updated only for presses of the "AC", "Percent", operator, and "Equals" buttons.
		#endregion

		#region Commands
		public ACButtonCommand ACButtonCommand { get; set; }
		public DecimalButtonCommand DecimalButtonCommand { get; set; }
		public EqualsButtonCommand EqualsButtonCommand { get; set; }
		public NegativeButtonCommand NegativeButtonCommand { get; set; }
		public NumberButtonCommand NumberButtonCommand { get; set; }
		public OperatorButtonCommand OperatorButtonCommand { get; set; }
		public PercentButtonCommand PercentButtonCommand { get; set; }
		#endregion
		 
		/// <summary>
		/// CalculatorVM Constructor
		/// </summary>
		public CalculatorVM()
		{
			// Reset the calculator to its initial state.
			ACButtonClick();

			// Assign commands.
			ACButtonCommand = new ACButtonCommand(this);
			DecimalButtonCommand = new DecimalButtonCommand(this);
			EqualsButtonCommand = new EqualsButtonCommand(this);
			NegativeButtonCommand = new NegativeButtonCommand(this);
			NumberButtonCommand = new NumberButtonCommand(this);
			OperatorButtonCommand = new OperatorButtonCommand(this);
			PercentButtonCommand = new PercentButtonCommand(this);
		}


		/// <summary>
		/// Display string bound to the calculator label.
		/// </summary>
		public string DisplayString
		{
			get
			{
				return displayString;
			}
			set
			{
				displayString = value;
				OnPropertyChanged("DisplayString");
			}
		}

        #region Button Command Implementations
        public void NumberButtonClick(object parameter)
		{
			/* Method invoked when a numeric button is pressed. */

			// If the last button pressed was "Equals" or "Percent", or if the calculator is in an error state, then reset the calculator to its initial state.
			if (lastDepButtonPressed2 == ButtonType.Equals || lastDepButtonPressed2 == ButtonType.Percent || errorState != ErrorState.None)
			{
				ACButtonClick();
			}

			// Get the digit that was pressed
			string number = parameter as string;

			// If the display reads "0" or the last button pressed was an operator button, then reset the display and begin writing a new number.
			if (DisplayString == "0" || lastDepButtonPressed == ButtonType.Operator)
			{
				DisplayString = number;
			}
			// Otherwise append the new digit to the current number.
			else
			{
				DisplayString = DisplayString + number;
			}

			// Update last button pressed.
			lastDepButtonPressed = ButtonType.Number;
		}

		public void DecimalButtonClick()
		{
			/* Method invoked when the decimal button is pressed. */

			// If the last button pressed was "Equals" or "Percent", or if the calculator is in an error state, then reset the calculator to its initial state.
			if (lastDepButtonPressed2 == ButtonType.Equals || lastDepButtonPressed2 == ButtonType.Percent  || errorState != ErrorState.None)
			{
				ACButtonClick();
			}

			// If the last button pressed was an operator button, then reset the display and begin writing a new number.
			if (lastDepButtonPressed2 == ButtonType.Operator)
			{
				DisplayString = $"0.";
			}
			// Otherwise append a decimal point, unless the current number already has one.
			else if (!DisplayString.Contains("."))
			{
				DisplayString = $"{DisplayString}.";
			}

			// Update last button pressed.
			lastDepButtonPressed = ButtonType.Decimal;
		}

		public void ACButtonClick()
		{
			/* Method invoked in class construction and when the "AC" button is pressed.
			 * Resets the calculator to its initial state. */

			DisplayString = "0";
			result = 0.0;
			selectedOperator = Operator.Undefined;
			errorState = ErrorState.None;
			lastDepButtonPressed = ButtonType.AC;
			lastDepButtonPressed2 = ButtonType.AC;
		}

		public void NegativeButtonClick()
		{
			/* Method invoked when the "+/-" button is pressed. */

			// If the calculator is in an error state, then reset to its initial state.
			if (errorState != ErrorState.None)
			{
				ACButtonClick();
			}

			// Variable to store the number currently being displayed.
			double number;

			// If the last button pressed was and operator, then don't do anything. Otherwise, parse the number on the display and store in the "number" variable.
			if (lastDepButtonPressed != ButtonType.Operator && double.TryParse(DisplayString, out number))
			{
				// Negate the number and display the result.
				number *= -1;
				DisplayString = number.ToString();

				// If the last button pressed was "Equals", then negate the number stored in "result", since we want to negate the result computed so far.
				// Otherwise, we only want to negate the number on the display, without affecting the result.
				if (lastDepButtonPressed2 == ButtonType.Equals)
				{
					result *= -1;
				}

				// Update last button pressed.
				lastDepButtonPressed = ButtonType.Negative;
			}
		}

		public void PercentButtonClick()
		{
			/* Method invoked when the "Percent" button is pressed. */

			// If the calculator is in an error state, then reset to its initial state.
			if (errorState != ErrorState.None)
			{
				ACButtonClick();
			}

			// Variable to store the number currently being displayed.
			double number;

			// Parse the number on the display and store in the "number" variable.
			if (double.TryParse(DisplayString, out number))
			{
				// If the currently selected operator is addition or subtraction, then take the number stored in "result" and multiply it by the current number, and divide by 100.
				if (selectedOperator == Operator.Addition || selectedOperator == Operator.Subtraction)
				{
					number = result * number / 100;
				}
				else
				// Otherwise divide the current number by 100.
				{
					number /= 100;
				}

				// Display the result.
				DisplayString = number.ToString();
			}

			// Update last button pressed and last dependency button pressed.
			lastDepButtonPressed = ButtonType.Percent;
			lastDepButtonPressed2 = ButtonType.Percent;
		}

		public void OperatorButtonClick(object parameter)
		{
			/* Method invoked when an operator button is pressed. */

			// If the calculator is in an error state, don't do anything.
			if (errorState == ErrorState.None) 
			{
				// The current number is the first number in the current computation (ie. the selected operator is undefined), then store the number in "result.
				if (selectedOperator == Operator.Undefined)
				{
					double number;

					if (double.TryParse(DisplayString, out number))
					{
						result = number;
					}
				}
				// Otherwise, an operator has already been selected. If this previous operator was followed by a number (that is, the last button pressed was not "Equals" or another operator),
				// then click the "Equals" button to compute an intermediate result.
				// Otherwise, if the last button pressed was another operator, then the user likely pressed the previous operator in error, so we update the operator without making a computation.
				else if (selectedOperator != Operator.Undefined && lastDepButtonPressed != ButtonType.Operator)
				{
					EqualsButtonClick();
				}

				// Get the parameter as a string.
				string op = parameter as string;

				// Set the selected operator.
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
			}

			// Update last button pressed and last dependency button pressed.
			lastDepButtonPressed = ButtonType.Operator;
			lastDepButtonPressed2 = ButtonType.Operator;
		}

		public void EqualsButtonClick()
		{
			/* Method invoked when the "Equals" button is pressed. */

			// If the calculator is in an error state, then don't do anything.
			if (errorState == ErrorState.None)
			{
				// Variable to store the number currently being displayed.
				double number;

				// Parse and store the number currently being displayed.
				if (double.TryParse(DisplayString, out number))
				{
					// If the current number is the first number in the current computation (ie. the selected operator is undefined), then store this number in "result".
					if (selectedOperator == Operator.Undefined)
					{
						result = number;
					}
					// Otherwise, perform the selected operation and store the result in the "result" member.
					else
					{
						switch (selectedOperator)
						{
							case Operator.Addition:
								result = BasicMath.Add(result, number);
								break;
							case Operator.Subtraction:
								result = BasicMath.Subtract(result, number);
								break;
							case Operator.Multiplication:
								result = BasicMath.Multiply(result, number);
								break;
							case Operator.Division:
								// If dividing by 0, set the calculator to error state
								if (number == 0)
									errorState = ErrorState.DivisionByZero;
								else
									result = BasicMath.Divide(result, number);
								break;
						}

						// Set the display accordingly
						if (errorState != ErrorState.None)
						{
							DisplayString = "Error";
						}
						else
						{
							DisplayString = $"{result}";
						}
					}
				}

				// Reset the selected operator to "Undefined" and update last button pressed and last dependency button pressed.
				selectedOperator = Operator.Undefined;
				lastDepButtonPressed = ButtonType.Equals;
				lastDepButtonPressed2 = ButtonType.Equals;
			}
		}

		#endregion

		#region INotifyPropertyChanged members.

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}

	/// <summary>
	/// An enum for possible operators.
	/// </summary>
	public enum Operator
	{
		Undefined,
		Addition,
		Subtraction,
		Multiplication,
		Division
	}

	/// <summary>
	/// An enum for button types.
	/// </summary>
	public enum ButtonType
	{
		AC,
		Negative,
		Number,
		Decimal,
		Percent,
		Operator,
		Equals
	}

	/// <summary>
	/// An enum for error states.
	/// </summary>
	public enum ErrorState
	{
		None,
		DivisionByZero
	}
}
