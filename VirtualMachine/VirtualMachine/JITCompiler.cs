using System.Linq;
using System.Runtime.Loader;

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
        private static ICollection<String> LoadedAssemblies;
        private static IDictionary<String, IInstruction> LoadedInstructions;
        internal static StringComparer OpcodeComparer;
        private static ICollection<String> ScannedAssemblies;

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
            JITCompiler.LoadedAssemblies = new HashSet<String>();
            JITCompiler.LoadedInstructions = new Dictionary<String, IInstruction>(opcodeComparer);
            JITCompiler.ScannedAssemblies = new HashSet<String>();
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

        internal static IEnumerable<Type> GetTypes(Assembly asm)
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

            List<Type> typeList = new List<Type>(types);
            typeList.RemoveAll((t) => t is null);
            return typeList;
        }

        private static IInstruction GetInstruction(String opcode)
		{
            if (!JITCompiler.AreDomainTypesScanned)
			{
                JITCompiler.ScanDomainTypes();
                JITCompiler.ScanDirectoryTypes();
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

            Assembly asm;

            try
            {
                asm = Assembly.Load(value.Key);
            }

			catch
			{
                try
                {
                    asm = Assembly.LoadFrom(value.Key);
                }

                catch
				{
                    throw new SvmCompilationException($"The requested assembly {value.Key} could not be found or loaded.");
				}
			}

            Type type = asm.GetType(value.Value);
            IInstruction instruction = (IInstruction) Activator.CreateInstance(type);
            JITCompiler.LoadedInstructions.Add(opcode, instruction);
            JITCompiler.ScannedInstructions.Remove(key);
            return instruction;
		}

        private static void ScanAssemblyForInstructions(Assembly asm)
		{
            Type instructionType = typeof (IInstruction);

            foreach (Type type in JITCompiler.GetTypes(asm))
			{
                if (type.GetInterfaces().Contains(instructionType))
				{
                    if (!JITCompiler.ScannedInstructions.ContainsKey(type.Name))
					{
                        JITCompiler.ScannedInstructions.Add(type.Name, new KeyValuePair<String, String>(asm.FullName, type.FullName));
					}
				}
			}
		}

        private static void ScanDirectoryTypes()
		{
            const String CONTEXT_NAME = "Compiler Reflection Context";
            AssemblyLoadContext reflectionContext = new AssemblyLoadContext(CONTEXT_NAME, true);

            foreach (String filename in Directory.EnumerateFiles(Environment.CurrentDirectory))
			{
                Assembly asm;

                try
				{
                    asm = reflectionContext.LoadFromAssemblyPath(filename);
				}

                catch
				{
                    continue;
				}

                Type instructionType = typeof (IInstruction);

                foreach (Type type in JITCompiler.GetTypes(asm))
				{
                    if (type.GetInterfaces().Contains(instructionType))
					{
                        JITCompiler.ScannedAssemblies.Add(asm.FullName);
                        JITCompiler.ScannedInstructions.Add(type.Name, new KeyValuePair<String, String>(filename, type.FullName));
					}
				}
			}

            reflectionContext.Unload();
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
