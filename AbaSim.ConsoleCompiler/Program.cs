using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.ConsoleCompiler
{
	class Program
	{
		static void Main(string[] args)
		{
			string sourceFile = (args.Length > 0 ? args[0] : null);
			string destinationFile = (args.Length > 1 ? args[1] : null);

			string sourceCode = null;
			if (sourceFile == null)
			{
				string line = null;
				while ((line = Console.ReadLine()) != "")
				{
					sourceCode += line + "\r\n";
				}
			}
			else
			{
				sourceCode = System.IO.File.ReadAllText(sourceFile);
			}


			Core.Compiler.Abacus16.AssemblerCompiler compiler = new Core.Compiler.Abacus16.AssemblerCompiler()
			{
				AllowStandaloneLabels = true
			};
			compiler.LoadMappings();
			var pipeline = Core.Compiler.CompilePipeline
				.Start(new Core.Compiler.Abacus16.AssemblerLexer())
				.Inspect((instructions, log) =>
				{
					if (destinationFile != null)
					{
						int i = 0;
						Console.WriteLine("Code before substitution:");
						Console.WriteLine("instruction index | line | instruction");
						foreach (var instruction in instructions)
						{
							Console.WriteLine("{0,4}|{2,4}| {1}", i, instruction, instruction.SourceLine);
							i++;
						}
					}
				})
				.Continue(new Core.Compiler.Abacus16.PseudoInstructionSubstitutor())
				.Inspect((instructions, log) =>
				{
					if (destinationFile != null)
					{
						int i = 0;
						Console.WriteLine("Code after substitution:");
						Console.WriteLine("instruction index | line | instruction");
						foreach (var instruction in instructions)
						{
							Console.WriteLine("{0,4}|{2,4}| {1}", i, instruction, instruction.SourceLine);
							i++;
						}
					}
				})
				.Continue(compiler)
				.Complete();

			var result = pipeline.Compile(sourceCode);

			if (result.Log.ErrorOccured)
			{
				Console.WriteLine("Compilation failed.");
			}
			else
			{
				Console.WriteLine("Compilation succeeded.");
			}

			//only print the log if the output is not printed
			if (destinationFile != null)
			{
				foreach (var item in result.Log.OrderByDescending(i => i.Severity))
				{
					Console.WriteLine("{0} | {1}: {2}", item.Severity, item.Location, item.Message);
					if (!string.IsNullOrEmpty(item.Description))
					{
						Console.WriteLine(item.Description);
					}
				}
			}

			if (result.Log.ErrorOccured)
			{
				Environment.ExitCode = 2;
				return;
			}

			if (destinationFile != null)
			{
				System.IO.File.WriteAllBytes(destinationFile, result.Output);
				Console.WriteLine("Done.");
			}
			else
			{
				Console.OpenStandardOutput().Write(result.Output, 0, result.Output.Length);
			}
		}
	}
}
