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
    public class EventGenBuilder : BaseBuilder
    {
        private EventGenBuilder(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict)
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

            ClassName = typeDef.Path[0].MakeMethod() + "Event";

            ReferenzName = $"{NameSpace}.{ClassName}";

            CodeNamespace typeNamespace = new(NameSpace);
            
            TargetUnit.Namespaces.Add(typeNamespace);

            var palletName = typeDef.Path[0]
                .Replace("pallet_", "")
                .Replace("frame_", "")
                .MakeMethod();

            TargetClass = new CodeTypeDeclaration(ClassName)
            {
                IsClass = true,
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
                    var eventClass = new CodeTypeDeclaration(variant.Name.MakeMethod())
                    {
                        IsClass = true,
                        TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
                    };

                    eventClass.Comments.Add(new CodeCommentStatement("<summary>", true));
                    eventClass.Comments.Add(new CodeCommentStatement($">> Event: {variant.Name}", true));
                    foreach (var doc in variant.Docs)
                    {
                        eventClass.Comments.Add(new CodeCommentStatement(doc, true));
                    }
                    eventClass.Comments.Add(new CodeCommentStatement("</summary>", true));

                    var codeTypeRef = new CodeTypeReference("BaseTuple");
                    if (variant.TypeFields != null)
                    {
                        foreach (var field in variant.TypeFields)
                        {
                            if (typeDict.TryGetValue(field.TypeId, out (string, List<string>) fullItem))
                            {
                                codeTypeRef.TypeArguments.Add(new CodeTypeReference(fullItem.Item1));
                                //fullItem.Item2.ForEach(p => ImportsNamespace.Imports.Add(new CodeNamespaceImport(p)));
                            }
                            else
                            {
                                Success = false;
                            }
                        }
                    }
                    eventClass.BaseTypes.Add(codeTypeRef);

                    TargetClass.Members.Add(eventClass);
                }
            }



        }

        public static EventGenBuilder Create(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            return new EventGenBuilder(id, typeDef, typeDict);
        }
    }
}
