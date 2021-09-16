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
    public class StructGeneratorBuilder
    {
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

        private StructGeneratorBuilder(string[] path)
        {
            _className = path.Last();
            
            _targetUnit = new CodeCompileUnit();

            CodeNamespace generatedCode = new CodeNamespace("SubstrateNetApi.Model.Types.Struct");
            
            generatedCode.Imports.Add(new CodeNamespaceImport("System"));
            generatedCode.Imports.Add(new CodeNamespaceImport("SubstrateNetApi.Model.Types.Base"));


            _targetClass = new CodeTypeDeclaration(_className)
            {
                IsClass = true,
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
            };
            _targetClass.BaseTypes.Add(new CodeTypeReference("StructType"));
            generatedCode.Types.Add(_targetClass);

            _targetUnit.Namespaces.Add(generatedCode);
        }

        public static StructGeneratorBuilder Init(string[] path)
        {
            return new StructGeneratorBuilder(path);
        }

        public StructGeneratorBuilder AddTypeFields(NodeTypeField[] typeFields)
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
