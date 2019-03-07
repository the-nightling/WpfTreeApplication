using System;
using System.Windows;
using System.Windows.Threading;

namespace WpfTreeApplication
{
	public static class UIElementExtensions
	{
		private static readonly Action emptyDelegate = delegate () { };

		public static void Refresh(this UIElement uiElement)

		{
			uiElement.Dispatcher.Invoke(DispatcherPriority.Render, emptyDelegate);
		}
	}
}
