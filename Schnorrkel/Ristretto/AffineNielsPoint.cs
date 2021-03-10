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
    public class AffineNielsPoint
    {
        public FieldElement51 Y_plus_X { get; set; }
        public FieldElement51 Y_minus_X { get; set; }
        public FieldElement51 XY2d { get; set; }

        public AffineNielsPoint()
        {
            Y_plus_X = FieldElement51.One();
            Y_minus_X = FieldElement51.One();
            XY2d = FieldElement51.Zero();
        }

        public void ConditionalAssign(AffineNielsPoint a, bool choice)
        {
            Y_plus_X.ConditionalAssign(a.Y_plus_X, choice);
            Y_minus_X.ConditionalAssign(a.Y_minus_X, choice);
            XY2d.ConditionalAssign(a.XY2d, choice);
        }

        public AffineNielsPoint BitXor(AffineNielsPoint a)
        {
            return new AffineNielsPoint
            {
                Y_plus_X = Y_plus_X.BitXor(a.Y_plus_X),
                Y_minus_X = Y_minus_X.BitXor(a.Y_minus_X),
                XY2d = XY2d.BitXor(XY2d)
            };
        }

        public void ConditionalNegate(bool choice)
        {
            var nself = Negate();
            ConditionalAssign(nself, choice);
        }

        private AffineNielsPoint Negate()
        {
            return new AffineNielsPoint
            {
                Y_plus_X = this.Y_minus_X.Clone(),
                Y_minus_X = this.Y_plus_X.Clone(),
                XY2d = XY2d.Negate()
            };
        }
    }
}
