using System.Collections.Generic;

namespace WpfTreeApplication.Trees
{
	public abstract class ANode
	{
		public ANode(char value)
		{
			Value = value;
		}

		public char Value { get; }

		public abstract IEnumerable<ANode> GetChildren();
	}
}
