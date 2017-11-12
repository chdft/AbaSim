# AbaSim

AbaSim is a library that virtualizes Abacus instructionset CPUs.
Note that this library does *not* emulate the actual circuit of an abacus CPU, but implements all operations using equivalent operations provided by the host platform.

# Contained Magic

- String Magic (see AssemblerLexer)
- Bit Magic (see instruction decoding)
- structure overlay Magic (see Word)
- Reflection Magic (see AssemblerCompiler)
- Threading/TPL Magic (see Host)