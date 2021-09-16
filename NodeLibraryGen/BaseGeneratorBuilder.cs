using SubstrateNetApi.Model.Types.Metadata.V14;
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
    public class BaseGeneratorBuilder
    {
        private NodeTypePrimitive _nodeType;

        private string _className;

        /// <summary>
        /// Define the compile unit to use for code generation.
        /// </summary>
        private readonly CodeCompileUnit _targetUnit;

        /// <summary>
        /// The only class in the compile unit. This class contains 2 fields,
        /// 3 properties, a constructor, an entry point, and 1 simple method.
        /// </summary>
        private readonly CodeTypeDeclaration _targetClass;

        private BaseGeneratorBuilder(NodeTypePrimitive nodeType)
        {
            _nodeType = nodeType;
            _className = "Prim"  + nodeType.Primitive.ToString();
            
            _targetUnit = new CodeCompileUnit();

            CodeNamespace generatedCode = new CodeNamespace("SubstrateNetApi.Model.Types.Base");
            
            generatedCode.Imports.Add(new CodeNamespaceImport("System"));
            generatedCode.Imports.Add(new CodeNamespaceImport("SubstrateNetApi.Model.Types.Base"));

            _targetClass = new CodeTypeDeclaration(_className)
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };

            Type type = GetPrimitveMap(nodeType.Primitive, out int size);

            _targetClass.BaseTypes.Add(new CodeTypeReference($"BaseType<{type.Name}>"));
            generatedCode.Types.Add(_targetClass);

            _targetUnit.Namespaces.Add(generatedCode);

            // Declaring a Name method
            _targetClass.Members.Add(GetCodeMemberMethod("Name", typeof(System.String), nodeType.Primitive.ToString().FirstCharToLowerCase()));

            // Declaring a Size method
            _targetClass.Members.Add(GetCodeMemberMethod("Size", typeof(System.Int32), size));

            // Declaring a Encode method
            var encodeMethod = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Override,
                Name = "Encode",
                ReturnType = new CodeTypeReference(typeof(System.Byte[]))
            };
            var sizeReturnStatement = new CodeMethodReturnStatement
            {
                Expression = new CodePrimitiveExpression(size)
            };
            encodeMethod.Statements.Add(sizeReturnStatement);
            _targetClass.Members.Add(encodeMethod);
        }

        private CodeMemberMethod GetCodeMemberMethod(string name, Type type, object returnStatement)
        {
            var method = new CodeMemberMethod
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Override,
                Name = name,
                ReturnType = new CodeTypeReference(type)
            };
            var methodReturnStatment = new CodeMethodReturnStatement
            {
                Expression = new CodePrimitiveExpression(returnStatement)
            };
            method.Statements.Add(methodReturnStatment);

            return method;
        }

        private Type GetPrimitveMap(TypeDefPrimitive typeDefPrimitive, out int size)
        {
            size = 0;
            switch (typeDefPrimitive)
            {
                case TypeDefPrimitive.Bool:
                    size = 1;
                    return typeof(System.Boolean);
                
                case TypeDefPrimitive.Char:
                    size = 4;
                    return typeof(System.Char);
                
                case TypeDefPrimitive.Str:
                    size = 0;
                    return typeof(System.String);
                
                case TypeDefPrimitive.U8:
                    size = 1;
                    return typeof(System.Byte);
                
                case TypeDefPrimitive.U16:
                    size = 2;
                    return typeof(System.UInt16);
                
                case TypeDefPrimitive.U32:
                    size = 4;
                    return typeof(System.UInt32);
                
                case TypeDefPrimitive.U64:
                    size = 8;
                    return typeof(System.UInt64);
                
                case TypeDefPrimitive.U128:
                    size = 16;
                    throw new NotImplementedException($"Currently not implemented primitve {typeDefPrimitive}!");
                
                case TypeDefPrimitive.U256:
                    size = 32;
                    throw new NotImplementedException($"Currently not implemented primitve {typeDefPrimitive}!");
                
                case TypeDefPrimitive.I8:
                    size = 1;
                    return typeof(System.SByte);
                
                case TypeDefPrimitive.I16:
                    size = 2;
                    return typeof(System.Int16);
                
                case TypeDefPrimitive.I32:
                    size = 4;
                    return typeof(System.Int32);
                
                case TypeDefPrimitive.I64:
                    size = 8;
                    return typeof(System.Int64);
                
                case TypeDefPrimitive.I128:
                    size = 16;
                    throw new NotImplementedException($"Currently not implemented primitve {typeDefPrimitive}!");
                
                case TypeDefPrimitive.I256:
                    size = 32;
                    throw new NotImplementedException($"Currently not implemented primitve {typeDefPrimitive}!");
                
                default:
                    throw new NotImplementedException($"Currently not implemented primitve {typeDefPrimitive}!");
            }
        }

        public static BaseGeneratorBuilder Init(NodeTypePrimitive primitive)
        {
            return new BaseGeneratorBuilder(primitive);
        }

        public BaseGeneratorBuilder AddTypeFields(NodeTypeField[] typeFields)
        {

            return this;
        }

        public string Build()
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions
            {
                BracingStyle = "C"
            };

            using StringWriter sourceWriter = new(new StringBuilder());

            provider.GenerateCodeFromCompileUnit(
                _targetUnit, sourceWriter, options);

            return sourceWriter.ToString();
        }
    }
}
