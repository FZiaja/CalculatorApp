using CalculatorApp.ViewModel.Commands;
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
		private string displayString;

		public string DisplayString
		{
			get { return displayString; }
			set 
			{ 
				displayString = value;
				OnPropertyChanged("DisplayString");
			}
		}

		private double number1;

		public double Number1
		{
			get { return number1; }
			set { number1 = value; }
		}

		private double number2;

		public double Number2
		{
			get { return number2; }
			set { number2 = value; }
		}

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
			ACButtonCommand = new ACButtonCommand(this);
			DecimalButtonCommand = new DecimalButtonCommand(this);
			EqualsButtonCommand = new EqualsButtonCommand(this);
			NegativeButtonCommand = new NegativeButtonCommand(this);
			NumberButtonCommand = new NumberButtonCommand(this);
			OperatorButtonCommand = new OperatorButtonCommand(this);
			PercentButtonCommand = new PercentButtonCommand(this);

			DisplayString = "0";
		}

		public void NumberButtonClick(object parameter)
		{
			//MessageBox.Show(parameter.ToString());
			//MessageBox.Show("---" + DisplayString + "---");
			string number = parameter as string;

			if (DisplayString == "0")
			{
				DisplayString = number;
			}
			else
			{
				DisplayString = DisplayString + number;
			}
			//MessageBox.Show("---" + DisplayString + "---");
		}

		public void DecimalButtonClick()
		{
			if (!DisplayString.Contains("."))
			{
				DisplayString = $"{DisplayString}.";
			}
		}

		public void ACButtonClick()
		{

		}

		public void NegativeButtonClick()
		{

		}

		public void PercentButtonClick()
		{

		}

		public void OperatorButtonClick()
		{

		}

		public void EqualsButtonClick()
		{

		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
