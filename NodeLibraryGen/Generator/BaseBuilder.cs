using System;
using System.CodeDom;
using System.Linq;

namespace NodeLibraryGen
{
    public class BaseBuilder
    {
        internal uint Id { get; set; }

        internal string ClassName { get; set; }

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