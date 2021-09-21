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
            : base(id, typeDef, typeDict)
        {
        }

        public static EventGenBuilder Create(uint id, NodeTypeVariant typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            return new EventGenBuilder(id, typeDef, typeDict);
        }

        public override BaseBuilder Create()
        {
            var typeDef = TypeDef as NodeTypeVariant;

            #region CREATE

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

            // add comment to class if exists
            TargetClass.Comments.AddRange(GetComments(typeDef.Docs, typeDef));

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

                    // add comment to variant if exists
                    eventClass.Comments.AddRange(GetComments(variant.Docs, null, variant.Name));

                    var codeTypeRef = new CodeTypeReference("BaseTuple");
                    if (variant.TypeFields != null)
                    {
                        foreach (var field in variant.TypeFields)
                        {
                            var fullItem = GetFullItemPath(field.TypeId);
                            codeTypeRef.TypeArguments.Add(new CodeTypeReference(fullItem.Item1));
                        }
                    }
                    eventClass.BaseTypes.Add(codeTypeRef);

                    TargetClass.Members.Add(eventClass);
                }
            }

            #endregion

            return this;
        }

    }
}
