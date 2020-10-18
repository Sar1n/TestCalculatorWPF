using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;
using MaliukCalculator.Models;

namespace MaliukCalculator.ViewModels
{
	public interface IMainViewModel
	{
		BindableCollection<HistoryModel> History { get; set; }
		string LeftOperand { get; set; }
		string RightOperand { get; set; }
		string Operator { get; set; }
		string Screen { get; set; }
		void Clear();
		void Backspace();
		void ButtonPress(Button button);
		void Coma();
		void Equals();
		double Calculate();
		void ClearHistory();
		void RefreshScreen();
		void About();
		void HistoryPull(Button button);
	}
}
