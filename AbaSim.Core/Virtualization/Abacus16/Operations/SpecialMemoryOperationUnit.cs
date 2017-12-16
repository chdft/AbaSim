using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("sync", OpCode, Compiler.Parsing.InstructionType.Store, ConstantRestriction = AbaSim.Core.Compiler.Parsing.ValueRestriction.Fixed, FixedConstantValue = SyncConstant)]
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("ovf", OpCode, Compiler.Parsing.InstructionType.Store, ConstantRestriction = AbaSim.Core.Compiler.Parsing.ValueRestriction.Fixed, FixedConstantValue = OverflowConstant)]
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("mvtm", OpCode, Compiler.Parsing.InstructionType.Store, ConstantRestriction = AbaSim.Core.Compiler.Parsing.ValueRestriction.Fixed, FixedConstantValue = MoveToMaskRegisterConstant)]
	[AbaSim.Core.Compiler.Parsing.AssemblyCode("mvtl", OpCode, Compiler.Parsing.InstructionType.Store, ConstantRestriction = AbaSim.Core.Compiler.Parsing.ValueRestriction.Fixed, FixedConstantValue = MoveToVectorLengthRegisterConstant)]
	class SpecialMemoryOperationUnit : StoreOperationUnit
	{
		public const byte OpCode = Bit.B5 + Bit.B4 + Bit.B3 + Bit.B2 + Bit.B1 + Bit.B0;
		private const byte SyncConstant = 0;
		private const byte OverflowConstant = Bit.B0;
		private const byte MoveToMaskRegisterConstant = Bit.B1;
		private const byte MoveToVectorLengthRegisterConstant = Bit.B1 + Bit.B0;

		public SpecialMemoryOperationUnit(SerialAbacus16Cpu cpu, IRegisterGroup registers)
			: base(registers)
		{
			Cpu = cpu;
		}

		private SerialAbacus16Cpu Cpu;

		protected override void InternalExecute()
		{
			switch (UnsignedConstant)
			{
				//sync
				case SyncConstant:
					Cpu.Synchronize();
					break;
				//ovf
				case OverflowConstant:
					throw new NotImplementedException();
				//mvtm
				case MoveToMaskRegisterConstant:
					throw new NotImplementedException();
				//mvtl
				case MoveToVectorLengthRegisterConstant:
					throw new NotImplementedException();
				default:
					throw new IllegalOperationArgumentException("Invalid c argument for special memory instruction.", Instruction);
			}
		}
	}
}
