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

			Core.Compiler.AssemblerCompiler compiler = new Core.Compiler.AssemblerCompiler();
			compiler.LoadMappings();
			byte[] binary = compiler.Compile(sourceCode);

			if (destinationFile != null)
			{
				System.IO.File.WriteAllBytes(destinationFile, binary);
				Console.WriteLine("Done.");
			}
			else
			{
				Console.OpenStandardOutput().Write(binary, 0, binary.Length);
			}
		}
	}
}
