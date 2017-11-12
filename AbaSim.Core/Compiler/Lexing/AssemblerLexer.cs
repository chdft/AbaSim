using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler.Lexing
{
	public class AssemblerLexer
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
				int offset = 0;
				int boffset = 0;
				Stage stage = Stage.Label;
				Instruction i = new Instruction();
				List<string> args = new List<string>();
				i.Arguments = args;
				int commentFirstSymbolSeenOffset = -2;
				while (offset < line.Length)
				{
					bool isWhiteSpace = WhiteSpace.Contains(line[offset]);
					if (isWhiteSpace && stage <= Stage.Label)
					{
						//ignore leading space
						boffset++;
					}
					else if (line[offset] == ':' && stage <= Stage.Label)
					{
						if (offset == 0)
						{
							throw new InvalidSymbolException(line[offset].ToString(), lineCounter, offset, "label name");
						}
						i.Label = line.Substring(0, offset - 1);
						boffset = offset + 1;
						stage++;
					}
					else if (isWhiteSpace && stage <= Stage.OperationPending)
					{
						//ignore leading space
						boffset++;
					}
					else if (!isWhiteSpace && stage == Stage.OperationPending)
					{
						stage = Stage.OperationRunning;
					}
					else if (isWhiteSpace && stage == Stage.OperationRunning)
					{
						i.Operation = line.Substring(boffset, offset - 1);
						if (i.Operation == string.Empty)
						{
							throw new InvalidSymbolException(line[offset].ToString(), lineCounter, offset, "operation");
						}
						boffset = offset + 1;
						stage++;
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
					else if ((line[offset] == ',' || isWhiteSpace) && stage == Stage.ArgumentsRunning)
					{
						if (boffset < offset)
						{
							//argument completed
							args.Add(line.Substring(boffset, offset - 1));
							boffset = offset + 1;
						}
						else
						{
							//more than one space
							boffset++;
						}
					}
					else if (line[offset] == '/' && stage == Stage.ArgumentsRunning)
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
					offset++;
				}
				if (stage == Stage.CommentRunning)
				{
					i.Comment = line.Substring(boffset);
				}
				i.Index = lineCounter;
				yield return i;
				lineCounter++;
			}
		}

		private enum Stage
		{
			Label = 0,
			OperationPending = 1,
			OperationRunning = 2,
			ArgumentsPending = 3,
			ArgumentsRunning = 4,
			CommentPending = 5,
			CommentRunning = 6
		}
	}
}
