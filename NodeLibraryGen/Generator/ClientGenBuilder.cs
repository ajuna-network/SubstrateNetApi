using SubstrateNetApi;
using SubstrateNetApi.Model.Meta;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace RuntimeMetadata
{
    partial class Program
    {
        public class ClientGenBuilder : ClientBuilder
        {
            private ClientGenBuilder(uint id, List<(string, List<string>)> moduleNames, Dictionary<uint, (string, List<string>)> typeDict) :
                base(id, moduleNames, typeDict)
            {
            }

            public static ClientGenBuilder Init(uint id, List<(string, List<string>)> moduleNames, Dictionary<uint, (string, List<string>)> typeDict)
            {
                return new ClientGenBuilder(id, moduleNames, typeDict);
            }

            public override ClientGenBuilder Create()
            {
                #region CREATE
 
                ClassName = "SubstrateClientExt";
                NameSpace = "SubstrateNetApi";

                CodeNamespace typeNamespace = new(NameSpace);
                TargetUnit.Namespaces.Add(typeNamespace);

                TargetClass = new CodeTypeDeclaration(ClassName)
                {
                    IsClass = true,
                    TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
                };
                TargetClass.BaseTypes.Add(new CodeTypeReference(typeof(SubstrateClient)));
                typeNamespace.Types.Add(TargetClass);

                CodeConstructor constructor = new()
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Final
                };

                // Add parameters.
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(
                    typeof(Uri), "uri"));
                constructor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("uri"));

                TargetClass.Members.Add(constructor);

                foreach(var tuple in ModuleNames)
                {
                    var name = tuple.Item1.Split('.').Last();
                    //ImportsNamespace.Imports.Add(new CodeNamespaceImport(tuple.Item2[0]));

                    CodeMemberField clientField = new()
                    {
                        Attributes = MemberAttributes.Public,
                        Name = name,
                        Type = new CodeTypeReference(tuple.Item1)
                    };
                    clientField.Comments.Add(new CodeCommentStatement($"{name} storage calls."));
                    TargetClass.Members.Add(clientField);

                    CodeFieldReferenceExpression fieldReference =
                        new(new CodeThisReferenceExpression(), name);

                    var createPallet = new CodeObjectCreateExpression(tuple.Item1);
                    createPallet.Parameters.Add(new CodeThisReferenceExpression());

                    constructor.Statements.Add(new CodeAssignStatement(fieldReference,
                        createPallet));
                }



                #endregion

                return this;
            }

        }

    }

}
