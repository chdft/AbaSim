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
			Substitutions.Add(new InstructionSubstitution("nop", new ReplacementInstruction("j", new ParameterSource("1"))));
		}

		private List<InstructionSubstitution> Substitutions = new List<InstructionSubstitution>();

		public IEnumerable<Lexing.Instruction> Compile(IEnumerable<Lexing.Instruction> input, CompileLog log)
		{
			foreach (var instruction in input)
			{
				InstructionSubstitution substitution = Substitutions.FirstOrDefault(s => instruction.Operation == s.PseudoOperation);
				if (substitution != null)
				{
					foreach (var replacementInstruction in substitution.Replacement)
					{
						List<string> arguments = new List<string>();
						foreach (var parameterSource in replacementInstruction.ParameterSources)
						{
							//TODO: log out of bounds access
							arguments.Add(parameterSource.Resolve(instruction));
						}
						yield return new Lexing.Instruction()
						{
							Operation = replacementInstruction.Operation,
							Arguments = arguments,
							Comment = "replaced pseudo instruction " + instruction.Operation
						};
					}
				}
				else
				{
					yield return instruction;
				}
			}
			throw new NotImplementedException();
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

			public string Resolve(Lexing.Instruction original)
			{
				if (Value == null)
				{
					return original.Arguments[Index];
				}
				else
				{
					return Value;
				}
			}
		}
	}
}
