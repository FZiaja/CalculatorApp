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

		public NumberButtonCommand NumberButtonCommand { get; set; }

		public CalculatorVM()
		{
			NumberButtonCommand = new NumberButtonCommand(this);
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
