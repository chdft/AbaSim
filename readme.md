# AbaSim

AbaSim is a library that virtualizes Abacus instructionset CPUs.
Note that this library does *not* emulate the actual circuit of an abacus CPU, but implements all operations using equivalent operations provided by the host platform.

## Usage

### Compiler

`AbaSim.ConsoleCompiler [sourceFile [destinationFile]]`

Where sourceFile is a textfile containing abacus assembler. The output will be written to destinationFile.
If sourceFile is missing, the application reads from stdin. If destinationFile the application writes to stdout. On compilation erros, the exit code is unequal to 0.

### Runner

`AbaSim.ConsoleRunner programMemoryDump dataMemoryDump [Flags]`

Where programMemoryDump is a binary file containing the initial program memory and dataMemoryDump is a binary file containing the initial data memory. Specify flag `C` for control flow monitoring - this may impact execution speed. Specify flag `P` to start with paused execution.

You can press ESC at any time to cancel the program execution. Press H for help.

## Contained Magic

- String Magic (see AssemblerLexer)
- Bit Magic (see instruction decoding)
- structure overlay Magic (see Word)
- Reflection Magic (see AssemblerCompiler)
- Threading/TPL Magic (see Host)