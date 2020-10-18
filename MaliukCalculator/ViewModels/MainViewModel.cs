using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using MaliukCalculator.Models;

namespace MaliukCalculator.ViewModels
{
	public class MainViewModel : Screen, IMainViewModel
	{
		private string _screen;
		private MainViewModel _mainViewModel;
		private string _leftOperand;
		private string _rightOperand;
		private string _operator;
		private BindableCollection<HistoryModel> _history = new BindableCollection<HistoryModel>();
		public MainViewModel()
		{
			Screen = "0";
		}
		public BindableCollection<HistoryModel> History
		{
			get
			{
				return _history;
			}
			set
			{
				_history = value;
			}
		}
		public string LeftOperand
		{
			get
			{
				return _leftOperand;
			}
			set
			{
				_leftOperand = value;
			}
		}
		public string RightOperand
		{
			get
			{
				return _rightOperand;
			}
			set
			{
				_rightOperand = value;
			}
		}
		public string Operator
		{
			get
			{
				return _operator;
			}
			set
			{
				_operator = value;
			}
		}
		public string Screen
		{
			get
			{
				return _screen;
			}
			set
			{
				_screen = value;
				NotifyOfPropertyChange(() => Screen);
			}
		}
		public void DoQuestion()
		{

		}
		public void Clear()
		{
			LeftOperand = "";
			RightOperand = "";
			Operator = "";
			RefreshScreen();
		}
		public void Backspace()
		{
			if (string.IsNullOrEmpty(Operator)) //if operator isnt set yet
			{
				if (!string.IsNullOrEmpty(LeftOperand)) //if not empty
					LeftOperand = LeftOperand.Substring(0, LeftOperand.Length - 1); //delete from first operand
			}
			else
			{
				if (string.IsNullOrEmpty(RightOperand)) //if operator is set but second operand isnt
				{
					if (Operator.Length > 0) //if not empty
						Operator = Operator.Substring(0, Operator.Length - 1); //delete from operator
				}
				else //if operator is set and second operand is set
					RightOperand = RightOperand.Substring(0, RightOperand.Length - 1); //delete from second operand
			}
			RefreshScreen();
		}
		public void ButtonPress(Button button)
		{
			if (int.TryParse(button.Content.ToString(), out int res)) //if it is number
				if (string.IsNullOrEmpty(Operator)) //if operator isnt set yet
					LeftOperand += res; //add to first operand
				else
					RightOperand += res; //add to second operand
			else //if it is operation
			{
				if (string.IsNullOrEmpty(LeftOperand))
					return;
				if (LeftOperand.Substring(LeftOperand.Length - 1) == ",") //if coma is last char in first operand
				{
					LeftOperand = LeftOperand.Substring(0, LeftOperand.Length - 1); //delete coma
					RefreshScreen();
				}
				switch (button.Name)
				{
					case "Plus":
						Operator = "+";
						break;
					case "Minus":
						Operator = "-";
						break;
					case "Multiply":
						Operator = "*";
						break;
					case "Divide":
						Operator = "/";
						break;
				}
			}
			RefreshScreen();
		}
		public void Coma()
		{
			if (string.IsNullOrEmpty(Operator)) //if operator isnt set yet
			{
				if (string.IsNullOrEmpty(LeftOperand)) //if it is first char
					LeftOperand += "0,"; //add zero before coma
				else
					if (!LeftOperand.Contains(",")) //if there is not already coma in operand
						LeftOperand += ",";
			}
			else //if operator is set
			{
				if (string.IsNullOrEmpty(RightOperand)) //if it is first char
					RightOperand += "0,"; //add zero before coma
				else
				if (!RightOperand.Contains(",")) //if there is not already coma in operand
					RightOperand += ",";
			}
			RefreshScreen();
		}
		public void Equals()
		{
			if (string.IsNullOrEmpty(LeftOperand))
				return;
			if (string.IsNullOrEmpty(RightOperand)) //second operand isnt set
			{
				if (LeftOperand.Substring(LeftOperand.Length - 1) == ",") //if coma is last char in first operand
					LeftOperand = LeftOperand.Substring(0, LeftOperand.Length - 1); //delete coma
				Operator = "";
				RefreshScreen();
			}
			else //both operands are set
			{
				if (RightOperand.Substring(RightOperand.Length - 1) == ",") //if coma is last char in second operand
				{
					RightOperand = RightOperand.Substring(0, RightOperand.Length - 1); //delete coma
					RefreshScreen();
				}
				LeftOperand = Calculate().ToString();
				RightOperand = "";
			}
			Operator = "";
			Screen += "=" + LeftOperand;
			if (History.Any())
			{
				if (History.First().Calculation != Screen)
					History.Insert(0, new HistoryModel {Calculation = Screen});
			}
			else
				History.Insert(0, new HistoryModel { Calculation = Screen });
		}
		public double Calculate()
		{
			double op1, op2, res = 0;
			double.TryParse(LeftOperand, out op1);
			double.TryParse(RightOperand, out op2);
			switch (Operator)
			{
				case "+":
					res = op1 + op2;
					break;
				case "-":
					res = op1 - op2;
					break;
				case "*":
					res = op1 * op2;
					break;
				case "/":
					res = op1 / op2;
					break;
			}
			return res;
		}
		public void ClearHistory()
		{
			if (!History.Any())
				return;
			var res = MessageBox.Show("Are you sure that you want to delete history?", "History", MessageBoxButton.YesNo);
			if (res == MessageBoxResult.Yes)
				foreach (var elem in History.ToArray())
				{
					History.Remove(elem);
				}
		}
		public void RefreshScreen()
		{
			string val = LeftOperand + Operator + RightOperand;
			if (val != "")
				Screen = val;
			else
				Screen = "0";
		}
		public void About()
		{
			var res = MessageBox.Show("This calculator was written by Ivan Maliuk as a test task", "About", MessageBoxButton.OK);
		}
		public void HistoryPull(Button button)
		{
			string calculation = button.Tag.ToString();
			LeftOperand = calculation.Substring(calculation.IndexOf("=") + 1);
			Screen = calculation;
		}
	}
}
