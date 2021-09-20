using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NodeLibraryGen
{
    public class BaseBuilder
    {
        internal bool Success { get; set; }

        internal uint Id { get; set; }

        public string NameSpace { get; set; }

        internal string ClassName { get; set; }

        internal string ReferenzName { get; set; }

        internal CodeNamespace ImportsNamespace { get; set; } = new()
        {
            Imports = {
                new CodeNamespaceImport("SubstrateNetApi.Model.Types.Base"),
                new CodeNamespaceImport("System.Collections.Generic"),
                new CodeNamespaceImport("System")
            }
        };

        internal CodeCompileUnit TargetUnit { get; set; }

        internal CodeTypeDeclaration TargetClass { get; set; }

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