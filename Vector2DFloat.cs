using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Additional.Math
{
    namespace Vectors
    {
        public struct Vector2DFloat
        {
            public float X;
            public float Y;
            public void SetXY(float x, float y)
            {
                X = x;
                Y = y;
            }
            public static Vector2DFloat Lerp(Vector2DFloat v0, Vector2DFloat v1, float t)
            {
                return v0 + (v1 - v0) * t;
            }

            public Vector2DFloat(Vector2DFloat value)
            {
                this.X = value.X;
                this.Y = value.Y;
            }
            public Vector2DFloat(float x, float y)
            {
                this.X = x;
                this.Y = y;
            }

            public Vector2DFloat Rotate90Degrees(bool positiveRotation)
            {
                return positiveRotation ? new Vector2DFloat(-Y, X) : new Vector2DFloat(Y, -X);
            }
            public Vector2DFloat Normalize()
            {
                float magnitude = (float)System.Math.Sqrt(X * X + Y * Y);
                return new Vector2DFloat(X / magnitude, Y / magnitude);
            }
            public float Distance(Vector2DFloat target)
            {
                return (float)System.Math.Sqrt((X - target.X) * (X - target.X) + (Y - target.Y) * (Y - target.Y));
            }
            public float DistanceSquared(Vector2DFloat target)
            {
                return (X - target.X) * (X - target.X) + (Y - target.Y) * (Y - target.Y);
            }
            public double DistanceTo(Vector2DFloat target)
            {
                return System.Math.Sqrt(System.Math.Pow(target.X - X, 2F) + System.Math.Pow(target.Y - Y, 2F));
            }

            public System.Drawing.PointF ToPointF()
            {
                return new System.Drawing.PointF(X, Y);
            }
            public Vector2DFloat(System.Drawing.PointF value)
            {
                this.X = value.X;
                this.Y = value.Y;
            }
            public static implicit operator Vector2DFloat(System.Drawing.PointF value)
            {
                return new Vector2DFloat(value);
            }

            public static Vector2DFloat operator +(Vector2DFloat first, Vector2DFloat second)
            {
                return new Vector2DFloat(first.X + second.X, first.Y + second.Y);
            }
            public static Vector2DFloat operator -(Vector2DFloat first, Vector2DFloat second)
            {
                return new Vector2DFloat(first.X - second.X, first.Y - second.Y);
            }
            public static Vector2DFloat operator *(Vector2DFloat first, float second)
            {
                return new Vector2DFloat(first.X * second, first.Y * second);
            }
            public static Vector2DFloat operator *(float first, Vector2DFloat second)
            {
                return new Vector2DFloat(second.X * first, second.Y * first);
            }
            public static Vector2DFloat operator *(Vector2DFloat first, int second)
            {
                return new Vector2DFloat(first.X * second, first.Y * second);
            }
            public static Vector2DFloat operator *(int first, Vector2DFloat second)
            {
                return new Vector2DFloat(second.X * first, second.Y * first);
            }
            public static Vector2DFloat operator *(Vector2DFloat first, double second)
            {
                return new Vector2DFloat((float)(first.X * second), (float)(first.Y * second));
            }
            public static Vector2DFloat operator *(double first, Vector2DFloat second)
            {
                return new Vector2DFloat((float)(second.X * first), (float)(second.Y * first));
            }
            public override bool Equals(object obj)
            {
                return this.Equals((Vector2DFloat)obj);
            }
            public bool Equals(Vector2DFloat p)
            {
                // If parameter is null, return false.
                if (p == null)
                {
                    return false;
                }

                // Optimization for a common success case.
                if (this == p)
                {
                    return true;
                }

                // If run-time types are not exactly the same, return false.
                if (this.GetType() != p.GetType())
                {
                    return false;
                }

                // Return true if the fields match.
                // Note that the base class is not invoked because it is
                // System.Object, which defines Equals as reference equality.
                return (X == p.X) && (Y == p.Y);
            }
            public override int GetHashCode()
            {
                return (int)(System.Math.Round(X + Y, 4) * 10000);
            }
            public static bool operator ==(Vector2DFloat first, Vector2DFloat second)
            {
                // Check for null on left side.
                if (first == null)
                {
                    if (second == null)
                    {
                        // null == null = true.
                        return true;
                    }

                    // Only the left side is null.
                    return false;
                }
                // Equals handles case of null on right side.
                return first.Equals(second);
            }
            public static bool operator !=(Vector2DFloat first, Vector2DFloat second)
            {
                return !(first == second);
            }
        }
    }
    namespace Beziers
    {
        public struct BezierCubic2D
        {
            public Vectors.Vector2DFloat P0;
            public Vectors.Vector2DFloat P1;
            public Vectors.Vector2DFloat P2;
            public Vectors.Vector2DFloat P3;
            public int ArcLengthDivisionCount;
            public List<float> ArcLengths { get { if (_ArcLengths.Count == 0) CalculateArcLength(); return _ArcLengths; } }
            public float ArcLength { get { if (_ArcLength == 0.0F) CalculateArcLength(); return _ArcLength; } }

            private Vectors.Vector2DFloat A;
            private Vectors.Vector2DFloat B;
            private Vectors.Vector2DFloat C;
            private List<float> _ArcLengths;
            private float _ArcLength;

            public BezierCubic2D(Vectors.Vector2DFloat p0, Vectors.Vector2DFloat p1, Vectors.Vector2DFloat p2, Vectors.Vector2DFloat p3)
            {
                P0 = p0;
                P1 = p1;
                P2 = p2;
                P3 = p3;

                // vt = At^3 + Bt^2 + Ct + p0
                A = P3 - 3 * P2 + 3 * P1 - P0;
                B = 3 * P2 - 6 * P1 + 3 * P0;
                C = 3 * P1 - 3 * P0;
                ArcLengthDivisionCount = 100;
                _ArcLengths = new List<float>();
                _ArcLength = 0.0F;
            }
            public BezierCubic2D(System.Drawing.PointF p0, System.Drawing.PointF p1, System.Drawing.PointF p2, System.Drawing.PointF p3)
            {
                P0 = p0;
                P1 = p1;
                P2 = p2;
                P3 = p3;

                // vt = At^3 + Bt^2 + Ct + p0
                A = P3 - 3 * P2 + 3 * P1 - P0;
                B = 3 * P2 - 6 * P1 + 3 * P0;
                C = 3 * P1 - 3 * P0;
                ArcLengthDivisionCount = 100;
                _ArcLengths = new List<float>();
                _ArcLength = 0.0F;
            }
            public BezierCubic2D(float p0X, float p0Y, float p1X, float p1Y, float p2X, float p2Y, float p3X, float p3Y)
            {
                P0 = new Vectors.Vector2DFloat(p0X, p0Y);
                P1 = new Vectors.Vector2DFloat(p1X, p1Y);
                P2 = new Vectors.Vector2DFloat(p2X, p2Y);
                P3 = new Vectors.Vector2DFloat(p3X, p3Y);

                // vt = At^3 + Bt^2 + Ct + p0
                A = P3 - 3 * P2 + 3 * P1 - P0;
                B = 3 * P2 - 6 * P1 + 3 * P0;
                C = 3 * P1 - 3 * P0;
                ArcLengthDivisionCount = 100;
                _ArcLengths = new List<float>();
                _ArcLength = 0.0F;
            }

            public Vectors.Vector2DFloat PointOnCurve(float t)
            {
                return A * System.Math.Pow(t, 3) + B * System.Math.Pow(t, 2) + C * t + P0;
            }
            public Vectors.Vector2DFloat PointOnCurveGeometric(float t)
            {
                Vectors.Vector2DFloat p4 = Vectors.Vector2DFloat.Lerp(P0, P1, t);
                Vectors.Vector2DFloat p5 = Vectors.Vector2DFloat.Lerp(P1, P2, t);
                Vectors.Vector2DFloat p6 = Vectors.Vector2DFloat.Lerp(P2, P3, t);
                Vectors.Vector2DFloat p7 = Vectors.Vector2DFloat.Lerp(p4, p5, t);
                Vectors.Vector2DFloat p8 = Vectors.Vector2DFloat.Lerp(p5, p6, t);
                return Vectors.Vector2DFloat.Lerp(p7, p8, t);
            }
            public Vectors.Vector2DFloat PointOnCurveTangent(float t)
            {
                return 3 * A * System.Math.Pow(t, 2) + 2 * B * t + C;
            }
            public Vectors.Vector2DFloat PointOnCurvePerpendicular(float t, bool positiveRotation)
            {
                return (3 * A * System.Math.Pow(t, 2) + 2 * B * t + C).Rotate90Degrees(positiveRotation).Normalize() * 10F + PointOnCurve(t);
            }
            public Vectors.Vector2DFloat PointOnCurvePerpendicular(float t, bool positiveRotation, float pointHeight)
            {
                return (3 * A * System.Math.Pow(t, 2) + 2 * B * t + C).Rotate90Degrees(positiveRotation).Normalize() * pointHeight + PointOnCurve(t);
            }
            public float FindTAtPointOnBezier(float u)
            {
                float t;
                int index = _ArcLengths.BinarySearch(u);
                if (index >= 0)
                    t = index / (float)(_ArcLengths.Count - 1);
                else if (index * -1 >= _ArcLengths.Count)
                    t = 1;
                else if (index == 0)
                    t = 0;
                else
                {
                    index *= -1;
                    float lengthBefore = _ArcLengths[index - 1];
                    float lengthAfter = _ArcLengths[index];
                    float segmentLength = lengthAfter - lengthBefore;

                    float segmentFraction = (u - lengthBefore) / segmentLength;

                    // add that fractional amount to t 
                    t = (index + segmentFraction) / (float)(_ArcLengths.Count - 1);
                }
                return t;
            }

            private void CalculateArcLength()
            {
                // calculate Arc Length through successive approximation. Use the squared version as it is faster.
                _ArcLength = 0.0F;
                int arrayMax = ArcLengthDivisionCount + 1;
                _ArcLengths = new List<float>(arrayMax)
            {
                0.0F
            };

                Vectors.Vector2DFloat prior = P0, current;
                for (int i = 1; i < arrayMax; i++)
                {
                    current = PointOnCurve(i / (float)ArcLengthDivisionCount);
                    _ArcLength += current.Distance(prior);
                    _ArcLengths.Add(_ArcLength);
                    prior = current;
                }
            }

            public override bool Equals(object obj)
            {
                return this.Equals((BezierCubic2D)obj);
            }
            public bool Equals(BezierCubic2D p)
            {
                // If parameter is null, return false.
                if (p == null)
                {
                    return false;
                }

                // Optimization for a common success case.
                if (this == p)
                {
                    return true;
                }

                // If run-time types are not exactly the same, return false.
                if (this.GetType() != p.GetType())
                {
                    return false;
                }

                // Return true if the fields match.
                // Note that the base class is not invoked because it is
                // System.Object, which defines Equals as reference equality.
                return (P0 == p.P0) && (P1 == p.P1) && (P2 == p.P2) && (P3 == p.P3);
            }
            public override int GetHashCode()
            {
                return P0.GetHashCode() + P1.GetHashCode() + P2.GetHashCode() + P3.GetHashCode() % int.MaxValue;
            }
            public static bool operator ==(BezierCubic2D first, BezierCubic2D second)
            {
                // Check for null on left side.
                if (first == null)
                {
                    if (second == null)
                    {
                        // null == null = true.
                        return true;
                    }

                    // Only the left side is null.
                    return false;
                }
                // Equals handles case of null on right side.
                return first.Equals(second);
            }
            public static bool operator !=(BezierCubic2D first, BezierCubic2D second)
            {
                return !(first == second);
            }
        }
        public struct BezierSplineCubic2D
        {
            public BezierCubic2D[] Beziers;

            public BezierCubic2D this[int index] { get { return Beziers[index]; } }
            public int Length { get { return Beziers.Length; } }
            public List<float> ArcLengths { get { if (_ArcLengths.Count == 0) CalculateArcLength(); return _ArcLengths; } }
            public float ArcLength { get { if (_ArcLength == 0.0F) CalculateArcLength(); return _ArcLength; } }

            private List<float> _ArcLengths;
            private float _ArcLength;

            public BezierSplineCubic2D(Vectors.Vector2DFloat[] source)
            {
                if (source == null || source.Length < 4 || (source.Length - 4) % 3 != 0) { Beziers = null; _ArcLength = 0.0F; _ArcLengths = new List<float>(); return; }
                int length = ((source.Length - 4) / 3) + 1;
                Beziers = new BezierCubic2D[length];
                Beziers[0] = new BezierCubic2D(source[0], source[1], source[2], source[3]);
                for (int i = 1; i < length; i++)
                    Beziers[i] = new BezierCubic2D(source[(i * 3)], source[(i * 3) + 1], source[(i * 3) + 2], source[(i * 3) + 3]);
                _ArcLength = 0.0F;
                _ArcLengths = new List<float>();
            }
            public BezierSplineCubic2D(System.Drawing.PointF[] source)
            {
                if (source == null || source.Length < 4 || (source.Length - 4) % 3 != 0) { Beziers = null; _ArcLength = 0.0F; _ArcLengths = new List<float>(); return; }
                int length = ((source.Length - 4) / 3) + 1;
                Beziers = new BezierCubic2D[length];
                Beziers[0] = new BezierCubic2D(source[0], source[1], source[2], source[3]);
                for (int i = 1; i < length; i++)
                    Beziers[i] = new BezierCubic2D(source[(i * 3)], source[(i * 3) + 1], source[(i * 3) + 2], source[(i * 3) + 3]);
                _ArcLength = 0.0F;
                _ArcLengths = new List<float>();
            }
            public BezierSplineCubic2D(System.Drawing.Point[] source)
            {
                if (source == null || source.Length < 4 || (source.Length - 4) % 3 != 0) { Beziers = null; _ArcLength = 0.0F; _ArcLengths = new List<float>(); return; }
                int length = ((source.Length - 4) / 3) + 1;
                Beziers = new BezierCubic2D[length];
                Beziers[0] = new BezierCubic2D(source[0], source[1], source[2], source[3]);
                for (int i = 1; i < length; i++)
                    Beziers[i] = new BezierCubic2D(source[(i * 3)], source[(i * 3) + 1], source[(i * 3) + 2], source[(i * 3) + 3]);
                _ArcLength = 0.0F;
                _ArcLengths = new List<float>();
            }

            public bool FindTAtPointOnSpline(float distanceAlongSpline, out BezierCubic2D bezier, out float t)
            {
                // to do: cache last distance and bezier. if new distance > old then start from old bezier.
                if (distanceAlongSpline > ArcLength) { bezier = Beziers[Beziers.Length - 1]; t = distanceAlongSpline / ArcLength; return false; }
                if (distanceAlongSpline <= 0.0F)
                {
                    bezier = Beziers[0];
                    t = 0.0F;
                    return true;
                }
                for (int i = 0; i < Beziers.Length; i++)
                {
                    float distanceRemainingBeyondCurrentBezier = distanceAlongSpline - Beziers[i].ArcLength;
                    if (distanceRemainingBeyondCurrentBezier < 0.0F)
                    {
                        // t is in current bezier.
                        bezier = Beziers[i];
                        t = bezier.FindTAtPointOnBezier(distanceAlongSpline);
                        return true;
                    }
                    else if (distanceRemainingBeyondCurrentBezier == 0.0F)
                    {
                        // t is 1.0F. Bezier is current one.
                        bezier = Beziers[i];
                        t = 1.0F;
                        return true;
                    }
                    // reduce the distance by the length of the bezier.
                    distanceAlongSpline -= Beziers[i].ArcLength;
                }
                // point is outside the spline.
                bezier = new BezierCubic2D();
                t = 0.0F;
                return false;
            }
            public void BendShapeToSpline(System.Drawing.RectangleF bounds, int dataLength, ref System.Drawing.PointF[] data, ref byte[] dataTypes)
            {
                System.Drawing.PointF pt;
                // move the origin for the data to 0,0
                float left = bounds.Left, height = bounds.Y + bounds.Height;

                for (int i = 0; i < dataLength; i++)
                {
                    pt = data[i];
                    float textX = pt.X - left;
                    float textY = pt.Y - height;

                    if (FindTAtPointOnSpline(textX, out BezierCubic2D bezier, out float t))
                    {
                        data[i] = bezier.PointOnCurvePerpendicular(t, true, textY).ToPointF();
                    }
                    else
                    {
                        // roll back all points until we reach curvedTypes[i] == 0
                        for (int j = i - 1; j > -1; j--)
                        {
                            if ((dataTypes[j] & 0x80) == 0x80)
                            {
                                System.Drawing.PointF[] temp1 = new System.Drawing.PointF[j + 1];
                                Array.Copy(data, 0, temp1, 0, j + 1);
                                byte[] temp2 = new byte[j + 1];
                                Array.Copy(dataTypes, 0, temp2, 0, j + 1);
                                data = temp1;
                                dataTypes = temp2;
                                break;
                            }
                        }
                        break;
                    }
                }
            }

            private void CalculateArcLength()
            {
                _ArcLength = 0.0F;
                _ArcLengths = new List<float>(Beziers.Length);
                for (int i = 0; i < Beziers.Length; i++)
                {
                    _ArcLength += Beziers[i].ArcLength;
                    _ArcLengths.Add(_ArcLength);
                }
            }

            internal static System.Drawing.PointF[] GetCurveTangents(System.Drawing.Point[] points, int count, float tension, int curveType)
            {
                if (points == null)
                    throw new ArgumentNullException("points");

                System.Drawing.PointF[] pointfs = new System.Drawing.PointF[count];
                for (int p = 0; p < count; p++)
                {
                    pointfs[p] = new System.Drawing.PointF(points[p].X, points[p].Y);
                }

                return GetCurveTangents(pointfs, count, tension, curveType);
            }
            internal static System.Drawing.PointF[] GetCurveTangents(System.Drawing.PointF[] points, int count, float tension, int curveType)
            {
                float coefficient = tension / 3f;
                System.Drawing.PointF[] tangents = new System.Drawing.PointF[count];

                if (count < 2)
                    return tangents;

                for (int i = 0; i < count; i++)
                {
                    int r = i + 1;
                    int s = i - 1;

                    if (r >= count)
                        r = count - 1;
                    if (curveType == 0) // 0 == CurveType.Open
                    {
                        if (s < 0)
                            s = 0;
                    }
                    else // 1 == CurveType.Closed, end point jumps to start point
                    {
                        if (s < 0)
                            s += count;
                    }

                    tangents[i].X += (coefficient * (points[r].X - points[s].X));
                    tangents[i].Y += (coefficient * (points[r].Y - points[s].Y));
                }

                return tangents;
            }
            internal static System.Drawing.PointF[] CreateCurve(System.Drawing.Point[] points, int offset, int length, int curveType)
            {
                if (points == null)
                    throw new ArgumentNullException("points");

                System.Drawing.PointF[] pointfs = new System.Drawing.PointF[length];
                for (int p = 0; p < length; p++)
                {
                    pointfs[p] = new System.Drawing.PointF(points[p].X, points[p].Y);
                }

                System.Drawing.PointF[] tangents = GetCurveTangents(pointfs, length, 0.5F, 0);
                return CreateCurve(pointfs, tangents, offset, length, curveType);
            }
            internal static System.Drawing.PointF[] CreateCurve(System.Drawing.Point[] points, System.Drawing.PointF[] tangents, int offset, int length, int curveType)
            {
                if (points == null)
                    throw new ArgumentNullException("points");

                System.Drawing.PointF[] pointfs = new System.Drawing.PointF[length];
                for (int p = 0; p < length; p++)
                {
                    pointfs[p] = new System.Drawing.PointF(points[p].X, points[p].Y);
                }

                return CreateCurve(pointfs, tangents, offset, length, curveType);
            }
            internal static System.Drawing.PointF[] CreateCurve(System.Drawing.PointF[] points, System.Drawing.PointF[] tangents, int offset, int length, int curveType)
            {
                List<System.Drawing.PointF> curve = new List<System.Drawing.PointF>();
                int i;

                Append(curve, points[offset].X, points[offset].Y, true);
                for (i = offset; i < offset + length - 1; i++)
                {
                    int j = i + 1;

                    float x1 = points[i].X + tangents[i].X;
                    float y1 = points[i].Y + tangents[i].Y;

                    float x2 = points[j].X - tangents[j].X;
                    float y2 = points[j].Y - tangents[j].Y;

                    float x3 = points[j].X;
                    float y3 = points[j].Y;

                    AppendBezier(curve, x1, y1, x2, y2, x3, y3, false);
                }
                return curve.ToArray<System.Drawing.PointF>();
            }
            internal static void Append(List<System.Drawing.PointF> points, float x, float y, bool compress)
            {
                System.Drawing.PointF pt = System.Drawing.PointF.Empty;

                /* in some case we're allowed to compress identical points */
                if (compress && (points.Count > 0))
                {
                    /* points (X, Y) must be identical */
                    System.Drawing.PointF lastPoint = points[points.Count - 1];
                    if ((lastPoint.X == x) && (lastPoint.Y == y))
                    {
                        return;
                    }
                }

                pt.X = x;
                pt.Y = y;

                points.Add(pt);
            }
            internal static void AppendBezier(List<System.Drawing.PointF> points, float x1, float y1, float x2, float y2, float x3, float y3, bool isReverseWindingOnFill)
            {
                if (isReverseWindingOnFill)
                {
                    Append(points, y1, x1, false);
                    Append(points, y2, x2, false);
                    Append(points, y3, x3, false);
                }
                else
                {
                    Append(points, x1, y1, false);
                    Append(points, x2, y2, false);
                    Append(points, x3, y3, false);
                }
            }

        }
    }
}