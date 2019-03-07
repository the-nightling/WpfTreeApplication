using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfTreeApplication.Trees;

namespace WpfTreeApplication
{
	public partial class MainWindow : Window
	{
		private Dictionary<char, FrameworkElement> controlByValue = new Dictionary<char, FrameworkElement>();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			var values = new[] { 'e', 'b', 'h', 'a', 'c', 'g', 'i', 'd', 'f' };
			foreach (var value in values)
				this.controlByValue.Add(value, CreateNodeControl(value));

			var tree = new BinarySearchTree();
			tree.Initialize(values);
			BuildTreeUI(tree);
		}

		private FrameworkElement CreateNodeControl(char content)
		{
			return new Label
			{
				Content = content.ToString(),
				Width = 50,
				Margin = new Thickness(10),
				HorizontalContentAlignment = HorizontalAlignment.Center,
				VerticalContentAlignment = VerticalAlignment.Center,
				BorderBrush = Brushes.Black,
				BorderThickness = new Thickness(1)
			};
		}

		private void BuildTreeUI(ATree tree)
		{
			AddRootNodeUI(tree.Root);
			AddSubTreeUI(tree.Root);
			this.nodeContainer.UpdateLayout();
			ConnectSubTreeUI(tree.Root);
		}

		private void AddSubTreeUI(ANode subTreeRoot)
		{
			if (subTreeRoot != null)
			{
				foreach (var child in subTreeRoot.GetChildren())
					AddChildUI(subTreeRoot, child);

				foreach (var child in subTreeRoot.GetChildren())
					AddSubTreeUI(child);
			}
		}

		private void ConnectSubTreeUI(ANode subTreeRoot)
		{
			if (subTreeRoot != null)
			{
				foreach (var child in subTreeRoot.GetChildren())
					ConnectChildUI(subTreeRoot, child);

				foreach (var child in subTreeRoot.GetChildren())
					ConnectSubTreeUI(child);
			}
		}

		private void AddRootNodeUI(ANode root)
		{
			var topLevelStackPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
			topLevelStackPanel.Children.Add(this.controlByValue[root.Value]);
			this.nodeContainer.Children.Add(topLevelStackPanel);
		}

		private void AddChildUI(ANode parent, ANode child)
		{
			var stackPanels = this.nodeContainer.Children.Cast<StackPanel>().ToArray();
			var parentStackPanel = FindParent<StackPanel>(this.controlByValue[parent.Value]);
			var parentPanelIndex = Array.IndexOf(stackPanels, parentStackPanel);
			var childPanelIndex = parentPanelIndex + 1;

			StackPanel childStackPanel;
			if (childPanelIndex >= stackPanels.Count())
			{
				childStackPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
				this.nodeContainer.Children.Add(childStackPanel);
			}
			else
			{
				childStackPanel = stackPanels[childPanelIndex];
			}

			childStackPanel.Children.Add(this.controlByValue[child.Value]);
		}

		private void ConnectChildUI(ANode parent, ANode child)
		{
			var startPoint = GetBottomMiddlePoint(this.controlByValue[parent.Value]);
			var endPoint = GetTopMiddlePoint(this.controlByValue[child.Value]);

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

		private Point GetTopMiddlePoint(FrameworkElement control)
		{
			return control.TransformToAncestor(this).Transform(new Point(control.ActualWidth / 2, 0));
		}

		private Point GetBottomMiddlePoint(FrameworkElement control)
		{
			return control.TransformToAncestor(this).Transform(new Point(control.ActualWidth / 2, control.ActualHeight));
		}

		private T FindParent<T>(DependencyObject child)
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
