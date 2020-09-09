namespace Schnorrkel.Ristretto
{
    using Schnorrkel.Scalars;

    public class CompletedPoint
    {
        public FieldElement51 X { get; set; }
        public FieldElement51 Y { get; set; }
        public FieldElement51 Z { get; set; }
        public FieldElement51 T { get; set; }

        public ProjectivePoint ToProjective()
        {
            return new ProjectivePoint
            {
                X = X.Mul(T),
                Y = Y.Mul(Z),
                Z = Z.Mul(T)
            };
        }

        public EdwardsPoint ToExtended()
        {
            return new EdwardsPoint
            {
                X = X.Mul(T),
                Y = Y.Mul(Z),
                Z = Z.Mul(T),
                T = X.Mul(Y)
            };
        }
    }
}
