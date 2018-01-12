using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler
{
	public class PseudoInstructionSubstitutor : ICompileStep<IEnumerable<Lexing.Instruction>, IEnumerable<Lexing.Instruction>>
	{
		public PseudoInstructionSubstitutor()
		{
			Substitutions = new List<InstructionSubstitution>();
			//helper
			Substitutions.Add(new InstructionSubstitution("nop", new ReplacementInstruction("j", "1")));
			//stack emulation
			//! Syntax
			// call a method         call {target method name}
			// start a method        {method name}: method
			// return from a method  end {method name}
			//$0 is used as stack-pointer (memory address of current stack-frame start)
			//! Stack-Frame Layout
			// 1Word jump ahead distance (distance between call-site and called method)
			// 7Word register content (excluding $0) in ascending order of register index
			Substitutions.Add(new InstructionSubstitution("stackinit", new ReplacementInstruction("mov", "$0", "0")));
			//TODO: parameters and return values
			Substitutions.Add(new InstructionSubstitution("call",
				//point to next stack frame
				new ReplacementInstruction("addiu", "$0", "$0", "8"),
				//store register content
				new ReplacementInstruction("sti", "$1", "$0", "1"),
				new ReplacementInstruction("sti", "$2", "$0", "2"),
				new ReplacementInstruction("sti", "$3", "$0", "3"),
				new ReplacementInstruction("sti", "$4", "$0", "4"),
				new ReplacementInstruction("sti", "$5", "$0", "5"),
				new ReplacementInstruction("sti", "$6", "$0", "6"),
				new ReplacementInstruction("sti", "$7", "$0", "7"),
				//retrieve jump ahead distance
				new ReplacementInstruction("mov", "$1", 0),
				new ReplacementInstruction("sti", "$1", "$0", "0"),
				new ReplacementInstruction("j", 0),
				//after control is returned
				new ReplacementInstruction("ldi", "$1", "$0", "1"),
				new ReplacementInstruction("ldi", "$2", "$0", "2"),
				new ReplacementInstruction("ldi", "$3", "$0", "3"),
				new ReplacementInstruction("ldi", "$4", "$0", "4"),
				new ReplacementInstruction("ldi", "$5", "$0", "5"),
				new ReplacementInstruction("ldi", "$6", "$0", "6"),
				new ReplacementInstruction("ldi", "$7", "$0", "7"),
				new ReplacementInstruction("subiu", "$0", "$0", "8")
				));
			Substitutions.Add(new InstructionSubstitution("method",
				//reset registers
				new ReplacementInstruction("mov", "$1", "0"),
				new ReplacementInstruction("mov", "$2", "0"),
				new ReplacementInstruction("mov", "$3", "0"),
				new ReplacementInstruction("mov", "$4", "0"),
				new ReplacementInstruction("mov", "$5", "0"),
				new ReplacementInstruction("mov", "$6", "0"),
				new ReplacementInstruction("mov", "$7", "0")
				));
			Substitutions.Add(new InstructionSubstitution("end",
				//jump back to caller
				new ReplacementInstruction("ldi", "$1", "$0", "0"),
				new ReplacementInstruction("muli", "$1", "$1", "-1"),
				new ReplacementInstruction("mov", "$2", 0),
				new ReplacementInstruction("add", "$1", "$1", "$2"),
				new ReplacementInstruction("jmp", "$1")
				));

			//register management
			Substitutions.Add(new InstructionSubstitution("movr", new ReplacementInstruction("addiu", 0, 1, "0")));
			//arithmetic
			Substitutions.Add(new InstructionSubstitution("mod", new ReplacementInstruction("div", 0, 1, 2), new ReplacementInstruction("ovf", 0)));
			Substitutions.Add(new InstructionSubstitution("modu", new ReplacementInstruction("divu", 0, 1, 2), new ReplacementInstruction("ovf", 0)));
			Substitutions.Add(new InstructionSubstitution("modi", new ReplacementInstruction("divi", 0, 1, 2), new ReplacementInstruction("ovf", 0)));
			Substitutions.Add(new InstructionSubstitution("modiu", new ReplacementInstruction("diviu", 0, 1, 2), new ReplacementInstruction("ovf", 0)));
			//comparision
			Substitutions.Add(new InstructionSubstitution("sgt", new ReplacementInstruction("slt", 0, 2, 1)));
			Substitutions.Add(new InstructionSubstitution("sgtu", new ReplacementInstruction("sltu", 0, 2, 1)));
			Substitutions.Add(new InstructionSubstitution("sge", new ReplacementInstruction("sle", 0, 2, 1)));
			Substitutions.Add(new InstructionSubstitution("sgeu", new ReplacementInstruction("sleu", 0, 2, 1)));
			Substitutions.Add(new InstructionSubstitution("vsgt", new ReplacementInstruction("vlt", 0, 2, 1)));
			Substitutions.Add(new InstructionSubstitution("vsgtu", new ReplacementInstruction("vltu", 0, 2, 1)));
			Substitutions.Add(new InstructionSubstitution("vsge", new ReplacementInstruction("vle", 0, 2, 1)));
			Substitutions.Add(new InstructionSubstitution("vsgeu", new ReplacementInstruction("vleu", 0, 2, 1)));
			//branching
			Substitutions.Add(new InstructionSubstitution("blt", new ReplacementInstruction("slt", 0, 1, 2), new ReplacementInstruction("bnz", 0, 3)));
			Substitutions.Add(new InstructionSubstitution("bltu", new ReplacementInstruction("sltu", 0, 1, 2), new ReplacementInstruction("bnz", 0, 3)));
			Substitutions.Add(new InstructionSubstitution("ble", new ReplacementInstruction("sle", 0, 1, 2), new ReplacementInstruction("bnz", 0, 3)));
			Substitutions.Add(new InstructionSubstitution("bleu", new ReplacementInstruction("sleu", 0, 1, 2), new ReplacementInstruction("bnz", 0, 3)));
			Substitutions.Add(new InstructionSubstitution("bgt", new ReplacementInstruction("sgt", 0, 1, 2), new ReplacementInstruction("bnz", 0, 3)));
			Substitutions.Add(new InstructionSubstitution("bgtu", new ReplacementInstruction("sgtu", 0, 1, 2), new ReplacementInstruction("bnz", 0, 3)));
			Substitutions.Add(new InstructionSubstitution("bge", new ReplacementInstruction("sge", 0, 1, 2), new ReplacementInstruction("bnz", 0, 3)));
			Substitutions.Add(new InstructionSubstitution("bgeu", new ReplacementInstruction("sgeu", 0, 1, 2), new ReplacementInstruction("bnz", 0, 3)));
			//easter egg
			Substitutions.Add(new InstructionSubstitution("helloworld",
				new ReplacementInstruction("mov", "$4", "0"),
				new ReplacementInstruction("mov", "$0", "72"),//H
				new ReplacementInstruction("mov", "$1", "101"),//e
				new ReplacementInstruction("mov", "$2", "108"),//l
				new ReplacementInstruction("mov", "$3", "111"),//o
				new ReplacementInstruction("mov", "$5", "87"),//W
				new ReplacementInstruction("mov", "$6", "114"),//r
				new ReplacementInstruction("mov", "$7", "100"),//d
				new ReplacementInstruction("sti", "$0", "$4", "0"),
				new ReplacementInstruction("sti", "$1", "$4", "1"),
				new ReplacementInstruction("sti", "$2", "$4", "2"),
				new ReplacementInstruction("sti", "$2", "$4", "3"),
				new ReplacementInstruction("sti", "$3", "$4", "4"),
				new ReplacementInstruction("sti", "$5", "$4", "6"),
				new ReplacementInstruction("sti", "$3", "$4", "7"),
				new ReplacementInstruction("sti", "$6", "$4", "8"),
				new ReplacementInstruction("sti", "$2", "$4", "9"),
				new ReplacementInstruction("sti", "$7", "$4", "10"),

				new ReplacementInstruction("mov", "$0", "32"),//Space
				new ReplacementInstruction("sti", "$0", "$4", "5")
				));

			//Substitutions.Add(new InstructionSubstitution("", new ReplacementInstruction("")));
		}

		private List<InstructionSubstitution> Substitutions { get; set; }

		public IEnumerable<Lexing.Instruction> Compile(IEnumerable<Lexing.Instruction> input, CompileLog log)
		{
			foreach (var instruction in input)
			{
				InstructionSubstitution substitution = Substitutions.FirstOrDefault(s => instruction.Operation == s.PseudoOperation);
				if (substitution != null)
				{
					var label = instruction.Label;
					foreach (var replacementInstruction in substitution.Replacement)
					{
						List<string> arguments = new List<string>();
						foreach (var parameterSource in replacementInstruction.ParameterSources)
						{
							arguments.Add(parameterSource.Resolve(instruction, log));
						}
						yield return new Lexing.Instruction()
						{
							Operation = replacementInstruction.Operation,
							Arguments = arguments,
							Comment = "replaced pseudo instruction " + instruction.Operation,
							SourceLine = instruction.SourceLine,
							Label = label
						};
						//avoid setting the label on the next instruction as well
						label = null;
					}
				}
				else
				{
					yield return instruction;
				}
			}
		}

		private class InstructionSubstitution
		{
			public InstructionSubstitution(string pseudoOperation, params ReplacementInstruction[] replacement)
			{
				PseudoOperation = pseudoOperation;
				Replacement = replacement;
			}

			public string PseudoOperation { get; private set; }
			public IEnumerable<ReplacementInstruction> Replacement { get; private set; }
		}

		private class ReplacementInstruction
		{
			public ReplacementInstruction(string operation, params ParameterSource[] parameterSources)
			{
				Operation = operation;
				ParameterSources = parameterSources;
			}

			public string Operation { get; private set; }
			public IEnumerable<ParameterSource> ParameterSources { get; private set; }
		}

		private class ParameterSource
		{
			public ParameterSource(int index)
			{
				Value = null;
				Index = index;
			}
			public ParameterSource(string value)
			{
				Value = value;
				Index = -1;
			}

			public int Index;
			public string Value;

			public string Resolve(Lexing.Instruction original, CompileLog log)
			{
				if (Value == null)
				{
					if (Index >= original.Arguments.Count)
					{
						log.CriticalError(original.SourceLine.ToString(), string.Format("Pseudo Instruction {0} is missing arguments.", original.Operation), string.Format("Operation {0} expects an argument at index {1}, however only {2} are provided.", original.Operation, Index, original.Arguments.Count));
						return string.Empty;
					}
					return original.Arguments[Index];
				}
				else
				{
					return Value;
				}
			}

			public static implicit operator ParameterSource(string source)
			{
				return new ParameterSource(source);
			}

			public static implicit operator ParameterSource(int source)
			{
				return new ParameterSource(source);
			}
		}
	}
}
