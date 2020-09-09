using Schnorrkel.Ristretto;
using System;
using System.Collections.Generic;
using System.Text;
using static Schnorrkel.Ristretto.EdwardsBasepointTable;

namespace Schnorrkel.Scalars
{
    public class NafLookupTable5PNP
    {
        public ProjectiveNielsPoint[] Pnp { get; set; }

        internal ProjectiveNielsPoint Select(byte v)
        {
            return Pnp[v / 2];
        }
    }

    public class NafLookupTable
    {
        private LookupTable _lookupTable;

        public NafLookupTable(LookupTable lookupTable)
        {
            _lookupTable = lookupTable;
        }

        internal AffineNielsPoint Select(byte v)
        {
            return _lookupTable.affineNielsPoints[v / 2];
        }

        public static NafLookupTable5PNP FromEdwardsPoint(EdwardsPoint points)
        {
            ProjectiveNielsPoint[] Ai = new ProjectiveNielsPoint[8];

            for (var i = 0; i < 8; i++)
            {
                Ai[i] = points.ToProjectiveNiels();
            }

            var A2 = EdwardsPoint.Double(points);

            for(var i = 0; i <= 6; i++)
            {
                Ai[i + 1] = A2.Add(Ai[i]).ToExtended().ToProjectiveNiels();
            }

            /// Now Ai = [A, 3A, 5A, 7A, 9A, 11A, 13A, 15A]
            return new NafLookupTable5PNP { Pnp =  Ai };
        }
    }
}
