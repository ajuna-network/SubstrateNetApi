/*
 * Copyright (C) 2020 Usetech Professional
 * Modifications: copyright (C) 2021 DOT Mog
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using Schnorrkel.Scalars;

namespace Schnorrkel.Ristretto
{
    public class ProjectivePoint
    {
        public FieldElement51 X { get; set; }
        public FieldElement51 Y { get; set; }
        public FieldElement51 Z { get; set; }

        public CompletedPoint Double()
        {
            var XX = X.Square();
            var YY = Y.Square();
            var ZZ2 = Z.Square2();
            var X_plus_Y = X.Add(Y);
            var X_plus_Y_sq = X_plus_Y.Square();
            var YY_plus_XX = YY.Add(XX);
            var YY_minus_XX = YY.Sub(XX);

            return new CompletedPoint
            {
                X = X_plus_Y_sq.Sub(YY_plus_XX),
                Y = YY_plus_XX,
                Z = YY_minus_XX,
                T = ZZ2.Sub(YY_minus_XX)
            };
        }

        internal static ProjectivePoint Identity()
        {
            return new ProjectivePoint
            {
                X = FieldElement51.Zero(),
                Y = FieldElement51.One(),
                Z = FieldElement51.One()
            };
        }

        internal EdwardsPoint ToExtended()
        {
            return new EdwardsPoint
            {
                X = X * Z,
                Y = Y * Z,
                Z = Z.Square(),
                T = X * Y
            };
        }
    }
}
