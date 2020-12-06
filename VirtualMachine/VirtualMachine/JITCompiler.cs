using System.Linq;

namespace SVM.VirtualMachine
{
    #region Using directives
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    #endregion
    /// <summary>
    /// Utility class which generates compiles a textual representation
    /// of an SML instruction into an executable instruction instance
    /// </summary>
    internal static class JITCompiler
    {
        #region Constants
        #endregion

        #region Fields
        private static bool AreDomainTypesScanned;
        private static IDictionary<String, IInstruction> LoadedInstructions;
        private static StringComparer OpcodeComparer;

        // Represents C# types matching SML instructions found using Reflection
        // Key is short type name, for opcode matching with OpcodeComparer
        // First part of Value is assembly name, second is full type name
        // Scanned instructions are removed once they are loaded into LoadedInstructions
        private static readonly IDictionary<String, KeyValuePair<String, String>> ScannedInstructions;
        #endregion

        #region Constructors
        static JITCompiler()
		{
            StringComparer opcodeComparer = StringComparer.InvariantCultureIgnoreCase;
            JITCompiler.LoadedInstructions = new Dictionary<String, IInstruction>(opcodeComparer);
            JITCompiler.ScannedInstructions = new Dictionary<String, KeyValuePair<String, String>>(opcodeComparer);
            JITCompiler.OpcodeComparer = opcodeComparer;
        }
        #endregion

        #region Properties
        #endregion

        #region Public methods
        #endregion

        #region Non-public methods
        internal static IInstruction CompileInstruction(string opcode)
        {
            IInstruction instruction = null;

            #region TASK 1 - TO BE IMPLEMENTED BY THE STUDENT
            instruction = JITCompiler.GetInstruction(opcode);
            #endregion

            return instruction;
        }

        internal static IInstruction CompileInstruction(string opcode, params string[] operands)
        {
            IInstructionWithOperand instruction = null;

            #region TASK 1 - TO BE IMPLEMENTED BY THE STUDENT
            IInstruction retrievedInstruction = JITCompiler.CompileInstruction(opcode);
            Type instructionType = retrievedInstruction.GetType();

            try
            {
                instruction = (IInstructionWithOperand) Activator.CreateInstance(instructionType);
            }

            catch (InvalidCastException)
			{
                throw new SvmCompilationException();
			}

            instruction.Operands = operands;
            #endregion

            return instruction;
        }

        private static IInstruction GetInstruction(String opcode)
		{
            if (!JITCompiler.AreDomainTypesScanned)
			{
                JITCompiler.ScanDomainTypes();
			}

            try
			{
                return JITCompiler.LoadedInstructions[opcode];
			}

            catch (KeyNotFoundException) { }

            IInstruction instruction = JITCompiler.LoadInstruction(opcode);
            return instruction;
		}

        private static IInstruction LoadInstruction(String opcode)
		{
            KeyValuePair<String, String> value = new KeyValuePair<String, String>();
            String key = null;

            foreach (KeyValuePair<String, KeyValuePair<String, String>> keyValuePair in JITCompiler.ScannedInstructions)
			{
                if (JITCompiler.OpcodeComparer.Equals(opcode, keyValuePair.Key))
				{
                    key = keyValuePair.Key;
                    value = keyValuePair.Value;
                    break;
				}
			}

            if (key is null)
			{
                throw new SvmCompilationException();
			}

            Assembly asm = Assembly.Load(value.Key);
            Type type = asm.GetType(value.Value);
            IInstruction instruction = (IInstruction) Activator.CreateInstance(type);
            JITCompiler.LoadedInstructions.Add(opcode, instruction);
            JITCompiler.ScannedInstructions.Remove(key);
            return instruction;
		}

        private static void ScanAssemblyForInstructions(Assembly asm)
		{
            Type[] types;

            try
			{
                types = asm.GetTypes();
			}

            catch (ReflectionTypeLoadException ex)
			{
                types = ex.Types;
			}

            Type instructionType = typeof (IInstruction);

            foreach (Type type in types)
			{
                if (type is null)
				{
                    continue;
				}

                if (type.GetInterfaces().Contains(instructionType))
				{
                    if (!JITCompiler.ScannedInstructions.ContainsKey(type.Name))
					{
                        JITCompiler.ScannedInstructions.Add(type.Name, new KeyValuePair<String, String>(asm.FullName, type.FullName));
					}
				}
			}
		}

        private static void ScanDomainTypes()
		{
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
			{
                JITCompiler.ScanAssemblyForInstructions(asm);
			}

            JITCompiler.AreDomainTypesScanned = true;
		}
        #endregion

    }
}
