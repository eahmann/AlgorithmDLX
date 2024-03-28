using AlgorithmDLX.Polyomino.Models;

namespace AlgorithmDLX.Polyomino;

public class PolyominoTileComparer : IEqualityComparer<PolyominoTile>
    {
        public bool Equals(PolyominoTile obj1, PolyominoTile obj2)
        {
            if (obj1.Shape.Count != obj2.Shape.Count)
            {
                return false;
            }

            for (int i = 0; i < obj1.Shape.Count; i++)
            {
                if (obj1.Shape[i].X != obj2.Shape[i].X || obj1.Shape[i].Y != obj2.Shape[i].Y)
                {
                    return false;
                }
            }

            return true;
        }

        public int GetHashCode(PolyominoTile obj)
        {
            int hash = 17;

            foreach (var point in obj.Shape)
            {
                hash = hash * 31 + point.X;
                hash = hash * 31 + point.Y;
            }

            return hash;
        }
    }