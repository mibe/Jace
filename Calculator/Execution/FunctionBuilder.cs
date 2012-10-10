﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Calculator.Operations;
using Calculator.Util;

namespace Calculator.Execution
{
    public class FunctionBuilder
    {
        private readonly CalculationEngine engine;

        private string functionText;
        private DataType? resultDataType;
        private List<ParameterInfo> parameters;

        /// <summary>
        /// Creates a new instance of the FunctionBuilder class.
        /// </summary>
        /// <param name="functionText">
        /// A calculation engine instance that can be used for interpreting and executing 
        /// the function.
        /// </param>
        internal FunctionBuilder(string functionText, CalculationEngine engine)
        {
            this.parameters = new List<ParameterInfo>();
            this.functionText = functionText;
            this.engine = engine;
        }

        public FunctionBuilder Parameter(string name, DataType dataType)
        {
            parameters.Add(new ParameterInfo() {Name = name, DataType = dataType});
            return this;
        }

        public FunctionBuilder Result(DataType dataType)
        {
            resultDataType = dataType;
            return this;
        }

        public Delegate Build()
        {
            if (!resultDataType.HasValue)
                throw new Exception("Please define a result data type for the function.");

            Func<Dictionary<string, double>, double> function = engine.Build(functionText);

            FuncAdapter adapter = new FuncAdapter();
            return adapter.Wrap(parameters, variables => function(variables));
        }
    }
}
