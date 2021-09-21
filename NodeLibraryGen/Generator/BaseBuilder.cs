using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RuntimeMetadata
{
    public abstract class BaseBuilder
    {
        internal uint Id { get; }

        internal NodeType TypeDef { get; }

        internal Dictionary<uint, (string, List<string>)> TypeDict { get; }

        internal bool Success { get; set; }

        public string NameSpace { get; set; }

        internal string ClassName { get; set; }

        internal string ReferenzName { get; set; }

        internal CodeNamespace ImportsNamespace { get; }

        internal BaseBuilder(uint id, NodeType typeDef, Dictionary<uint, (string, List<string>)> typeDict)
        {
            Id = id;
            TypeDef = typeDef;
            TypeDict = typeDict;
            ImportsNamespace = new()
            {
                Imports = {
                    new CodeNamespaceImport("SubstrateNetApi.Model.Types.Base"),
                    new CodeNamespaceImport("System.Collections.Generic"),
                    new CodeNamespaceImport("System")
                }
            };


            TargetUnit = new CodeCompileUnit();
            TargetUnit.Namespaces.Add(ImportsNamespace);

            Success = true;

            NameSpace = typeDef.Path != null && typeDef.Path[0].Contains("_") ?
                "SubstrateNetApi.Model." + typeDef.Path[0].MakeMethod()
                : "SubstrateNetApi.Model." + "Base";
        }

        internal (string, List<string>) GetFullItemPath(uint typeId)
        {
            if (!TypeDict.TryGetValue(typeId, out (string, List<string>) fullItem))
            {
                Success = false;
                fullItem = ("Unknown", new List<string>() { "Unknown" });
            } 
            else
            {
                fullItem.Item2.ForEach(p => ImportsNamespace.Imports.Add(new CodeNamespaceImport(p)));
            }

            return fullItem;
        }

        public static CodeCommentStatementCollection GetComments(string[] docs, NodeType typeDef = null, string typeName = null)
        {
            CodeCommentStatementCollection comments = new();
            comments.Add(new CodeCommentStatement("<summary>", true));

            if (typeDef != null)
            {
                var path = typeDef.Path != null ? "[" + String.Join('.', typeDef.Path) + "]" : "";
                comments.Add(new CodeCommentStatement($">> {typeDef.Id} - {typeDef.TypeDef}{path}", true));
            }

            if (typeName != null)
            {
                comments.Add(new CodeCommentStatement($">> {typeName}", true));
            }

            if (typeDef?.Docs != null)
            {
                foreach (var doc in typeDef.Docs)
                {
                    comments.Add(new CodeCommentStatement(doc, true));
                }
            }
            comments.Add(new CodeCommentStatement("</summary>", true));
            return comments;
        }

        internal CodeCompileUnit TargetUnit { get; set; }

        internal CodeTypeDeclaration TargetClass { get; set; }

        public  abstract BaseBuilder Create();

        public CodeMemberMethod SimpleMethod(string name, string returnType = null, object returnExpression = null)
        {
            CodeMemberMethod nameMethod = new()
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Override,
                Name = name
            };

            if (returnType != null)
            {
                nameMethod.ReturnType = new CodeTypeReference(returnType);
                CodeMethodReturnStatement nameReturnStatement = new()
                {
                    Expression = new CodePrimitiveExpression(returnExpression)
                };
                nameMethod.Statements.Add(nameReturnStatement);
            }
            return nameMethod;
        }

        public virtual (string, List<string>) Build(out bool success)
        {
            success = Success;
            if (Success)
            {
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                CodeGeneratorOptions options = new()
                {
                    BracingStyle = "C"
                };
                var space = NameSpace.Split('.').ToList();
                space.RemoveAt(0);
                space.Add(ClassName + ".cs");
                var path = Path.Combine(space.ToArray());
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (StreamWriter sourceWriter = new(path))
                {
                    provider.GenerateCodeFromCompileUnit(
                        TargetUnit, sourceWriter, options);
                }
            }
            return (ReferenzName, new List<string>() { NameSpace });
        }
    }
}