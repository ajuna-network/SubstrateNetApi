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
        public class ModuleGenBuilder : ModuleBuilder
        {
            private ModuleGenBuilder(uint id, PalletModule module, Dictionary<uint, (string, List<string>)> typeDict) :
                base(id, module, typeDict)
            {
            }

            public static ModuleGenBuilder Init(uint id, PalletModule module, Dictionary<uint, (string, List<string>)> typeDict)
            {
                return new ModuleGenBuilder(id, module, typeDict);
            }

            public override ModuleGenBuilder Create()
            {
                #region CREATE

                ImportsNamespace.Imports.Add(new CodeNamespaceImport("System.Threading.Tasks"));
                ImportsNamespace.Imports.Add(new CodeNamespaceImport("SubstrateNetApi.Model.Meta"));
                ImportsNamespace.Imports.Add(new CodeNamespaceImport("System.Threading"));
                ImportsNamespace.Imports.Add(new CodeNamespaceImport("SubstrateNetApi.Model.Types"));

                ClassName = PrefixName + Module.Name + "Storage";

                var storage = Module.Storage;

                CodeNamespace typeNamespace = new(NameSpace);
                TargetUnit.Namespaces.Add(typeNamespace);

                TargetClass = new CodeTypeDeclaration(ClassName)
                {
                    IsClass = true,
                    TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
                };
                typeNamespace.Types.Add(TargetClass);

                // Declare the widthValue field.
                CodeMemberField clientField = new CodeMemberField
                {
                    Attributes = MemberAttributes.Private,
                    Name = "_client",
                    Type = new CodeTypeReference(typeof(SubstrateClient))
                };
                clientField.Comments.Add(new CodeCommentStatement(
                    "Substrate client for the storage calls."));
                TargetClass.Members.Add(clientField);

                CodeConstructor constructor = new CodeConstructor
                {
                    Attributes =
                    MemberAttributes.Public | MemberAttributes.Final
                };

                // Add parameters.
                constructor.Parameters.Add(new CodeParameterDeclarationExpression(
                    typeof(SubstrateClient), "client"));
                CodeFieldReferenceExpression fieldReference =
                    new CodeFieldReferenceExpression(
                    new CodeThisReferenceExpression(), "_client");
                constructor.Statements.Add(new CodeAssignStatement(fieldReference,
                    new CodeArgumentReferenceExpression("client")));

                TargetClass.Members.Add(constructor);

                if (storage?.Entries != null)
                {
                    foreach (var entry in storage.Entries)
                    {
                        // async Task<object>
                        CodeMemberMethod storageMethod = new()
                        {
                            Attributes = MemberAttributes.Public | MemberAttributes.Final,
                            Name = entry.Name,
                        };
                        // add comment to class if exists
                        storageMethod.Comments.AddRange(GetComments(entry.Docs, null, entry.Name));

                        TargetClass.Members.Add(storageMethod);

                        if (entry.StorageType == SubstrateNetApi.Model.Meta.Storage.Type.Plain)
                        {
                            var fullItem = GetFullItemPath(entry.TypeMap.Item1);

                            storageMethod.ReturnType = new CodeTypeReference($"async Task<{fullItem.Item1}>");

                            storageMethod.Parameters.Add(new CodeParameterDeclarationExpression("CancellationToken", "token"));
                            string getStorageString = GetStorageString(storage.Prefix, entry.Name, entry.StorageType);
                            storageMethod.Statements.Add(new CodeSnippetExpression(getStorageString));
                            storageMethod.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(GetInvoceString(fullItem.Item1))));

                        }
                        else if (entry.StorageType == SubstrateNetApi.Model.Meta.Storage.Type.Map)
                        {
                            var typeMap = entry.TypeMap.Item2;
                            var hashers = typeMap.Hashers;
                            var key = GetFullItemPath(typeMap.Key);
                            var value = GetFullItemPath(typeMap.Value);

                            storageMethod.ReturnType = new CodeTypeReference($"async Task<{value.Item1}>");
                            storageMethod.Parameters.Add(new CodeParameterDeclarationExpression(key.Item1, "key"));
                            storageMethod.Parameters.Add(new CodeParameterDeclarationExpression("CancellationToken", "token"));

                            var keyParamsString = hashers.Length == 1 ? "var keyParams = new IType[] { key }" : "var keyParams = key.Value";
                            storageMethod.Statements.Add(new CodeSnippetExpression(keyParamsString));
                            string getStorageString = GetStorageString(storage.Prefix, entry.Name, entry.StorageType, hashers);
                            storageMethod.Statements.Add(new CodeSnippetExpression(getStorageString));
                            storageMethod.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression(GetInvoceString(value.Item1))));
                        } 
                        else
                        {
                            throw new NotImplementedException();
                        }

                    }
                }

                #endregion

                return this;
            }

            private string GetInvoceString(string returnType)
            {
                return "await _client.GetStorageAsync<" + returnType + ">(parameters, token)";
            }

            private string GetStorageString(string module, string item, Storage.Type type, Storage.Hasher[] hashers = null)
            {
                string map = string.Empty;
                if (hashers != null && hashers.Length > 0)
                {
                    map = ", new[] {" + 
                        string.Join(",", hashers.Select(p => "Storage.Hasher." + p).ToArray()) + "}, keyParams";
                }
                return $"var parameters = RequestGenerator.GetStorage(\"{module}\", \"{item}\", Storage.Type.{type}{map})";
            }
        }

    }

}
