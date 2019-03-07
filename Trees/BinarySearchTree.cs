using System.Collections.Generic;

namespace WpfTreeApplication.Trees
{
	public class BinarySearchTree : ATree
	{
		public override void Insert(char value)
		{
			Root = Insert(value, (Node)Root);
		}

		private Node Insert(char value, Node subTreeRoot)
		{
			if (subTreeRoot == null)
			{
				return new Node(value);
			}
			else if (value < subTreeRoot.Value)
			{
				subTreeRoot.Left = Insert(value, subTreeRoot.Left);
				return subTreeRoot;
			}
			else
			{
				subTreeRoot.Right = Insert(value, subTreeRoot.Right);
				return subTreeRoot;
			}
		}

		private class Node : ANode
		{
			public Node(char value)
				: base(value)
			{ }

			public Node Left { get; set; }
			public Node Right { get; set; }
			
			public override IEnumerable<ANode> GetChildren()
			{
				if (Left != null)
				{
					return Right != null
							? new[] { Left, Right }
							: new[] { Left };
				}
				else if (Right != null)
				{
					return new[] { Right };
				}
				else
				{
					return new Node[0];
				}
			}
		}
	}
}
