using System.Linq;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

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
        private static bool AreHashesRead;
        private static IDictionary<String, KeyValuePair<String, String>> Hashes;
        private static bool IsPublicKeyRead;
        private static ICollection<String> LoadedAssemblies;
        private static IDictionary<String, IInstruction> LoadedInstructions;
        internal static StringComparer OpcodeComparer;
        private static StrongNameKeyPair PublicKey;
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
            JITCompiler.Hashes = new Dictionary<String, KeyValuePair<String, String>>();
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

        private static String GetHash(String filename, out HashAlgorithm algorithm)
		{
            if (!JITCompiler.AreHashesRead)
			{
                JITCompiler.ReadHashes();
			}

            if (!JITCompiler.Hashes.ContainsKey(filename))
            {
                algorithm = null;
                return null;
			}

            KeyValuePair<String, String> hashInfo = JITCompiler.Hashes[filename];

            switch (hashInfo.Key.ToLower())
            {
                case "md5":
                    algorithm = new MD5CryptoServiceProvider();
                    break;
                case "sha1":
                    algorithm = new SHA1CryptoServiceProvider();
                    break;
                case "sha256":
                    algorithm = new SHA256CryptoServiceProvider();
                    break;
                case "sha384":
                    algorithm = new SHA384CryptoServiceProvider();
                    break;
                case "sha512":
                    algorithm = new SHA512CryptoServiceProvider();
                    break;
                default:
                    throw new InvalidHashException(filename);
            }

            return hashInfo.Value;
		}

        private static bool IsAlgorithmValid(String algorithm)
		{
            if (algorithm is null)
			{
                return false;
			}

            switch (algorithm.ToLower())
			{
                case "sha1":
                case "sha256":
                case "md5":
                    return true;
                default:
                    return false;
			}
		}

        private static bool IsHashValid(String hash)
		{
            if (hash is null)
			{
                return false;
			}                

            foreach (char c in hash)
			{
                if (!(Char.IsDigit(c) || Char.IsLower(c) && c >= 'a' && c <= 'f' || Char.IsUpper(c) && c >= 'A' && c <= 'F' || c == '-'))
				{
                    return false;
				}
			}

            return true;
		}

        private static void ReadHashes()
        {
            String hashJson = File.ReadAllText("assemblyHash.json");

            using (JsonDocument document = JsonDocument.Parse(hashJson))
            {
                foreach (JsonProperty property in document.RootElement.EnumerateObject())
                {
                    if (property.Name.ToLower().Equals("assemblyhashes"))
                    {
                        foreach (JsonElement item in property.Value.EnumerateArray())
                        {
                            foreach (JsonProperty fileInfo in item.EnumerateObject())
							{
                                try
                                {
                                    String algorithm = null, hash = null;

                                    foreach (JsonProperty fileHashInfo in fileInfo.Value.EnumerateObject())
                                    {
                                        String name = fileHashInfo.Name.ToLower();

                                        if (name.Equals("algorithm"))
                                        {
                                            algorithm = fileHashInfo.Value.GetString();
                                        }

                                        else if (name.Equals("hash"))
                                        {
                                            hash = fileHashInfo.Value.GetString();
                                        }
                                    }

                                    if (!JITCompiler.Hashes.ContainsKey(fileInfo.Name) && JITCompiler.IsAlgorithmValid(algorithm) && JITCompiler.IsHashValid(hash))
									{
                                        String name = fileInfo.Name.StartsWith(Environment.CurrentDirectory) ? fileInfo.Name : Environment.CurrentDirectory + Path.DirectorySeparatorChar + fileInfo.Name;
                                        KeyValuePair<String, String> hashInfo = new KeyValuePair<String, String>(algorithm, hash);
                                        JITCompiler.Hashes.Add(name, hashInfo);
									}
                                }

                                catch { }
							}
                        }
                    }
                }
            }

            JITCompiler.AreHashesRead = true;
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
                if (!JITCompiler.IsFileAssembly(filename))
                {
                    continue;
                }

                if (!JITCompiler.VerifyAssembly(filename))
                {
                    throw new InvalidHashException(filename);
                }

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

        internal static bool IsFileAssembly(String filename)
		{
            try
			{
                AssemblyName name = AssemblyName.GetAssemblyName(filename);
                return !filename.EndsWith("SVM.dll"); // Ensures not checking hash file for current assembly
			}

            catch
			{
                return false;
			}
		}

        internal static bool VerifyAssembly(String filename) => JITCompiler.VerifyHash(filename, out Exception ex) && JITCompiler.VerifyStrongName(filename);

        private static bool VerifyStrongName(String filename)
		{
            return true; // Can't do anything until John replies with replacement

            try
			{
                AssemblyName asmName = AssemblyName.GetAssemblyName(filename);
                byte[] asmPublicKey = asmName.GetPublicKey();
                byte[] publicKey = JITCompiler.GetPublicKey().PublicKey;

                if (asmPublicKey.Length != publicKey.Length)
				{
                    return false;
				}

                for (int i = 0; i < publicKey.Length; i ++)
				{
                    if (publicKey[i] != asmPublicKey[i])
					{
                        return false;
					}
				}

                return true;
			}

            catch (Exception ex)
			{
                return false;
			}
		}

        private static StrongNameKeyPair GetPublicKey()
		{
            if (!JITCompiler.IsPublicKeyRead)
			{
                byte[] bytes = File.ReadAllBytes("SVM.snk");
                JITCompiler.PublicKey = new StrongNameKeyPair(bytes);
                JITCompiler.IsPublicKeyRead = true;
			}

            return JITCompiler.PublicKey;
		}

        private static bool VerifyHash(String filename, out Exception ex)
		{
            try
			{
                ex = null;
                String hash = JITCompiler.GetHash(filename, out HashAlgorithm hashAlgorithm);

                if (hash is null)
				{
                    return false;
				}

                byte[] bytes = File.ReadAllBytes(filename);
                byte[] hashedBytes = hashAlgorithm.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();

                foreach (byte b in hashedBytes)
				{
                    sb.AppendFormat("{0:X2}", b);
                    sb.Append('-');
				}

                String computedHash = sb.ToString();
                computedHash = computedHash.TrimEnd('-');
                computedHash = computedHash.ToUpper();
                hash = hash.ToUpper();
                return computedHash.Equals(hash) || computedHash.Replace("-", String.Empty).Equals(hash);
			}

            catch (Exception innerEx)
			{
                ex = innerEx;
                return false;
			}
		}
        #endregion

    }
}
