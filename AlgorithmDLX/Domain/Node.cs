namespace AlgorithmDLX.Domain;

/// <summary>
/// Represents a node in a dancing links data structure
/// </summary>
public class Node
{
    /// <summary>
    /// Left <see cref="Node"/>
    /// </summary>
    public Node Left { get; set; }

    /// <summary>
    /// Right <see cref="Node"/>
    /// </summary>
    public Node Right { get; set; }

    /// <summary>
    /// Up <see cref="Node"/>
    /// </summary>
    public Node Up { get; set; }

    /// <summary>
    /// Down <see cref="Node"/>
    /// </summary>
    public Node Down { get; set; }

    /// <summary>
    /// ColumnHeader <see cref="Header"/>
    /// </summary>
    public Header ColumnHeader { get; set; }

    /// <summary>
    /// Row index
    /// </summary>
    public int RowIndex { get; set; }

    /// <summary>
    /// Default constructor. Creates a self referencing <see cref="Node"/>
    /// </summary>
    public Node()
    {
        // Initially, a node points to itself in all directions.
        Left = Right = Up = Down = this;
    }

    /// <summary>
    /// Removes this node from its row.
    /// </summary>
    public void RemoveFromRow()
    {
        Left.Right = Right;
        Right.Left = Left;
    }

    /// <summary>
    /// Reinserts this node into its row.
    /// </summary>
    public void ReturnToRow()
    {
        Left.Right = this;
        Right.Left = this;
    }

    /// <summary>
    /// Remove this node from its column.
    /// </summary>
    public void RemoveFromColumn()
    {
        Up.Down = Down;
        Down.Up = Up;
    }

    /// <summary>
    /// Return this node to its column.
    /// </summary>
    public void ReturnToColumn()
    {
        Up.Down = this;
        Down.Up = this;
    }
}
