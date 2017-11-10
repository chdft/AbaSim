﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Virtualization.Abacus16.Operations
{
	abstract class ImmediateOperationUnit : IOperationUnit
	{
		protected RegisterIndex DestinationRegister { get; private set; }

		protected RegisterIndex LeftRegister { get; private set; }

		protected RegisterIndex RightRegister { get; private set; }

		public void Decode(Word instruction)
		{
			throw new NotImplementedException();
		}

		public void Execute()
		{
			throw new NotImplementedException();
		}

		public Word?[] UpdatedRegisters
		{
			get { throw new NotImplementedException(); }
		}

		public Vector[] UpdatedVRegisters
		{
			get { throw new NotImplementedException(); }
		}

		public int? UpdateMemoryAddress
		{
			get { throw new NotImplementedException(); }
		}

		public Word UpdateMemoryValue
		{
			get { throw new NotImplementedException(); }
		}
	}
}