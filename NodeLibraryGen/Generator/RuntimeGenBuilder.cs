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
    public class RuntimeGenBuilder : BaseBuilder
    {
        private RuntimeGenBuilder(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            Success = true;
            Id = id;

            TargetUnit = new CodeCompileUnit();
            TargetUnit.Namespaces.Add(ImportsNamespace);

            var runtimeType = $"{typeDef.Path.Last()}";
            var enumName = $"Node{runtimeType}";

            if (typeDef.Path[0].Contains("_"))
            {
                NameSpace = "SubstrateNetApi.Model." + typeDef.Path[0].MakeMethod();
            }
            else
            {
                NameSpace = "SubstrateNetApi.Model." + "Base";
            }

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
            TargetClass.Comments.Add(new CodeCommentStatement("<summary>", true));
            TargetClass.Comments.Add(new CodeCommentStatement($">> {String.Join('.', typeDef.Path)}", true));
            if (typeDef.Docs != null)
            {
                foreach (var doc in typeDef.Docs)
                {
                    TargetClass.Comments.Add(new CodeCommentStatement(doc, true));
                }
            }
            TargetClass.Comments.Add(new CodeCommentStatement("</summary>", true));

            typeNamespace.Types.Add(TargetClass);

        }

        public static RuntimeGenBuilder Create(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            return new RuntimeGenBuilder(id, typeDef, typeDict);
        }
    }
}
