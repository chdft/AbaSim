using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbaSim.Core.Compiler
{
	public abstract class CompilePipeline
	{
		public static ICompilePipelineBuilder<TStepOutput, TStepInput> Start<TStepInput, TStepOutput>(ICompileStep<TStepInput, TStepOutput> step)
		{
			return new FirstCompileStepWrapper<TStepInput, TStepOutput>(step);
		}

		public interface ICompilePipelineBuilder<TOutput, TInitialInput>
		{
			ICompilePipelineBuilder<TNextOutput, TInitialInput> Continue<TNextOutput>(ICompileStep<TOutput, TNextOutput> step);

			ICompilePipelineBuilder<TNextOutput, TInitialInput> Convert<TNextOutput>(Func<TOutput, TNextOutput> converter);

			ICompilePipelineBuilder<TOutput, TInitialInput> Inspect(Action<TOutput, CompileLog> inspector);

			CompilePipeline<TInitialInput, TOutput> Complete();
		}

		internal interface ICompileStepWrapper<TOutput, TInitialInput>
		{
			TOutput GetCompilerResult(TInitialInput initialInput, CompileLog log, bool continueOnCriticalError);
		}

		private class ConversionCompileStep<TInput, TOutput> : ICompileStep<TInput, TOutput>
		{
			public ConversionCompileStep(Func<TInput, TOutput> converter)
			{
				Converter = converter;
			}

			private readonly Func<TInput, TOutput> Converter;

			public TOutput Compile(TInput input, CompileLog log)
			{
				return Converter(input);
			}
		}

		private class InspectionCompileStep<TInput> : ICompileStep<TInput, TInput>
		{
			public InspectionCompileStep(Action<TInput, CompileLog> inspector)
			{
				Inspector = inspector;
			}

			private readonly Action<TInput, CompileLog> Inspector;

			public TInput Compile(TInput input, CompileLog log)
			{
				Inspector(input, log);
				return input;
			}
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

			public TOutput GetCompilerResult(TInitialInput initialInput, CompileLog log, bool continueOnCriticalError)
			{
				TInput previousOutput = Previous.GetCompilerResult(initialInput, log, continueOnCriticalError);

				if (log.ErrorOccured && !continueOnCriticalError)
				{
					//skip this step, because a critical error occurred in a previous step
					return default(TOutput);
				}

				try
				{
					return Step.Compile(previousOutput, log);
				}
				catch (CompilerException e)
				{
					log.Error(string.Empty, e.GetType().ToString(), e.Message);
					return default(TOutput);
				}
			}

			public ICompilePipelineBuilder<TNextOutput, TInitialInput> Continue<TNextOutput>(ICompileStep<TOutput, TNextOutput> step)
			{
				return new IntermediateCompileStepWrapper<TOutput, TNextOutput, TInitialInput>(this, step);
			}

			public ICompilePipelineBuilder<TNextOutput, TInitialInput> Convert<TNextOutput>(Func<TOutput, TNextOutput> converter)
			{
				return new IntermediateCompileStepWrapper<TOutput, TNextOutput, TInitialInput>(this, new ConversionCompileStep<TOutput, TNextOutput>(converter));
			}

			public ICompilePipelineBuilder<TOutput, TInitialInput> Inspect(Action<TOutput, CompileLog> inspector)
			{
				return new IntermediateCompileStepWrapper<TOutput, TOutput, TInitialInput>(this, new InspectionCompileStep<TOutput>(inspector));
			}

			public CompilePipeline<TInitialInput, TOutput> Complete()
			{
				return new CompilePipeline<TInitialInput, TOutput>(this);
			}
		}

		private class FirstCompileStepWrapper<TInput, TOutput> : ICompileStepWrapper<TOutput, TInput>, ICompilePipelineBuilder<TOutput, TInput>
		{
			public FirstCompileStepWrapper(ICompileStep<TInput, TOutput> step)
			{
				Step = step;
			}

			ICompileStep<TInput, TOutput> Step;

			public TOutput GetCompilerResult(TInput initialInput, CompileLog log, bool continueOnCriticalError)
			{
				try
				{
					return Step.Compile(initialInput, log);
				}
				catch (CompilerException e)
				{
					log.Error(string.Empty, e.GetType().ToString(), e.Message);
					return default(TOutput);
				}

			}

			public ICompilePipelineBuilder<TNextOutput, TInput> Continue<TNextOutput>(ICompileStep<TOutput, TNextOutput> step)
			{
				return new IntermediateCompileStepWrapper<TOutput, TNextOutput, TInput>(this, step);
			}

			public ICompilePipelineBuilder<TNextOutput, TInput> Convert<TNextOutput>(Func<TOutput, TNextOutput> converter)
			{
				return new IntermediateCompileStepWrapper<TOutput, TNextOutput, TInput>(this, new ConversionCompileStep<TOutput, TNextOutput>(converter));
			}

			public ICompilePipelineBuilder<TOutput, TInput> Inspect(Action<TOutput, CompileLog> inspector)
			{
				return new IntermediateCompileStepWrapper<TOutput, TOutput, TInput>(this, new InspectionCompileStep<TOutput>(inspector));
			}

			public CompilePipeline<TInput, TOutput> Complete()
			{
				return new CompilePipeline<TInput, TOutput>(this);
			}
		}

	}
	public class CompilePipeline<TInput, TOutput> : CompilePipeline
	{
		internal CompilePipeline(ICompileStepWrapper<TOutput, TInput> lastStep)
		{
			LastStep = lastStep;
		}

		public bool ContinueOnCritcalError { get; set; }

		private readonly ICompileStepWrapper<TOutput, TInput> LastStep;

		public CompileResult<TOutput> Compile(TInput input)
		{
			CompileLog log = new CompileLog();

			var output = LastStep.GetCompilerResult(input, log, ContinueOnCritcalError);

			return new CompileResult<TOutput>(output, log);
		}
	}
}
