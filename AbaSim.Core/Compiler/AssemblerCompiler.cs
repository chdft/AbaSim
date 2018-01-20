using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AbaSim.Core.Virtualization.Abacus16;

namespace AbaSim.Core.Compiler
{
	public class AssemblerCompiler : ICompileStep<IEnumerable<Lexing.Instruction>, byte[]>
	{
		public AssemblerCompiler() { }

		public string LineSperator { get; set; }

		public string Dialect { get; set; }

		//protected Dictionary<string, int> Labels = new Dictionary<string, int>();

		protected Dictionary<string, InstructionMapping> Mappings;

		[Obsolete("use Lexing.AssemblerLexer.Lex(string) instead; consider piping the output using a CompilerPipeline", true)]
		public byte[] Compile(string sourceCode)
		{
			throw new NotImplementedException();
		}

		public byte[] Compile(IEnumerable<Lexing.Instruction> instructions, CompileLog log)
		{
			List<Lexing.Instruction> instructionsList = instructions.ToList();

			List<Word> nativeInstructions = new List<Word>();

			var labels = IndexInstructions(instructionsList, log);

			int instructionCounter = 0;
			foreach (var instruction in instructionsList)
			{
				if (!string.IsNullOrWhiteSpace(instruction.Operation))
				{
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
									nativeInstruction |= ((((Word)rd) & ((Word)Bit.MaskFirstS(3))) << 7);

									//rl
									int rl = ParseRawRegister(instruction.Arguments[1]);
									nativeInstruction |= ((((Word)rl) & ((Word)Bit.MaskFirstS(3))) << 4);

									//rr
									int rr = ParseRawRegister(instruction.Arguments[1]);
									nativeInstruction |= ((((Word)rr) & ((Word)Bit.MaskFirstS(3))) << 1);

									if (mapping.Type == Parsing.InstructionType.VRegister)
									{
										nativeInstruction |= ((Word)Bit.S0);
									}
								}
								else
								{
									log.Error(instruction.SourceLine.ToString(),
										"Illegal parameter list (wrong parameter count).",
										string.Format("Register instructions without fixed parameters expect exactly 3 parameters (register rd, register rl, register rr), however {0} were provided ({2}).", instruction.Arguments.Count, string.Join(",", instruction.Arguments)));

									//throw new IllegalArgumentListException(instruction.Operation, instruction.Arguments, mapping.Type);
								}
								break;
							case AbaSim.Core.Compiler.Parsing.InstructionType.Store:
								{
									byte constantSize = 7;
									int constant = 0;
									int rd = 0;
									if (mapping.ConstantRestriction == Parsing.ConstantValueRestriction.Fixed && mapping.DestinationRestriction == Parsing.RegisterReferenceRestriction.Fixed)
									{
										constant = mapping.FixedConstantValue;
										rd = mapping.FixedDestinationValue;
										if (instruction.Arguments.Count != 0)
										{
											log.Warning(instruction.SourceLine.ToString(),
												"Ignoring parameters on fixed store instruction.",
												"Fixed store instructions use the constant part of the binary instruction to multiplex between multiple logical instructions. They do not accept any parameters.");
										}
									}
									else if (mapping.ConstantRestriction == Parsing.ConstantValueRestriction.Fixed && mapping.DestinationRestriction != Parsing.RegisterReferenceRestriction.Fixed)
									{
										constant = mapping.FixedConstantValue;
										rd = ParseRawRegister(instruction.Arguments[0]);
										if (instruction.Arguments.Count != 1)
										{
											log.Warning(instruction.SourceLine.ToString(),
												"Ignoring parameters on constant-fixed store instruction.",
												"Constant-fixed store instructions use the constant part of the binary instruction to multiplex between multiple logical instructions. They accept exactly 1 parameter (rd).");
										}
									}
									else if (mapping.ConstantRestriction != Parsing.ConstantValueRestriction.Fixed && mapping.DestinationRestriction == Parsing.RegisterReferenceRestriction.Fixed)
									{
										constant = ParseRawConstant(instruction.Arguments[1], instructionCounter, labels);
										rd = mapping.FixedDestinationValue;
										if (instruction.Arguments.Count != 1)
										{
											log.Warning(instruction.SourceLine.ToString(),
												"Ignoring parameters on destination-fixed store instruction.",
												"Destination-fixed store instructions use the rd (destination register) part of the binary instruction to multiplex between multiple logical instructions. They accept exactly 1 parameter (c).");
										}
									}
									else if (instruction.Arguments.Count == 2)
									{
										constant = ParseRawConstant(instruction.Arguments[1], instructionCounter, labels);
										rd = ParseRawRegister(instruction.Arguments[0]);
									}
									else
									{
										log.Error(instruction.SourceLine.ToString(),
											"Illegal parameter list (wrong parameter count).",
											string.Format("Store instructions without fixed parameters expect exactly 2 parameters (register rd, constant c), however {0} were provided ({2}).", instruction.Arguments.Count, string.Join(",", instruction.Arguments)));
										//throw new IllegalArgumentListException(instruction.Operation, instruction.Arguments, mapping.Type);
									}

									int constantMin = Bit.LowerBound(constantSize, mapping.ConstantRestriction == Parsing.ConstantValueRestriction.Unsigned);
									int constantMax = Bit.UpperBound(constantSize, mapping.ConstantRestriction == Parsing.ConstantValueRestriction.Unsigned);
									if (constant < constantMin || constant > constantMax)
									{
										log.Error(instruction.SourceLine.ToString(),
											string.Format("Constant/Label (c={0}) is out of bounds ([{1};{2}]).", constant, constantMin, constantMax),
											"Consider using a mov statement and adding the remainder in a register using addi/addiu.");
									}

									nativeInstruction |= (((Word)constant) & ((Word)Bit.MaskFirstS(7)));
									nativeInstruction |= ((((Word)rd) & ((Word)Bit.MaskFirstS(3))) << 7);

								}
								break;
							case AbaSim.Core.Compiler.Parsing.InstructionType.Immediate:
								{
									byte constantSize = 4;
									int constant = 0;
									int rd = 0;
									int rl = 0;
									if (mapping.ConstantRestriction == Parsing.ConstantValueRestriction.Fixed && mapping.DestinationRestriction == Parsing.RegisterReferenceRestriction.Fixed)
									{
										constant = mapping.FixedConstantValue;
										rd = mapping.FixedDestinationValue;
										rl = 0;
										if (instruction.Arguments.Count != 0)
										{
											log.Warning(instruction.SourceLine.ToString(),
												"Illegal parameter list (wrong parameter count).",
												"Fixed immediate instructions use the constant part of the binary instruction to multiplex between multiple logical instructions. They do not accept any parameters.");
										}
									}
									else if (mapping.ConstantRestriction == Parsing.ConstantValueRestriction.Fixed && mapping.DestinationRestriction != Parsing.RegisterReferenceRestriction.Fixed)
									{
										if (instruction.Arguments.Count == 1)
										{
											constant = mapping.FixedConstantValue;
											rd = ParseRawRegister(instruction.Arguments[0]);
											rl = 0;
										}
										else
										{
											log.Warning(instruction.SourceLine.ToString(),
												"Illegal parameter list (wrong parameter count).",
												"Constant-fixed immediate instructions use the constant part of the binary instruction to multiplex between multiple logical instructions. They accept exactly 1 parameter (rd).");
										}
									}
									else if (mapping.ConstantRestriction != Parsing.ConstantValueRestriction.Fixed && mapping.DestinationRestriction == Parsing.RegisterReferenceRestriction.Fixed)
									{
										if (instruction.Arguments.Count == 1)
										{
											constant = ParseRawConstant(instruction.Arguments[0], instructionCounter, labels);
											rd = mapping.FixedDestinationValue;
											rl = 0;
										}
										else
										{
											log.Error(instruction.SourceLine.ToString(),
												"Ignoring parameters on destination-fixed immediate instruction.",
												"Destination-fixed immediate instructions use the rd (destination register) part of the binary instruction to multiplex between multiple logical instructions. They accept exactly 1 parameter (c).");

										}
									}
									else
									{
										if (instruction.Arguments.Count == 3)
										{
											constant = ParseRawConstant(instruction.Arguments[2], instructionCounter, labels);
											rd = ParseRawRegister(instruction.Arguments[0]);
											rl = ParseRawRegister(instruction.Arguments[1]);
										}
										else
										{
											log.Error(instruction.SourceLine.ToString(),
												"Illegal parameter list (wrong parameter count).",
												string.Format("Immediate instructions without fixed parameters expect exactly 3 parameters (register rd, register rl, constant c), however {0} were provided ({2}).", instruction.Arguments.Count, string.Join(",", instruction.Arguments)));

											//throw new IllegalArgumentListException(instruction.Operation, instruction.Arguments, mapping.Type);
										}
									}

									//bounds validation
									//int constantMin = (mapping.ConstantRestriction == Parsing.ConstantValueRestriction.Unsigned ? 0 : -1 * (int)constantMask / 2);
									//int constantMax = (mapping.ConstantRestriction == Parsing.ConstantValueRestriction.Unsigned ? (int)constantMask : (int)constantMask / 2);
									int constantMin = Bit.LowerBound(constantSize, mapping.ConstantRestriction == Parsing.ConstantValueRestriction.Unsigned);
									int constantMax = Bit.UpperBound(constantSize, mapping.ConstantRestriction == Parsing.ConstantValueRestriction.Unsigned);
									if (constant < constantMin || constant > constantMax)
									{
										log.Error(instruction.SourceLine.ToString(),
											string.Format("Constant/Label (c={0}) is out of bounds ([{1};{2}]).", constant, constantMin, constantMax),
											"Consider using a mov statement and adding the remainder in a register using addi/addiu.");
									}

									nativeInstruction |= (((Word)constant) & Bit.MaskFirstS(constantSize));
									nativeInstruction |= ((((Word)rd) & ((Word)Bit.MaskFirstS(3))) << 7);
									nativeInstruction |= ((((Word)rl) & ((Word)Bit.MaskFirstS(3))) << 4);
								}
								break;
							case AbaSim.Core.Compiler.Parsing.InstructionType.Jump:
								{
									byte constantSize = 10;
									int constant = 0;
									if (mapping.ConstantRestriction == Parsing.ConstantValueRestriction.Fixed)
									{
										if (instruction.Arguments.Count == 0)
										{
											constant = mapping.FixedConstantValue;
										}
										else
										{
											log.Warning(instruction.SourceLine.ToString(),
												"Ignoring parameters on constant-fixed jump instruction.",
												"Constant-fixed jump instructions use the constant part of the binary instruction to multiplex between multiple logical instructions. They accept exactly 1 parameter (c).");
										}
									}
									else
									{
										if (instruction.Arguments.Count == 1)
										{
											constant = ParseRawConstant(instruction.Arguments[0], instructionCounter, labels);
										}
										else
										{
											log.Error(instruction.SourceLine.ToString(),
												"Illegal parameter list (wrong parameter count).",
												string.Format("Jump instructions without fixed parameters expect exactly 1 parameter (constant c), however {0} were provided ({2}).", instruction.Arguments.Count, string.Join(",", instruction.Arguments)));


											//throw new IllegalArgumentListException(instruction.Operation, instruction.Arguments, mapping.Type);
										}
									}

									nativeInstruction |= (((Word)constant) & Bit.MaskFirstS(constantSize));
									int constantMin = Bit.LowerBound(constantSize, mapping.ConstantRestriction == Parsing.ConstantValueRestriction.Unsigned);
									int constantMax = Bit.UpperBound(constantSize, mapping.ConstantRestriction == Parsing.ConstantValueRestriction.Unsigned);
									if (constant < constantMin || constant > constantMax)
									{
										log.Error(instruction.SourceLine.ToString(),
											string.Format("Constant/Label (c={0}) is out of bounds ([{1};{2}]).", constant, constantMin, constantMax),
											"Consider using jmp with a pre-calculated (i.e. using mov and add) target instead of a constant jump (j) for long jumps.");
									}
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
			foreach (var type in typeof(AssemblerCompiler).Assembly.GetTypes())
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
							FixedConstantValue = mappingAttribute.FixedConstantValue,
							DestinationRestriction = mappingAttribute.DestinationRestriction,
							FixedDestinationValue = mappingAttribute.FixedDestinationValue
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

		protected virtual int ParseRawConstant(string rawConstant, int instructionCounter, Dictionary<string, int> labels)
		{
			rawConstant = rawConstant.Trim();
			int constant;
			if (!int.TryParse(rawConstant, out constant))
			{
				int targetInstruction;
				if (labels.TryGetValue(rawConstant, out targetInstruction))
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

		private Dictionary<string, int> IndexInstructions(List<Lexing.Instruction> instructions, CompileLog log)
		{
			int instructionCounter = -1;
			var labels = new Dictionary<string, int>();

			foreach (var instruction in instructions)
			{
				if (!string.IsNullOrWhiteSpace(instruction.Label))
				{
					if (labels.ContainsKey(instruction.Label))
					{
						log.Error(instruction.SourceLine.ToString(), 
							"Ambiguous Label", 
							string.Format("Labels must be unique, however the label {0} was declared in line {1} and {2}.", instruction.Label, instruction.SourceLine, labels[instruction.Label]));
					}
					labels.Add(instruction.Label.Trim(), instructionCounter + 1);
					if (string.IsNullOrWhiteSpace(instruction.Operation))
					{
						if (Dialect != Parsing.Dialects.ChDFT)
						{
							log.Error(instruction.SourceLine.ToString(),
								"Labels may only decorate Operations",
								"Labels may only be used on lines where an operation is declared. Consider using j 1 or nop if you need to declare a label separately.");
						}
						else
						{
							log.Information(instruction.SourceLine.ToString(),
								"Label without Operation",
								"A label was declared on a line without an operation.");
						}
					}
				}
				if (!string.IsNullOrWhiteSpace(instruction.Operation))
				{
					instructionCounter++;
				}
			}
			return labels;
		}

		protected struct InstructionMapping
		{
			public Parsing.InstructionType Type { get; set; }

			public byte OpCode { get; set; }

			public string Dialect { get; set; }

			public Parsing.ConstantValueRestriction ConstantRestriction { get; set; }

			public byte FixedConstantValue { get; set; }

			public Parsing.RegisterReferenceRestriction DestinationRestriction { get; set; }

			public byte FixedDestinationValue { get; set; }
		}
	}
}
