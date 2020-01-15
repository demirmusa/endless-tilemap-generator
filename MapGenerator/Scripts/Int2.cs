/** \brief Auxiliar class to store coordinates inside the world grid*/
/**I got it from web but I don't know remember references, I can add reference link if owner contact me*/
public struct Int2
{
    /** x coordinate */
    public int X;
    /** y coordinate */
    public int Y;

    /** Constructor */
    public Int2(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    /** Module */
    public int SqrMagnitude
    {
        get
        {
            return X * X + Y * Y;
        }
    }

    /** Module(long) */
    public long SqrMagnitudeLong
    {
        get
        {
            return (long)X * (long)X + (long)Y * (long)Y;
        }
    }
    /** + override */
    public static Int2 operator +(Int2 a, Int2 b)
    {
        return new Int2(a.X + b.X, a.Y + b.Y);
    }
    /** - override */
    public static Int2 operator -(Int2 a, Int2 b)
    {
        return new Int2(a.X - b.X, a.Y - b.Y);
    }
    /** == override */
    public static bool operator ==(Int2 a, Int2 b)
    {
        return a.X == b.X && a.Y == b.Y;
    }
    /** != override */
    public static bool operator !=(Int2 a, Int2 b)
    {
        return a.X != b.X || a.Y != b.Y;
    }
    /** dot override */
    public static int Dot(Int2 a, Int2 b)
    {
        return a.X * b.X + a.Y * b.Y;
    }
    /** dot(long) override */
    public static long DotLong(Int2 a, Int2 b)
    {
        return (long)a.X * (long)b.X + (long)a.Y * (long)b.Y;
    }

    /** Equals override */
    public override bool Equals(System.Object o)
    {
        if (o == null) return false;
        Int2 rhs = (Int2)o;

        return X == rhs.X && Y == rhs.Y;
    }

    /** Hash override */
    public override int GetHashCode()
    {
        return X * 49157 + Y * 98317;
    }

    public override string ToString()
    {
        return "{ x: " + X + ", y: " + Y + " }";
    }
}
