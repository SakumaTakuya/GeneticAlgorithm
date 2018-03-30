using System;
using UnityEngine;

[Serializable]
public struct Matrix 
{
    public bool Equals(Matrix other)
    {
        return Equals(_element, other._element) && Row == other.Row && Column == other.Column;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is Matrix && Equals((Matrix) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (_element != null ? _element.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ Row;
            hashCode = (hashCode * 397) ^ Column;
            return hashCode;
        }
    }

    private readonly float[/*行*/,/*列*/] _element;
    
    public static explicit operator float[,] (Matrix i){return i._element;}
    public static implicit operator Matrix(float[,] a)
    {
        return new Matrix(a);
    }
    
    /// <summary>
    /// 行
    /// </summary>
    public int Row { get; private set; }
    
    /// <summary>
    /// 列
    /// </summary>
    public int Column { get; private set; }

    public Matrix(float[,] element) : this()
    {
        _element = element;
        Row = element.GetLength(0);
        Column = element.GetLength(1);
    }

    public Matrix(int row, int column) : this()
    {
        _element = new float[row, column];
        Row = row;
        Column = column;
    }
    
    public float this[int i, int j]
    {
        set{_element[i,j] = value;}
        get{return _element[i,j];}
    }

    public static Matrix operator +(Matrix a, Matrix b)
    {
        if(a.Column != b.Column || a.Row != b.Row) throw new Exception("行列の大きさが無効です");
        var mat = new Matrix(a.Row, a.Column);
        for (var i = 0; i < a.Row; i++)
        {
            for (var j = 0; j < a.Column; j++)
            {
                mat[i,j] = a[i,j] + b[i, j];
            }
        }
        return mat;
    }

    public static Matrix operator -(Matrix a, Matrix b)
    {
        if(a.Column != b.Column || a.Row != b.Row) throw new Exception("行列の大きさが無効です");
        var mat = new Matrix(a.Row, a.Column);
        for (var i = 0; i < a.Row; i++)
        {
            for (var j = 0; j < a.Column; j++)
            {
                mat[i,j] = a[i,j] - b[i, j];
            }
        }
        return mat;
    }
    
    public static bool operator ==(Matrix a, Matrix b)
    {
        if(a.Column != b.Column || a.Row != b.Row) throw new Exception("行列の大きさが無効です");
        var mat = new Matrix(a.Row, a.Column);
        var ret = true;
        for (var i = 0; i < a.Row; i++)
        {
            for (var j = 0; j < a.Column; j++)
            {
                mat[i,j] = a[i,j] - b[i, j];
                ret &= mat[i, j] == 0;
            }
        }
        return ret;
    }

    public static bool operator !=(Matrix a, Matrix b)
    {
        return !(a == b);
    }

    public static Matrix operator * (Matrix a, Matrix b)
    {
        if(a.Column != b.Row) throw new Exception("行列の大きさが無効です: a.col=" + a.Column + " ,b.row=" + b.Row);
        var mat = new Matrix(a.Row, b.Column);
        
        for (var i = 0; i < mat.Row; i++)
        {
            for (var j = 0; j < mat.Column; j++)
            {
                mat[i, j] = 0;
                for (var k = 0; k < a.Column/*==b.Row*/; k++)
                {
                    mat[i, j] += a[i, k] * b[k, j];
                }
            }
        }
        
        return mat;
    }
    
    public static Matrix operator * (Matrix a, float[] b)
    {
        if(a.Column != b.Length) throw new Exception("行列の大きさが無効です");
        var mat = new Matrix(a.Row, 1);
        
        for (var i = 0; i < mat.Row; i++)
        {
            mat[i,0] = 0;
            for (var k = 0; k < a.Column/*==b.Length*/; k++)
            {
                mat[i, 0] += a[i, k] * b[k];
            }
        }
        
        return mat;
    }
    
    public static Matrix operator * (Matrix a, int[] b)
    {
        if(a.Column != b.Length) throw new Exception("行列の大きさが無効です");
        var mat = new Matrix(a.Row, 1);
        
        for (var i = 0; i < mat.Row; i++)
        {
            mat[i,0] = 0;
            for (var k = 0; k < a.Column/*==b.Length*/; k++)
            {
                mat[i, 0] += a[i, k] * b[k];
            }
        }
        
        return mat;
    }

    public static Matrix operator *(Matrix a, float b)
    {
        var mat = new Matrix(a.Row, a.Column);
        for (var i = 0; i < a.Row; i++)
        {
            for (var j = 0; j < a.Column; j++)
            {
                mat[i, j] = a[i, j] * b;
            }
        }
        return mat;
    }
    
    public static Matrix operator *(float b, Matrix a)
    {
        var mat = new Matrix(a.Row, a.Column);
        for (var i = 0; i < a.Row; i++)
        {
            for (var j = 0; j < a.Column; j++)
            {
                mat[i, j] = a[i, j] * b;
            }
        }
        return mat;
    }

    public override string ToString()
    {
        var s = Row + "x" + Column + ":\n";
        for (var i = 0; i < Row; i++)
        {
            for (var j = 0; j < Column; j++)
            {
                s += this[i, j].ToString("0.0") + " ";
            }
            s += '\n';
        }
        return s;
    }
}
