namespace WpfTreeApplication.Trees
{
	public abstract class ATree
	{
		public ANode Root { get; protected set; }

		public void Initialize(params char[] values)
		{
			Root = null;
			foreach (var value in values)
				Insert(value);
		}

		public abstract void Insert(char value);
	}
}
