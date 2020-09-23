namespace Schnorrkel.Ristretto
{
    using Schnorrkel.Scalars;

    public class ProjectiveNielsPoint
    {
        public FieldElement51 Y_plus_X { get; set; }
        public FieldElement51 Y_minus_X { get; set; }
        public FieldElement51 Z { get; set; }
        public FieldElement51 T2d { get; set; }

        public ProjectiveNielsPoint()
        {
            Y_plus_X = new FieldElement51();
            Y_minus_X = new FieldElement51();
            Z = new FieldElement51();
            T2d = new FieldElement51();
        }

        public ProjectiveNielsPoint BitXor(ProjectiveNielsPoint a)
        {
            return new ProjectiveNielsPoint
            {
                Y_plus_X = Y_plus_X.BitXor(a.Y_plus_X),
                Y_minus_X = Y_minus_X.BitXor(a.Y_minus_X),
                Z = Z.BitXor(a.Z),
                T2d = T2d.BitXor(a.T2d),
            };
        }

        public ProjectiveNielsPoint BitAnd(uint a)
        {
            return new ProjectiveNielsPoint
            {
                Y_plus_X = Y_plus_X.BitAnd(a),
                Y_minus_X = Y_minus_X.BitAnd(a),
                Z = Z.BitAnd(a),
                T2d = T2d.BitAnd(a)
            };
        }

        public ProjectiveNielsPoint Negate()
        {
            return new ProjectiveNielsPoint
            {
                Y_plus_X = Y_plus_X.Negate(),
                Y_minus_X = Y_minus_X.Negate(),
                Z = Z.Negate(),
                T2d = T2d.Negate()
            };
        }

        public ProjectiveNielsPoint Copy()
        {
            return new ProjectiveNielsPoint
            {
                Y_plus_X = Y_plus_X,
                Y_minus_X = Y_minus_X,
                Z = Z,
                T2d = T2d
            };
        }

        public ProjectiveNielsPoint GetPoint()
        {
            return this;
        }

        public void FromPoint(ProjectiveNielsPoint a)
        {
            Y_plus_X = a.Y_plus_X;
            Y_minus_X = a.Y_minus_X;
            Z = a.Z;
            T2d = a.T2d;
        }

        public void ConditionalAssign(ProjectiveNielsPoint a, bool choice)
        {
            // if choice = 0, mask = (-0) = 0000...0000
            // if choice = 1, mask = (-1) = 1111...1111
            int mask = choice ? 0b1111_1111_1111_1111 : 0b0000_0000_0000_0000;

            //    *self ^= mask & (*self ^ *other);
            FromPoint(GetPoint().BitXor(BitXor(a).BitAnd((uint)mask)));
        }

        public void ConditionalNegate(bool choice)
        {
            var p = GetPoint();
            ConditionalAssign(p, choice);
        }
    }
}
