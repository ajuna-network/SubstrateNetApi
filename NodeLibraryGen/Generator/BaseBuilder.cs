using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace NodeLibraryGen
{
    public class BaseBuilder
    {
        internal bool Success { get; set; }

        internal uint Id { get; set; }

        public string NameSpace { get; set; }

        internal string ClassName { get; set; }

        internal List<string> Imports { get; } = new List<string>();

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


    }


}