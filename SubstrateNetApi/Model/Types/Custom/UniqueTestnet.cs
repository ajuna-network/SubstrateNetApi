using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Struct;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SubstrateNetApi.Model.Types.Custom
{
    #region BASE_TYPES

    public class DecimalPoints : U8
    {
        public override string Name() => "DecimalPoints";
    }

    #endregion

    #region ENUM_TYPES

    public enum AccessMode
    {
        Normal,
        WhiteList
    }

    #endregion

    #region STRUCT_TYPES


    #endregion



}
