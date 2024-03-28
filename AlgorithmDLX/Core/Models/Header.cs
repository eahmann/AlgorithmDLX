namespace AlgorithmDLX.Core.Models;

public class Header : Node
{
    public int ColumnNumber { get; set; }
    public int NodeCount { get; private set; } = 0;

    public Header() : base()
    {
        Left = this;
        Right = this;
        Up = this;
        Down = this;
        ColumnHeader = this;
    }

    public void AppendNode(Node node)
    {
        Node lastNode = Up;
        lastNode.Down = node;
        node.Up = lastNode;
        node.Down = this;
        Up = node;
        node.ColumnHeader = this;
        NodeCount++;
    }

    /// <summary>
    /// Covers this header's column, effectively removing it from the matrix.
    /// </summary>
    public void Cover()
    {
        // Disconnect this column's header from its neighbors.
        Left.Right = Right;
        Right.Left = Left;

        // Go down the column and remove each row.
        for (Node rowNode = Down; rowNode != this; rowNode = rowNode.Down)
        {
            // Go right along the row and remove each node from its column.
            for (Node rowElement = rowNode.Right; rowElement != rowNode; rowElement = rowElement.Right)
            {
                rowElement.RemoveFromColumn();
                rowElement.ColumnHeader.NodeCount--;
            }
        }
    }

    /// <summary>
    /// Uncovers this header's column, effectively reinserting it into the matrix.
    /// </summary>
    public void Uncover()
    {
        // Start by restoring the links going upwards, then the links going left and right.
        for (Node rowNode = Up; rowNode != this; rowNode = rowNode.Up)
        {
            for (Node rowElement = rowNode.Left; rowElement != rowNode; rowElement = rowElement.Left)
            {
                rowElement.ReturnToColumn();
                rowElement.ColumnHeader.NodeCount++;
            }
        }

        // Now, reconnect the header itself into the list of headers.
        Left.Right = this;
        Right.Left = this;
    }

}
