using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RuntimeMetadata
{
    public class RuntimeGenBuilder : BaseBuilder
    {
        private RuntimeGenBuilder(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict) 
            : base(id, typeDef, typeDict)
        {
        }

        public static RuntimeGenBuilder Create(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            return new RuntimeGenBuilder(id, typeDef, typeDict);
        }

        public override BaseBuilder Create()
        {
            var typeDef = TypeDef as NodeTypeVariant;

            #region CREATE

            var runtimeType = $"{typeDef.Path.Last()}";
            var enumName = $"Node{runtimeType}";

            ClassName = $"Enum{enumName}";
            ReferenzName = $"{NameSpace}.{ClassName}";
            CodeNamespace typeNamespace = new(NameSpace);
            TargetUnit.Namespaces.Add(typeNamespace);

            CodeTypeDeclaration TargetType = new(enumName)
            {
                IsEnum = true
            };

            if (typeDef.Variants != null)
            {
                foreach (var enumFieldName in typeDef.Variants.Select(p => p.Name))
                {
                    TargetType.Members.Add(new CodeMemberField(ClassName, enumFieldName));
                }
            }
            typeNamespace.Types.Add(TargetType);

            TargetClass = new CodeTypeDeclaration(ClassName)
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };
            TargetClass.BaseTypes.Add(new CodeTypeReference($"BaseEnum<{enumName}>"));

            // add comment to class if exists
            TargetClass.Comments.AddRange(GetComments(typeDef.Docs, typeDef));

            typeNamespace.Types.Add(TargetClass);

            #endregion

            return this;
        }
    }
}
