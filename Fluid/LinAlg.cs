using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Fluid
{
    //Represents a matrix.
    public interface Matrix
    {
        int Rows { get; }
        int Columns { get; }

        /// <summary>
        /// Gets or sets the element in the matrix at the ith row and jth column.
        /// </summary>
        /// <param name="i">The row number.</param>
        /// <param name="j">The column number.</param>
        /// <returns>The element at the ith row and jth column.</returns>
        double this[int i, int j] { get; set; }
    }

    interface Vector
    {
        int Elements { get; }

        /// <summary>
        /// Gets or sets the ith element in the vector.
        /// </summary>
        /// <param name="i">The element number.</param>
        /// <returns>The ith element.</returns>
        double this[int i] { get; set; }
    }

    public class DenseMatrix : Matrix
    {
        double[,] data;
        int rows;
        int columns;

        public DenseMatrix(int rows, int columns)
        {
            data = new double[rows, columns];
            this.rows = rows;
            this.columns = columns;
        }

        public double[,] Data
        {
            get
            {
                return data;
            }
        }

        public int Rows { get { return rows; } }

        public int Columns { get { return columns; } }

        public double this[int i, int j]
        {
            get
            {
                return data[i, j];
            }
            set
            {
                data[i, j] = value;
            }
        }
    }

    /// <summary>
    /// Represents a Discrete Sine Transform Matrix of size dim.
    /// This matrix is immutable.
    /// </summary>
    public class DSTMatrix : Matrix
    {
        int dim;

        public DSTMatrix(int dim)
        {
            this.dim = dim;
        }

        public int Rows
        {
            get { return dim; }
        }

        public int Columns
        {
            get { return dim; }
        }

        public double this[int i, int j]
        {
            get
            {
                return Math.Sin(2 * Math.PI * i * j / dim);
            }
            set
            {
                throw new ArgumentException();
            }
        }
    }

    /// <summary>
    /// Represents a subsection of a larger matrix.
    /// </summary>
    public class ShallowSubsectionMatrix : Matrix
    {
        Matrix super;
        int xOffset;
        int yOffset;
        int rows;
        int columns;

        public ShallowSubsectionMatrix(Matrix super, int xOffset, int yOffset, int rows, int columns)
        {
            this.super = super;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.rows = rows;
            this.columns = columns;
        }

        public int Rows
        {
            get { return rows; }
        }

        public int Columns
        {
            get { return columns; }
        }

        public double this[int i, int j]
        {
            get
            {
                return super[xOffset + i, yOffset + j];
            }
            set
            {
                throw new ArgumentException();
            }
        }
    }

    public class DiagonalDelegateMatrix : Matrix
    {
        public delegate double DiagonalFunc(int ij);

        int dim;
        DiagonalFunc f;

        public DiagonalDelegateMatrix(DiagonalFunc f, int dim)
        {
            this.f = f;
            this.dim = dim;
        }

        public int Rows
        {
            get { return dim; }
        }

        public int Columns
        {
            get { return dim; }
        }

        public double this[int i, int j]
        {
            get
            {
                return i == j ? f(i) : 0;
            }
            set
            {
                throw new ArgumentException();
            }
        }
    }

    public class GridCellMatrix : Matrix
    {
        GridCell[,] cells;
        int dataIndex;

        public GridCellMatrix(GridCell[,] cells, int dataIndex)
        {
            this.cells = cells;
            this.dataIndex = dataIndex;
        }
        
        public int Rows
        {
            get { return cells.GetLength(0); }
        }

        public int Columns
        {
            get { return cells.GetLength(1); }
        }

        public double this[int i, int j]
        {
            get
            {
                return cells[i, j][dataIndex];
            }
            set
            {
                cells[i, j][dataIndex] = value;
            }
        }
    }

    public class DenseDelegateMatrix : Matrix
    {
        public delegate double DenseFunc(int i, int j);

        int dim;
        DenseFunc f;

        public DenseDelegateMatrix(DenseFunc f, int dim)
        {
            this.f = f;
            this.dim = dim;
        }

        public int Rows
        {
            get { return dim; }
        }

        public int Columns
        {
            get { return dim; }
        }

        public double this[int i, int j]
        {
            get
            {
                return f(i, j);
            }
            set
            {
                throw new ArgumentException();
            }
        }
    }

    public class IdentityMatrix : Matrix
    {
        int dim;

        public IdentityMatrix(int dim)
        {
            this.dim = dim;
        }

        public int Rows
        {
            get { return dim; }
        }

        public int Columns
        {
            get { return dim; }
        }

        public double this[int i, int j]
        {
            get
            {
                return i == j ? 1 : 0;
            }
            set
            {
                throw new ArgumentException();
            }
        }
    }

    public static class MatrixAlgebra
    {
        public static DenseMatrix Multiply(Matrix A, Matrix B)
        {
            DenseMatrix C = new DenseMatrix(A.Rows, B.Columns);
            for (int i = 0; i < A.Rows; i++)
                for (int j = 0; j < B.Columns; j++)
                    for (int k = 0; k < A.Columns; k++)
                        C[i, j] += A[i, k] * B[k, j];
            return C;
        }

        public static DenseMatrix Scale(Matrix A, double factor)
        {
            DenseMatrix B = new DenseMatrix(A.Rows, A.Columns);
            for (int i = 0; i < A.Rows; i++)
                for (int j = 0; j < A.Columns; j++)
                    B[i, j] = factor * A[i, j];
            return B;
        }

        public static DenseMatrix ToDenseMatrix(Matrix A)
        {
            DenseMatrix B = new DenseMatrix(A.Rows, A.Columns);
            for (int i = 0; i < A.Rows; i++)
                for (int j = 0; j < A.Columns; j++)
                    B[i, j] = A[i, j];
            return B;
        }

        //In the interest of time, we use the provided library.
        public static DenseMatrix Invert(Matrix A)
        {
            DenseMatrix B = ToDenseMatrix(A);
            MathNet.Numerics.LinearAlgebra.Double.DenseMatrix C = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.OfArray(B.Data);
            C = (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)C.Inverse();
            for (int i = 0; i < A.Rows; i++)
                for (int j = 0; j < A.Columns; j++)
                    B[i, j] = C[i, j];
            return B;
        }

        public static string ToString(Matrix A)
        {
            string s = "[";
            for (int i = 0; i < A.Rows; i++)
            {
                for (int j = 0; j < A.Columns; j++)
                    s += " " + A[i, j];
                if (i < A.Rows - 1)
                    s += "\n";
            }
            s += "]";
            return s;
        }
    }
}
