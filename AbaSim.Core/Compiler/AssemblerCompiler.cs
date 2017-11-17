using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AbaSim.Core.Virtualization.Abacus16;

namespace AbaSim.Core.Compiler
{
	public class AssemblerCompiler
	{
		public AssemblerCompiler() { }

		public string LineSperator { get; set; }

		public string Dialect { get; set; }

		protected Dictionary<string, int> Labels = new Dictionary<string, int>();

		protected Dictionary<string, InstructionMapping> Mappings;

		public byte[] Compile(string sourceCode)
		{
			Lexing.AssemblerLexer lexer = new Lexing.AssemblerLexer();

			return Compile(lexer.Lex(sourceCode));
		}

		public byte[] Compile(IEnumerable<Lexing.Instruction> instructions)
		{
			List<Lexing.Instruction> instructionsList = instructions.ToList();

			List<Word> nativeInstructions = new List<Word>();

			IndexInstructions(instructionsList);

			int instructionCounter = 0;
			foreach (var instruction in instructionsList)
			{
				if (!string.IsNullOrWhiteSpace(instruction.Operation))
				{
					//TODO: handle pseudo instructions
					InstructionMapping mapping;

					if (Mappings.TryGetValue(instruction.Operation.Trim(), out mapping))
					{
						Word nativeInstruction = Word.Empty;

						nativeInstruction |= ((Word)mapping.OpCode) << 10;

						switch (mapping.Type)
						{
							case AbaSim.Core.Compiler.Parsing.InstructionType.Register:
							case AbaSim.Core.Compiler.Parsing.InstructionType.VRegister:
								if (instruction.Arguments.Count == 3)
								{
									//rd
									int rd = ParseRawRegister(instruction.Arguments[0]);
									nativeInstruction |= ((((Word)rd) & ((Word)(Bit.S7 + Bit.S8 + Bit.S9))) << 7);

									//rl
									int rl = ParseRawRegister(instruction.Arguments[1]);
									nativeInstruction |= ((((Word)rl) & ((Word)(Bit.S4 + Bit.S5 + Bit.S6))) << 4);

									//rr
									int rr = ParseRawRegister(instruction.Arguments[1]);
									nativeInstruction |= (((Word)rr) & ((Word)(Bit.S1 + Bit.S2 + Bit.S3)));

									if (mapping.Type == Parsing.InstructionType.VRegister)
									{
										nativeInstruction |= ((Word)Bit.S0);
									}
								}
								else
								{
									throw new IllegalArgumentListException(instruction.Operation, instruction.Arguments, mapping.Type);
								}
								break;
							case AbaSim.Core.Compiler.Parsing.InstructionType.Store:
								{
									int constant;
									int rd;
									if (mapping.ConstantRestriction == Parsing.ValueRestriction.Fixed)
									{
										constant = mapping.FixedConstantValue;
										rd = 0;
									}
									else if (instruction.Arguments.Count == 2)
									{
										constant = ParseRawConstant(instruction.Arguments[1], instructionCounter);
										rd = ParseRawRegister(instruction.Arguments[0]);
									}
									else
									{
										throw new IllegalArgumentListException(instruction.Operation, instruction.Arguments, mapping.Type);
									}

									//TODO: bound validation on constant

									nativeInstruction |= (((Word)constant) & ((Word)(Bit.S0 + Bit.S1 + Bit.S2 + Bit.S3 + Bit.S4 + Bit.S5 + Bit.S6)));
									nativeInstruction |= ((((Word)rd) & ((Word)(Bit.S7 + Bit.S8 + Bit.S9))) << 7);

								}
								break;
							case AbaSim.Core.Compiler.Parsing.InstructionType.Immediate:
								{
									int constant;
									int rd;
									int rl;
									if (mapping.ConstantRestriction == Parsing.ValueRestriction.Fixed)
									{
										constant = mapping.FixedConstantValue;
										rd = 0;
										rl = 0;
									}
									else if (instruction.Arguments.Count == 3)
									{
										constant = ParseRawConstant(instruction.Arguments[2], instructionCounter);
										rd = ParseRawRegister(instruction.Arguments[0]);
										rl = ParseRawRegister(instruction.Arguments[1]);
									}
									else
									{
										throw new IllegalArgumentListException(instruction.Operation, instruction.Arguments, mapping.Type);
									}

									Word constantMask = ((Word)(Bit.S0 + Bit.S1 + Bit.S2 + Bit.S3));

									//bounds validation
									int constantMin = (mapping.ConstantRestriction == Parsing.ValueRestriction.Unsigned ? 0 : -1 * constantMask / 2);
									int constantMax = (mapping.ConstantRestriction == Parsing.ValueRestriction.Unsigned ? constantMask : constantMask / 2);
									if (constant < constantMin || constant > constantMax)
									{
										throw new ValueOutOfBoundsException(constant, constantMin, constantMax);
									}

									nativeInstruction |= (((Word)constant) & constantMask);
									nativeInstruction |= ((((Word)rd) & ((Word)(Bit.S7 + Bit.S8 + Bit.S9))) << 7);
									nativeInstruction |= ((((Word)rl) & ((Word)(Bit.S4 + Bit.S5 + Bit.S6))) << 4);
								}
								break;
							case AbaSim.Core.Compiler.Parsing.InstructionType.Jump:
								if (instruction.Arguments.Count == 1)
								{
									int constant = ParseRawConstant(instruction.Arguments[0], instructionCounter);
									nativeInstruction |= (((Word)constant) & ((Word)(Bit.S0 + Bit.S1 + Bit.S2 + Bit.S3 + Bit.S4 + Bit.S5 + Bit.S6 + Bit.S7 + Bit.S8 + Bit.S9)));
									//TODO: bound validation on constant
								}
								else
								{
									throw new IllegalArgumentListException(instruction.Operation, instruction.Arguments, mapping.Type);
								}
								break;
							default:
								//should not happen
								throw new CompilerException("Unknown Instruction Type");
						}

						nativeInstructions.Add(nativeInstruction);
					}
					else
					{
						//unmapped instruction
						throw new UnmappedOperationException(instruction.Operation);
					}

					instructionCounter++;
				}
			}

			byte[] binary = new byte[nativeInstructions.Count * (Word.Size / 8)];
			for (int i = 0; i < nativeInstructions.Count; i++)
			{
				nativeInstructions[i].RawValue.CopyTo(binary, i * (Word.Size / 8));
			}
			return binary;
		}

