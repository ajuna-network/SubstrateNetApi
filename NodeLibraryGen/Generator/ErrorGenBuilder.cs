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
        {
            Success = true;
            Id = id;

            TargetUnit = new CodeCompileUnit();
            TargetUnit.Namespaces.Add(ImportsNamespace);

            if (typeDef.Path[0].Contains("_"))
            {
                NameSpace = "SubstrateNetApi.Model." + typeDef.Path[0].MakeMethod();
            }
            else
            {
                NameSpace = "SubstrateNetApi.Model." + "Base";
            }

            ClassName = typeDef.Path[0].MakeMethod() + "Error";
            ReferenzName = $"{NameSpace}.{ClassName}";
            CodeNamespace typeNamespace = new(NameSpace);
            TargetUnit.Namespaces.Add(typeNamespace);

            var palletName = typeDef.Path[0]
                .Replace("pallet_", "")
                .Replace("frame_", "")
                .MakeMethod();

            TargetClass = new CodeTypeDeclaration(ClassName)
            {
                IsEnum = true,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };

            TargetClass.Comments.Add(new CodeCommentStatement("<summary>", true));
            TargetClass.Comments.Add(new CodeCommentStatement($">> Path: {String.Join('.', typeDef.Path)}", true));
            foreach (var doc in typeDef.Docs)
            {
                TargetClass.Comments.Add(new CodeCommentStatement(doc, true));
            }
            TargetClass.Comments.Add(new CodeCommentStatement("</summary>", true));
            typeNamespace.Types.Add(TargetClass);


            if (typeDef.Variants != null)
            {
                foreach (var variant in typeDef.Variants)
                {
                    var enumField = new CodeMemberField(ClassName, variant.Name);
                    enumField.Comments.Add(new CodeCommentStatement("<summary>", true));
                    enumField.Comments.Add(new CodeCommentStatement($">> Event: {variant.Name}", true));
                    foreach (var doc in variant.Docs)
                    {
                        enumField.Comments.Add(new CodeCommentStatement(doc, true));
                    }
                    enumField.Comments.Add(new CodeCommentStatement("</summary>", true));
                    TargetClass.Members.Add(enumField);
                }
            }
        }

        public static ErrorGenBuilder Create(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            return new ErrorGenBuilder(id, typeDef, typeDict);
        }


    }
}
