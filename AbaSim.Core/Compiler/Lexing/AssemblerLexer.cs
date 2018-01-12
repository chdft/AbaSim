using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler.Lexing
{
	public class AssemblerLexer : ICompileStep<string, IEnumerable<Instruction>>
	{
		public AssemblerLexer()
		{
			LineSperator = "\r\n";
			WhiteSpace = new char[] { ' ', '\t' };
		}

		public string LineSperator { get; set; }

		public char[] WhiteSpace { get; set; }

		public IEnumerable<Instruction> Lex(string sourceCode)
		{
			string[] lines = sourceCode.Split(new string[] { LineSperator }, StringSplitOptions.RemoveEmptyEntries);
			int lineCounter = 0;
			foreach (var line in lines)
			{
				var codeLine = line.TrimStart(WhiteSpace);
				if (codeLine.StartsWith("//") || codeLine.StartsWith("#"))
				{
					continue;
				}
				int offset = 0;
				int boffset = 0;
				Stage stage = Stage.LabelPending;
				Instruction i = new Instruction();
				List<string> args = new List<string>();
				i.Arguments = args;
				int commentFirstSymbolSeenOffset = -2;
				while (offset < codeLine.Length)
				{
					bool isWhiteSpace = WhiteSpace.Contains(codeLine[offset]);
					if (isWhiteSpace && stage <= Stage.LabelPending)
					{
						//ignore leading space
						boffset++;
					}
					else if (!isWhiteSpace && stage == Stage.LabelPending)
					{
						stage = Stage.LabelRunning;
					}
					else if (codeLine[offset] == ':' && stage <= Stage.LabelRunning)
					{
						if (offset == 0)
						{
							throw new InvalidSymbolException(codeLine[offset].ToString(), lineCounter, offset, "label name");
						}
						i.Label = codeLine.Substring(boffset, offset - boffset);
						boffset = offset + 1;
						stage = Stage.OperationPending;
					}
					else if (isWhiteSpace && stage == Stage.OperationPending)
					{
						//ignore leading space
						boffset++;
					}
					else if (!isWhiteSpace && stage == Stage.OperationPending)
					{
						stage = Stage.OperationRunning;
					}
					else if (isWhiteSpace && stage <= Stage.OperationRunning)
					{
						i.Operation = codeLine.Substring(boffset, offset - boffset);
						if (i.Operation == string.Empty)
						{
							throw new InvalidSymbolException(codeLine[offset].ToString(), lineCounter, offset, "operation");
						}
						boffset = offset + 1;
						stage = Stage.ArgumentsPending;
					}
					else if (isWhiteSpace && stage <= Stage.ArgumentsPending)
					{
						//ignore leading space
						boffset++;
					}
					else if (!isWhiteSpace && stage == Stage.ArgumentsPending)
					{
						stage = Stage.ArgumentsRunning;
					}
					else if ((codeLine[offset] == ',' || isWhiteSpace || codeLine[offset] == '/') && stage == Stage.ArgumentsRunning)
					{
						if (boffset < offset)
						{
							//argument completed
							args.Add(codeLine.Substring(boffset, offset - boffset));
							boffset = offset + 1;
						}
						else
						{
							//more than one space
							boffset++;
						}
						if (codeLine[offset] == '/')
						{
							if (commentFirstSymbolSeenOffset == offset - 1)
							{
								stage = Stage.CommentRunning;
								boffset = offset + 1;
							}
							else if (commentFirstSymbolSeenOffset == -2)
							{
								commentFirstSymbolSeenOffset = offset;
							}
							else
							{
								throw new InvalidSymbolException("/", lineCounter, offset, "comment start token (\"\")");
							}
						}
					}
					offset++;
				}
				if (stage == Stage.CommentRunning)
				{
					i.Comment = codeLine.Substring(boffset);
				}
				else if (stage == Stage.ArgumentsRunning)
				{
					args.Add(codeLine.Substring(boffset, offset - boffset));
				}
				i.Index = lineCounter;
				yield return i;
				lineCounter++;
			}
		}

		IEnumerable<Instruction> ICompileStep<string, IEnumerable<Instruction>>.Compile(string input, CompileLog log)
		{
			return Lex(input);
		}

		private enum Stage
		{
			LabelPending = 0,
			LabelRunning = 1,
			OperationPending = 2,
			OperationRunning = 3,
			ArgumentsPending = 4,
			ArgumentsRunning = 5,
			CommentPending = 6,
			CommentRunning = 7
		}
	}
}
