# AbaSim

AbaSim is a library that virtualizes Abacus instructionset CPUs.
Note that this library does *not* emulate the actual circuit of an abacus CPU, but implements all operations using equivalent operations provided by the host platform.

## Features

- all instructions (excluding vector instructions)
	- basic instructions
	- pseudo instructions
	- memory synchronization
- Memory Caching (single- and multi-layer, shared and separate)
- Abacus assembler compiler (API and command line utility)
	- extendable compile pipeline (enables injection of pre-/post-processing for each compile step - helpful for static analysis)
	- support for constant based multiplexing (i.e. sync/overflw/...)
- Abacus binary runner (API and command line utility)
	- Debugger API
	- basic endless loop detection
- Extendibility (many event-based hooks and sensible class hierarchy)

Additional instructions (like vector instructions) are architecturally possible and could be added, but currently not implemented.

## Usage

### Compiler

`AbaSim.ConsoleCompiler [sourceFile [destinationFile]]`

Where sourceFile is a textfile containing abacus assembler. The output will be written to destinationFile.
If sourceFile is missing, the application reads from stdin. If destinationFile the application writes to stdout. On compilation erros, the exit code is unequal to 0.

### Runner

`AbaSim.ConsoleRunner programMemoryDump dataMemoryDump [Flags]`

Where programMemoryDump is a binary file containing the initial program memory and dataMemoryDump is a binary file containing the initial data memory. Specify flag `C` for control flow monitoring - this may impact execution speed. Specify `F` to enable program flow analysis (requires `C` flag) - this may impact execution speed, but enables endless loop detection. Specify flag `P` to start with paused execution.

You can press ESC at any time to cancel the program execution. Press H for help.

## Contained Magic

- String Magic (see AssemblerLexer)
- Bit Magic (see instruction decoding)
- structure overlay Magic (see Word)
- Reflection Magic (see AssemblerCompiler)
- Threading/TPL Magic (see Host)