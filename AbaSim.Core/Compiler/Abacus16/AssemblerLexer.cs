using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler.Abacus16
{
	public class AssemblerLexer : ICompileStep<string, IEnumerable<AssemblerInstruction>>
	{
		public AssemblerLexer()
		{
			LineSperator = "\r\n";
			WhiteSpace = new char[] { ' ', '\t' };
			LabelTerminator = ':';
			ArgumentSeparator = ',';
			CommentSeparator = '/';
		}

		public string LineSperator { get; set; }

		public char[] WhiteSpace { get; set; }

		public char LabelTerminator { get; set; }

		public char ArgumentSeparator { get; set; }

		public char CommentSeparator { get; set; }

		public bool AllowWhitespaceArgumentSeparation { get; set; }

		private static readonly Regex InstructionExpression = new Regex(@"^\s*(([^:]+):)?\s*([^\s]+)(\s+([^\s,]+)(\s*,\s*([^\s,]))*)?\s*(\/\/(.*))?$", RegexOptions.CultureInvariant);

		public IEnumerable<AssemblerInstruction> Lex(string sourceCode, CompileLog log)
		{
			string[] lines = sourceCode.Split(new string[] { LineSperator }, StringSplitOptions.None);
			int lineCounter = 0;
			foreach (var line in lines)
			{
				var codeLine = line.TrimStart(WhiteSpace);
				if (codeLine.StartsWith("#"))
				{
					log.Warning(lineCounter.ToString(),
						"Ignoring Compiler Directive",
						"AbaSim.Compiler does not support compiler directives. You have to configure the runtime separately to change runtime settings.");
					continue;
				}
				AssemblerInstruction i = new AssemblerInstruction();
				List<string> args = new List<string>();
				i.Arguments = args;
				if (codeLine.StartsWith(CommentSeparator.ToString() + CommentSeparator.ToString()))
				{
					i.Comment = codeLine.Substring(2);
					yield return i;
					continue;
				}
				int offset = 0;
				int boffset = 0;
				Stage stage = Stage.LabelRunning;
				int commentFirstSymbolSeenOffset = -2;
				bool seenArgumentSeparatorSymbol = true; //set initially to true, as first argument does not need to be comma separated

				string location = null;
				while (offset < codeLine.Length)
				{
					location = lineCounter.ToString() + ":" + offset.ToString();
					bool isWhiteSpace = WhiteSpace.Contains(codeLine[offset]);
					switch (stage)
					{
						case Stage.LabelRunning:
							if (codeLine[offset] == LabelTerminator)
							{
								i.Label = codeLine.Substring(boffset, offset - boffset);
								stage = Stage.OperationPending;
							}
							else if (isWhiteSpace)
							{
								i.Operation = codeLine.Substring(boffset, offset - boffset);
								stage = Stage.ArgumentsPending;
							}
							break;
						case Stage.OperationPending:
							if (!isWhiteSpace)
							{
								boffset = offset;
								stage = Stage.OperationRunning;
							}
							break;
						case Stage.OperationRunning:
							if (isWhiteSpace)
							{
								i.Operation = codeLine.Substring(boffset, offset - boffset);
								stage = Stage.ArgumentsPending;
							}
							break;
						case Stage.ArgumentsPending:
							if (!isWhiteSpace)
							{
								if (codeLine[offset] == ArgumentSeparator)
								{
									if (seenArgumentSeparatorSymbol)
									{
										args.Add(string.Empty);
										log.Warning(location, "Empty item in argument list.", null);
									}
								}
								else if (codeLine[offset] == CommentSeparator)
								{
									if (commentFirstSymbolSeenOffset == -2)
									{
										commentFirstSymbolSeenOffset = offset;
									}
									else if (commentFirstSymbolSeenOffset == offset - 1)
									{
										stage = Stage.CommentRunning;
										boffset = offset+1;
									}
									else
									{
										log.Error(location, "Unexpected comment separator.", "Comments must be started by 2 comment separator characters (\"" + CommentSeparator.ToString() + "\").");
									}
								}
								else
								{
									boffset = offset;
									stage = Stage.ArgumentsRunning;
									if (!seenArgumentSeparatorSymbol && !AllowWhitespaceArgumentSeparation)
									{
										log.Warning(location, "Arguments are separated by whitespace instead of \"" + ArgumentSeparator.ToString() + "\"", "According to the language standard, arguments must be separated by an argument separator character and optionally additional whitespace.");
									}
									//CHECK: should we reset seenArgumentSeparatorSymbol to false?
								}
							}
							break;
						case Stage.ArgumentsRunning:
							if (isWhiteSpace)
							{
								stage = Stage.ArgumentsPending;
								args.Add(codeLine.Substring(boffset, offset - boffset));
							}
							break;
					}
					offset++;
				}
				if (stage == Stage.OperationRunning || stage == Stage.LabelRunning)
				{
					i.Operation = codeLine.Substring(boffset);
				}
				else if (stage == Stage.CommentRunning)
				{
					i.Comment = codeLine.Substring(boffset);
				}
				else if (stage == Stage.ArgumentsRunning)
				{
					args.Add(codeLine.Substring(boffset, offset - boffset));
				}
				i.SourceLine = lineCounter;
				yield return i;
				lineCounter++;




				//var codeLine = line.TrimStart(WhiteSpace);
				//if (codeLine.StartsWith("//") || codeLine.StartsWith("#"))
				//{
				//	continue;
				//}
				//int offset = 0;
				//int boffset = 0;
				//Stage stage = Stage.LabelPending;
				//Instruction i = new Instruction();
				//List<string> args = new List<string>();
				//i.Arguments = args;
				//int commentFirstSymbolSeenOffset = -2;
				//while (offset < codeLine.Length)
				//{
				//	bool isWhiteSpace = WhiteSpace.Contains(codeLine[offset]);
				//	if (isWhiteSpace && stage <= Stage.LabelPending)
				//	{
				//		//ignore leading space
				//		boffset++;
				//	}
				//	else if (!isWhiteSpace && stage == Stage.LabelPending)
				//	{
				//		stage = Stage.LabelRunning;
				//	}
				//	else if (codeLine[offset] == ':' && stage <= Stage.LabelRunning)
				//	{
				//		if (offset == 0)
				//		{
				//			throw new InvalidSymbolException(codeLine[offset].ToString(), lineCounter, offset, "label name");
				//		}
				//		i.Label = codeLine.Substring(boffset, offset - boffset);
				//		boffset = offset + 1;
				//		stage = Stage.OperationPending;
				//	}
				//	else if (isWhiteSpace && stage == Stage.OperationPending)
				//	{
				//		//ignore leading space
				//		boffset++;
				//	}
				//	else if (!isWhiteSpace && stage == Stage.OperationPending)
				//	{
				//		stage = Stage.OperationRunning;
				//	}
				//	else if (isWhiteSpace && stage <= Stage.OperationRunning)
				//	{
				//		i.Operation = codeLine.Substring(boffset, offset - boffset);
				//		if (i.Operation == string.Empty)
				//		{
				//			throw new InvalidSymbolException(codeLine[offset].ToString(), lineCounter, offset, "operation");
				//		}
				//		boffset = offset + 1;
				//		stage = Stage.ArgumentsPending;
				//	}
				//	else if (isWhiteSpace && stage <= Stage.ArgumentsPending)
				//	{
				//		//ignore leading space
				//		boffset++;
				//	}
				//	else if (!isWhiteSpace && stage == Stage.ArgumentsPending)
				//	{
				//		stage = Stage.ArgumentsRunning;
				//	}
				//	else if ((codeLine[offset] == ',' || isWhiteSpace || codeLine[offset] == '/') && stage == Stage.ArgumentsRunning)
				//	{
				//		if (boffset < offset)
				//		{
				//			//argument completed
				//			args.Add(codeLine.Substring(boffset, offset - boffset));
				//			boffset = offset + 1;
				//		}
				//		else
				//		{
				//			//more than one space
				//			boffset++;
				//		}
				//		if (codeLine[offset] == '/')
				//		{
				//			if (commentFirstSymbolSeenOffset == offset - 1)
				//			{
				//				stage = Stage.CommentRunning;
				//				boffset = offset + 1;
				//			}
				//			else if (commentFirstSymbolSeenOffset == -2)
				//			{
				//				commentFirstSymbolSeenOffset = offset;
				//			}
				//			else
				//			{
				//				throw new InvalidSymbolException("/", lineCounter, offset, "comment start token (\"\")");
				//			}
				//		}
				//	}
				//	offset++;
				//}
				//if (stage == Stage.CommentRunning)
				//{
				//	i.Comment = codeLine.Substring(boffset);
				//}
				//else if (stage == Stage.ArgumentsRunning)
				//{
				//	args.Add(codeLine.Substring(boffset, offset - boffset));
				//}
				//i.SourceLine = lineCounter;
				//yield return i;
				//lineCounter++;
			}
		}

		IEnumerable<AssemblerInstruction> ICompileStep<string, IEnumerable<AssemblerInstruction>>.Compile(string input, CompileLog log)
		{
			return Lex(input, log);
		}

		private enum Stage
		{
			//LabelPending = 0,
			LabelRunning = 1,
			OperationPending = 2,
			OperationRunning = 3,
			ArgumentsPending = 4,
			ArgumentsRunning = 5,
			//CommentPending = 6,
			CommentRunning = 7
		}
	}
}
