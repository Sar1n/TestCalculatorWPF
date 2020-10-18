using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using MaliukCalculator.ViewModels;
using Ninject;

namespace MaliukCalculator
{
	public class Bootstrapper : BootstrapperBase
	{
		private IKernel kernel;
		protected override void Configure()
		{
			this.kernel = new StandardKernel();
			this.kernel.Bind<IMainViewModel>().To<MainViewModel>();
			this.kernel.Bind<IWindowManager>().To<WindowManager>();
		}
		public Bootstrapper()
		{
			Initialize();
		}
		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<IMainViewModel>();
		}
		protected override object GetInstance(Type serviceType, string key)
		{
			return kernel.Get(serviceType);
		}

	}
}
