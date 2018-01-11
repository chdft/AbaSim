using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler
{
	public class CompilePipeline
	{
		public static ICompilePipelineBuilder<TStepOutput, TStepInput> Start<TStepInput, TStepOutput>(ICompileStep<TStepInput, TStepOutput> step)
		{
			return new FirstCompileStepWrapper<TStepInput, TStepOutput>(step);
		}

		public interface ICompilePipelineBuilder<TOutput, TInitialInput>
		{
			ICompilePipelineBuilder<TNextOutput, TInitialInput> Append<TNextOutput>(ICompileStep<TOutput, TNextOutput> step);

			CompilePipeline<TInitialInput, TOutput> Complete();
		}

		protected interface ICompileStepWrapper<TOutput, TInitialInput>
		{
			TOutput GetCompilerResult(TInitialInput initialInput);
		}

		private class IntermediateCompileStepWrapper<TInput, TOutput, TInitialInput> : ICompileStepWrapper<TOutput, TInitialInput>, ICompilePipelineBuilder<TOutput, TInitialInput>
		{
			public IntermediateCompileStepWrapper(ICompileStepWrapper<TInput, TInitialInput> previous, ICompileStep<TInput, TOutput> step)
			{
				Previous = previous;
				Step = step;
			}

			ICompileStep<TInput, TOutput> Step;

			ICompileStepWrapper<TInput, TInitialInput> Previous;

			public TOutput GetCompilerResult(TInitialInput initialInput)
			{
				return Step.Compile(Previous.GetCompilerResult(initialInput));
			}

			public ICompilePipelineBuilder<TNextOutput, TInitialInput> Append<TNextOutput>(ICompileStep<TOutput, TNextOutput> step)
			{
				return new IntermediateCompileStepWrapper<TOutput, TNextOutput, TInitialInput>(this, step);
			}
		}

		private class FirstCompileStepWrapper<TInput, TOutput> : ICompileStepWrapper<TOutput, TInput>, ICompilePipelineBuilder<TOutput, TInput>
		{
			public FirstCompileStepWrapper(ICompileStep<TInput, TOutput> step)
			{
				Step = step;
			}

			ICompileStep<TInput, TOutput> Step;

			public TOutput GetCompilerResult(TInput initialInput)
			{
				return Step.Compile(initialInput);
			}

			public ICompilePipelineBuilder<TNextOutput, TInput> Append<TNextOutput>(ICompileStep<TOutput, TNextOutput> step)
			{
				return new IntermediateCompileStepWrapper<TOutput, TNextOutput, TInput>(this, step);
			}

			public CompilePipeline<TInput, TOutput> Complete()
			{
				throw new NotImplementedException();
			}
		}

	}
	public class CompilePipeline<TInput, TOutput>:CompilePipeline
	{
		public CompilePipeline(ICompileStepWrapper<TOutput, TInput> lastStep)
		{
			LastStep = lastStep;
		}

		private readonly ICompileStepWrapper<TOutput, TInput> LastStep;

		public TOutput Compile(TInput input)
		{
			return LastStep.GetCompilerResult(input);
		}
	}
}