		public virtual void LoadMappings()
		{
			Mappings = new Dictionary<string, InstructionMapping>();
			foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
			{
				foreach (var mappingAttribute in type.GetCustomAttributes<Parsing.AssemblyCodeAttribute>())
				{
					if (mappingAttribute.Dialect == null || mappingAttribute.Dialect == Dialect)
					{
						Mappings.Add(mappingAttribute.FriendlyName, new InstructionMapping()
						{
							Type = mappingAttribute.Type,
							OpCode = mappingAttribute.OpCode,
							Dialect = mappingAttribute.Dialect,
							ConstantRestriction = mappingAttribute.ConstantRestriction,
							FixedConstantValue = mappingAttribute.FixedConstantValue
						});
					}
				}
			}
		}

		protected virtual int ParseRawRegister(string rawRegister)
		{
			rawRegister = rawRegister.Trim();
			if (string.IsNullOrWhiteSpace(rawRegister))
			{
				throw new IllegalRegisterReferenceException(rawRegister);
			}
			if (rawRegister.StartsWith("$"))
			{
				rawRegister = rawRegister.Substring(1);
			}
			int registerIndex;
			if (int.TryParse(rawRegister, out registerIndex))
			{
				if (registerIndex > 7 || registerIndex < 0)
				{
					throw new IllegalRegisterReferenceException(rawRegister);
				}
				return registerIndex;
			}
			else
			{
				throw new IllegalRegisterReferenceException(rawRegister);
			}
		}

		protected virtual int ParseRawConstant(string rawConstant, int instructionCounter)
		{
			rawConstant = rawConstant.Trim();
			int constant;
			if (!int.TryParse(rawConstant, out constant))
			{
				int targetInstruction;
				if (Labels.TryGetValue(rawConstant, out targetInstruction))
				{
					constant = targetInstruction - instructionCounter;
				}
				else
				{
					throw new IllegalLabelReferenceException(rawConstant);
				}
			}
			return constant;
		}

		private void IndexInstructions(List<Lexing.Instruction> instructions)
		{
			int instructionCounter = 0;
			Labels.Clear();

			foreach (var instruction in instructions)
			{
				if (!string.IsNullOrWhiteSpace(instruction.Label))
				{
					Labels.Add(instruction.Label.Trim(), instructionCounter);
				}
				if (!string.IsNullOrWhiteSpace(instruction.Operation))
				{
					instructionCounter++;
				}
			}
		}

		protected struct InstructionMapping
		{
			public Parsing.InstructionType Type { get; set; }

			public byte OpCode { get; set; }

			public string Dialect { get; set; }

			public Parsing.ValueRestriction ConstantRestriction { get; set; }

			public byte FixedConstantValue { get; set; }
		}
	}
}
