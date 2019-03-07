using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTreeApplication
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			var label1 = CreateNode("1");
			var label2 = CreateNode("2");
			var label3 = CreateNode("3");
			var label4 = CreateNode("4");
			var label5 = CreateNode("5");
			var label6 = CreateNode("6");
			var label7 = CreateNode("7");

			AddRootNode(label1);

			//var stackPanel2 = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
			//stackPanel2.Children.Add(label2);
			//stackPanel2.Children.Add(label3);
			//this.nodeContainer.Children.Add(stackPanel2);

			AddChildren(label1,
						label2, label3, label4);

			AddChildren(label2,
						label5, label6);

			AddChildren(label4,
						label7);

			this.nodeContainer.UpdateLayout();

			ConnectNodes(label1,
						label2, label3, label4);

			ConnectNodes(label2,
						label5, label6);

			ConnectNodes(label4,
						label7);
		}

		private FrameworkElement CreateNode(string content)
		{
			return new Label()
			{
				Content = content,
				Width = 50,
				Margin = new Thickness(10),
				HorizontalContentAlignment = HorizontalAlignment.Center,
				VerticalContentAlignment = VerticalAlignment.Center,
				BorderBrush = Brushes.Black,
				BorderThickness = new Thickness(1)
			};
		}

		private void AddRootNode(FrameworkElement rootNode)
		{
			var topLevelStackPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
			topLevelStackPanel.Children.Add(rootNode);
			this.nodeContainer.Children.Add(topLevelStackPanel);
		}

		private void AddChildren(FrameworkElement parentNode, params FrameworkElement[] childNodes)
		{
			var stackPanels = this.nodeContainer.Children.Cast<StackPanel>().ToArray();
			var parentStackPanel = FindParent<StackPanel>(parentNode);
			var parentPanelIndex = Array.IndexOf(stackPanels, parentStackPanel);
			var childPanelIndex = parentPanelIndex + 1;

			StackPanel childStackPanel;
			if (childPanelIndex >= stackPanels.Count())
			{
				childStackPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
				this.nodeContainer.Children.Add(childStackPanel);
				this.nodeContainer.UpdateLayout();
			}
			else
			{
				childStackPanel = stackPanels[childPanelIndex];
			}

			foreach (var childNode in childNodes)
				childStackPanel.Children.Add(childNode);
		}

		private void ConnectNodes(FrameworkElement parent, params FrameworkElement[] children)
		{
			var startPoint = GetBottomMiddlePoint(parent);

			foreach (var child in children)
			{
				var endPoint = GetTopMiddlePoint(child);

				this.overlay.Children.Add(new Line()
				{
					X1 = startPoint.X,
					Y1 = startPoint.Y,
					X2 = endPoint.X,
					Y2 = endPoint.Y,
					Stroke = Brushes.Black,
					StrokeThickness = 1
				});
			}
		}

		private Point GetTopMiddlePoint(FrameworkElement control)
		{
			return control.TransformToAncestor(this).Transform(new Point(control.ActualWidth / 2, 0));
		}

		private Point GetBottomMiddlePoint(FrameworkElement control)
		{
			return control.TransformToAncestor(this).Transform(new Point(control.ActualWidth / 2, control.ActualHeight));
		}

		public T FindParent<T>(DependencyObject child)
			where T : DependencyObject
		{
			var parentObject = VisualTreeHelper.GetParent(child);

			if (parentObject == null)
				return null;

			if (parentObject is T parent)
				return parent;
			
			return FindParent<T>(parentObject);
		}
	}
}
