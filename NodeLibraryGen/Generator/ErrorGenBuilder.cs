using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NodeLibraryGen
{
    public class ErrorGenBuilder : BaseBuilder
    {
        private ErrorGenBuilder(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict) 
            : base(id, typeDef, typeDict)
        {
        }

        public static ErrorGenBuilder Create(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            return new ErrorGenBuilder(id, typeDef, typeDict);
        }

        public override BaseBuilder Create()
        {
            var typeDef = TypeDef as NodeTypeVariant;

            #region CREATE

            ClassName = typeDef.Path[0].MakeMethod() + "Error";
            ReferenzName = $"{NameSpace}.{ClassName}";
            CodeNamespace typeNamespace = new(NameSpace);
            TargetUnit.Namespaces.Add(typeNamespace);

            TargetClass = new CodeTypeDeclaration(ClassName)
            {
                IsEnum = true,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };

            // add comment to class if exists
            TargetClass.Comments.AddRange(GetComments(typeDef.Docs, typeDef));

            typeNamespace.Types.Add(TargetClass);

            if (typeDef.Variants != null)
            {
                foreach (var variant in typeDef.Variants)
                {
                    var enumField = new CodeMemberField(ClassName, variant.Name);

                    // add comment to field if exists
                    enumField.Comments.AddRange(GetComments(variant.Docs, null, variant.Name));

                    TargetClass.Members.Add(enumField);
                }
            }
            #endregion

            return this;
        }
    }
}
