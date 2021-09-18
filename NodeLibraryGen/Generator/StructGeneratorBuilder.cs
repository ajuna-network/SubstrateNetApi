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
        private Dictionary<uint, NodeType> _nodeTypes;

        private NodeTypeComposite _nodeType;

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

        private StructGeneratorBuilder(NodeTypeComposite nodeType, Dictionary<uint, NodeType> nodeTypes)
        {
            _nodeTypes = nodeTypes;
            _nodeType = nodeType;
            _className = nodeType.Path.Last();
            
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

            GenerateTypeFields(_nodeType.TypeFields);
        }

        private void GenerateTypeFields(NodeTypeField[] typeFields)
        {
            for (int i = 0; i < typeFields.Length; i++)
            {
                NodeTypeField typeField = typeFields[i];
                var nodeType = _nodeTypes[typeField.TypeId];
            }
        }

        public static StructGeneratorBuilder Create(NodeTypeComposite nodeTypeComposite, Dictionary<uint, NodeType> nodeTypes)
        {
            return new StructGeneratorBuilder(nodeTypeComposite, nodeTypes);
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
