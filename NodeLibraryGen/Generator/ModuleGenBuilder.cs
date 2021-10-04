using Newtonsoft.Json;
using SubstrateNetApi;
using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.Extrinsics;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Types;
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
        public class ModuleGenBuilder : ModuleBuilder
        {
            private ModuleGenBuilder(uint id, PalletModule module, Dictionary<uint, (string, List<string>)> typeDict, Dictionary<uint, NodeType> nodeTypes) :
                base(id, module, typeDict, nodeTypes)
            {
            }

            public static ModuleGenBuilder Init(uint id, PalletModule module, Dictionary<uint, (string, List<string>)> typeDict, Dictionary<uint, NodeType> nodeTypes)
            {
                return new ModuleGenBuilder(id, module, typeDict, nodeTypes);
            }

            public override ModuleGenBuilder Create()
            {
                #region CREATE

                ImportsNamespace.Imports.Add(new CodeNamespaceImport("System.Threading.Tasks"));
                ImportsNamespace.Imports.Add(new CodeNamespaceImport("SubstrateNetApi.Model.Meta"));
                ImportsNamespace.Imports.Add(new CodeNamespaceImport("System.Threading"));
                ImportsNamespace.Imports.Add(new CodeNamespaceImport("SubstrateNetApi.Model.Types"));
                ImportsNamespace.Imports.Add(new CodeNamespaceImport("SubstrateNetApi.Model.Extrinsics"));

                FileName = "Main" + Module.Name;

                ReferenzName = "SubstrateNetApi.Model." + Module.Name;

                CodeNamespace typeNamespace = new(NameSpace);
                TargetUnit.Namespaces.Add(typeNamespace);

                CreateStorage(typeNamespace);

                CreateCalls(typeNamespace);

                CreateEvents(typeNamespace);

                CreateConstants(typeNamespace);

                CreateErrors(typeNamespace);

                #endregion

                return this;
            }

            private void CreateStorage(CodeNamespace typeNamespace)
            {
                ClassName = Module.Name + "Storage";

                var storage = Module.Storage;

                var targetClass = new CodeTypeDeclaration(ClassName)
                {
                    IsClass = true,
                    TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
                };
                typeNamespace.Types.Add(targetClass);

                // Declare the client field.
                CodeMemberField clientField = new CodeMemberField
                {
                    Attributes = MemberAttributes.Private,
                    Name = "_client",
                    Type = new CodeTypeReference("SubstrateClientExt")
                };
                clientField.Comments.Add(new CodeCommentStatement(
                    "Substrate client for the storage calls."));
                targetClass.Members.Add(clientField);

                CodeConstructor constructor = new CodeConstructor
                {
                    Attributes =
                    MemberAttributes.Public | MemberAttributes.Final
                };

                // Add parameters.
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(
                    clientField.Type, "client"));
                CodeFieldReferenceExpression fieldReference =
                    new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), "_client");
                constructor.Statements.Add(new CodeAssignStatement(fieldReference,
                    new CodeArgumentReferenceExpression("client")));

                CodeMethodInvokeExpression test_method = new(
                                new CodeTypeReferenceExpression("_client"),
                                "StorageKeyDict", Array.Empty<CodeExpression>());

                targetClass.Members.Add(constructor);

                if (storage?.Entries != null)
                {
                    foreach (var entry in storage.Entries)
                    {
                        var storageParams = entry.Name + "Params";
                        CodeMemberMethod parameterMethod = new()
                        {
                            Attributes = MemberAttributes.Static | MemberAttributes.Public | MemberAttributes.Final,
                            Name = storageParams,
                            ReturnType = new CodeTypeReference(typeof(string))
                        };
                        // add comment to class if exists
                        parameterMethod.Comments.AddRange(GetComments(entry.Docs, null, storageParams));
                        targetClass.Members.Add(parameterMethod);

                        // async Task<object>
                        CodeMemberMethod storageMethod = new()
                        {
                            Attributes = MemberAttributes.Public | MemberAttributes.Final,
                            Name = entry.Name,
                        };
                        // add comment to class if exists
                        storageMethod.Comments.AddRange(GetComments(entry.Docs, null, entry.Name));

                        targetClass.Members.Add(storageMethod);


                        if (entry.StorageType == SubstrateNetApi.Model.Meta.Storage.Type.Plain)
                        {
                            var fullItem = GetFullItemPath(entry.TypeMap.Item1);

                            parameterMethod.Statements.Add(new CodeMethodReturnStatement(
                                GetStorageString(storage.Prefix, entry.Name, entry.StorageType)));

                            storageMethod.ReturnType = new CodeTypeReference($"async Task<{fullItem.Item1}>");

                            storageMethod.Parameters.Add(new CodeParameterDeclarationExpression("CancellationToken", "token"));

                            CodeMethodInvokeExpression methodInvoke = new(
                                new CodeTypeReferenceExpression(targetClass.Name), 
                                parameterMethod.Name,Array.Empty<CodeExpression>());

                            CodeVariableDeclarationStatement variableDeclaration = new(typeof(string), "parameters", methodInvoke);

                            storageMethod.Statements.Add(variableDeclaration);

                            storageMethod.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(GetInvoceString(fullItem.Item1))));

                            // add storage key mapping in constructor
                            constructor.Statements.Add(
                                AddPropertyValues(GetStorageMapString("", storage.Prefix, entry.Name, entry.StorageType), "_client.StorageKeyDict"));

                        }
                        else if (entry.StorageType == SubstrateNetApi.Model.Meta.Storage.Type.Map)
                        {
                            var typeMap = entry.TypeMap.Item2;
                            var hashers = typeMap.Hashers;
                            var key = GetFullItemPath(typeMap.Key);
                            var value = GetFullItemPath(typeMap.Value);

                            parameterMethod.Parameters.Add(new CodeParameterDeclarationExpression(key.Item1, "key"));
                            parameterMethod.Statements.Add(new CodeMethodReturnStatement(
                                GetStorageString(storage.Prefix, entry.Name, entry.StorageType, hashers)));

                            storageMethod.ReturnType = new CodeTypeReference($"async Task<{value.Item1}>");
                            storageMethod.Parameters.Add(new CodeParameterDeclarationExpression(key.Item1, "key"));
                            storageMethod.Parameters.Add(new CodeParameterDeclarationExpression("CancellationToken", "token"));
                           
                            CodeMethodInvokeExpression methodInvoke = new(new CodeTypeReferenceExpression(targetClass.Name), parameterMethod.Name,
                                new CodeExpression[] { new CodeArgumentReferenceExpression("key") });
                            CodeVariableDeclarationStatement variableDeclaration = new(typeof(string), "parameters", methodInvoke);
                            storageMethod.Statements.Add(variableDeclaration);

                            storageMethod.Statements.Add(
                                new CodeMethodReturnStatement(
                                    new CodeArgumentReferenceExpression(GetInvoceString(value.Item1))));

                            // add storage key mapping in constructor
                            constructor.Statements.Add(
                                AddPropertyValues(GetStorageMapString(key.Item1, storage.Prefix, entry.Name, entry.StorageType, hashers), "_client.StorageKeyDict"));
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            }

            private void CreateCalls(CodeNamespace typeNamespace)
            {
                ClassName = Module.Name + "Calls";

                var calls = Module.Calls;

                var targetClass = new CodeTypeDeclaration(ClassName)
                {
                    IsClass = true,
                    TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
                };
                typeNamespace.Types.Add(targetClass);
               
                if (calls != null)
                {
                    if (NodeTypes.TryGetValue(calls.TypeId, out NodeType nodeType))
                    {
                        var typeDef = nodeType as NodeTypeVariant;

                        if (typeDef.Variants != null)
                        {
                            foreach (var variant in typeDef.Variants)
                            {
                                CodeMemberMethod callMethod = new()
                                {
                                    Attributes = MemberAttributes.Static | MemberAttributes.Public | MemberAttributes.Final,
                                    Name = variant.Name.MakeMethod(),
                                    ReturnType = new CodeTypeReference(typeof(Method).Name)
                                };

                                // add comment to class if exists
                                callMethod.Comments.AddRange(GetComments(typeDef.Docs, null, variant.Name));

                                var byteArrayName = "byteArray";

                                callMethod.Statements.Add(new CodeVariableDeclarationStatement(
                                    typeof(List<byte>), byteArrayName, new CodeObjectCreateExpression("List<byte>", Array.Empty<CodeExpression>())));

                                if (variant.TypeFields != null)
                                {

                                    foreach (var field in variant.TypeFields)
                                    {
                                        var fullItem = GetFullItemPath(field.TypeId);

                                        CodeParameterDeclarationExpression param = new()
                                        {
                                            Type = new CodeTypeReference(fullItem.Item1),
                                            Name = field.Name
                                        };
                                        callMethod.Parameters.Add(param);

                                        callMethod.Statements.Add(new CodeMethodInvokeExpression(
                                            new CodeVariableReferenceExpression(byteArrayName), "AddRange", new CodeMethodInvokeExpression(
                                            new CodeVariableReferenceExpression(field.Name), "Encode")));
                                    }
                                }

                                // return statment
                                var create = new CodeObjectCreateExpression(typeof(Method).Name, Array.Empty<CodeExpression>());
                                create.Parameters.Add(new CodePrimitiveExpression((int)Module.Index));
                                create.Parameters.Add(new CodePrimitiveExpression(Module.Name));
                                create.Parameters.Add(new CodePrimitiveExpression(variant.Index));
                                create.Parameters.Add(new CodePrimitiveExpression(variant.Name));
                                create.Parameters.Add(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression(byteArrayName), "ToArray"));
                                CodeMethodReturnStatement returnStatement = new()
                                {
                                    Expression = create
                                };

                                callMethod.Statements.Add(returnStatement);
                                targetClass.Members.Add(callMethod);
                            }
                        }
                    }
                }
            }

            private void CreateEvents(CodeNamespace typeNamespace)
            {
                ClassName = Module.Name + "Events";

                var events = Module.Events;


                if (events != null)
                {
                    if (NodeTypes.TryGetValue(events.TypeId, out NodeType nodeType))
                    {
                        var typeDef = nodeType as NodeTypeVariant;

                        foreach (var variant in typeDef.Variants)
                        {
                            var eventClass = new CodeTypeDeclaration("Event" + variant.Name.MakeMethod())
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

                            typeNamespace.Types.Add(eventClass);
                        }
                    }
                }
            }

            private void CreateConstants(CodeNamespace typeNamespace)
            {
                // TODO
            }

            private void CreateErrors(CodeNamespace typeNamespace)
            {
                ClassName = Module.Name + "Errors";

                var errors = Module.Errors;

                if (errors != null)
                {
                    if (NodeTypes.TryGetValue(errors.TypeId, out NodeType nodeType))
                    {
                        var typeDef = nodeType as NodeTypeVariant;

                        var targetClass = new CodeTypeDeclaration(ClassName)
                        {
                            IsEnum = true,
                            TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
                        };


                        foreach (var variant in typeDef.Variants)
                        {
                            var enumField = new CodeMemberField(ClassName, variant.Name);

                            // add comment to field if exists
                            enumField.Comments.AddRange(GetComments(variant.Docs, null, variant.Name));

                            targetClass.Members.Add(enumField);
                        }

                        typeNamespace.Types.Add(targetClass);
                    }
                }
            }

            private string GetInvoceString(string returnType)
            {
                return "await _client.GetStorageAsync<" + returnType + ">(parameters, token)";
            }

            private CodeMethodInvokeExpression GetStorageString(string module, string item, Storage.Type type, Storage.Hasher[] hashers = null)
            {

                CodeExpression[] codeExpressions = 
                    new CodeExpression[] {
                        new CodePrimitiveExpression(module),
                        new CodePrimitiveExpression(item),
                        new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(Storage.Type)), type.ToString())
                 };

                // if it is a map fill hashers and key
                if (hashers != null && hashers.Length > 0)
                {

                    codeExpressions = new CodeExpression[] {
                        new CodePrimitiveExpression(module),
                        new CodePrimitiveExpression(item),
                        new CodePropertyReferenceExpression(
                            new CodeTypeReferenceExpression(typeof(Storage.Type)), type.ToString()),
                        new CodeArrayCreateExpression(
                            new CodeTypeReference(typeof(Storage.Hasher)),
                                hashers.Select(p => new CodePropertyReferenceExpression(
                                    new CodeTypeReferenceExpression(typeof(Storage.Hasher)), p.ToString())).ToArray()),
                        new CodeArrayCreateExpression(
                            new CodeTypeReference(typeof(IType)),
                            new CodeArgumentReferenceExpression[] { 
                                new CodeArgumentReferenceExpression("key") })
                    };
                }

                return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("RequestGenerator"), "GetStorage", codeExpressions);
            }

            private CodeExpression[] GetStorageMapString(string keyType, string module, string item, Storage.Type type, Storage.Hasher[] hashers = null)
            {
                CodeExpression[] result = new CodeExpression[] {
                    new CodeObjectCreateExpression(
                            new CodeTypeReference("System.Tuple<string,string>"),
                            new CodeExpression[] {
                                new CodePrimitiveExpression(module),
                                new CodePrimitiveExpression(item)
                            }),
                    new CodeObjectCreateExpression(
                        new CodeTypeReference("System.Tuple<Storage.Hasher[],Type>"),
                        new CodeExpression[] {
                            new CodePrimitiveExpression(null),
                            new CodePrimitiveExpression(null) })
                };

                // if it is a map fill hashers and key
                if (hashers != null && hashers.Length > 0)
                {
                    var arrayExpression = new CodeArrayCreateExpression(
                                new CodeTypeReference(typeof(Storage.Hasher)),
                                    hashers.Select(p => new CodePropertyReferenceExpression(
                                        new CodeTypeReferenceExpression(typeof(Storage.Hasher)), p.ToString())).ToArray());
                    var typeofType = new CodeTypeOfExpression(keyType);

                    result = new CodeExpression[] {
                            new CodeObjectCreateExpression(
                                new CodeTypeReference("System.Tuple<string,string>"),
                                new CodeExpression[] {
                                    new CodePrimitiveExpression(module),
                                    new CodePrimitiveExpression(item)
                            }),
                        new CodeObjectCreateExpression(
                            new CodeTypeReference("System.Tuple<Storage.Hasher[],Type>"),
                            new CodeExpression[] { 
                                arrayExpression, 
                                typeofType
                            })
                    };
                }

                return result;
            }

            private CodeStatement AddPropertyValues(CodeExpression[] exprs, string variableReference)
            {
                return new CodeExpressionStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            new CodeTypeReferenceExpression(
                                new CodeTypeReference(variableReference)), "Add"), exprs));
            }
        }

    }

}
