using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Cloo;

namespace GeneticAlgo
{
    public enum OpenCLKernel
    {
        CalcFFFullAndCheckTrades,
        CalcFFPrediction
    }

    public abstract class OpenCLTask
    {
        public OpenCLKernel Kernel { get; private set; }

        public AutoResetEvent TaskCompletedEvent { get; private set; }

        public OpenCLTask(OpenCLKernel kernel, AutoResetEvent taskCompletedEvent)
        {
            Kernel = kernel;
            TaskCompletedEvent = taskCompletedEvent;
        }

        public abstract void RunKernel(ComputeContext context, ComputeKernel kernel, ComputeCommandQueue commands, long[] dimensions);

        public long[] Dimensions { get; set; }

        public abstract void CreateBuffers();
    }

    public class CalcFFFullAndCheckTradesOpenCLTask : OpenCLTask
    {
        public int[] AllVariables;

        public int[] LowerBounds;

        public int[] UpperBounds;

        public float[] TradeProfits;
        public short[] shortTradeProfits;

        public byte[] TradeArbitrage;

        public float[] SumProfit;
        public int[] intSumProfit;

        public float[] ArbPc;

        public float[] ArbProfit;

        public float[] FitFunction;

        public int[] Size;

        public int[] ArbSize;

        public int VariablesCount;

        public ComputeBuffer<int> allvarsBuffer;

        public ComputeBuffer<int> loboundsBuffer;

        public ComputeBuffer<int> upboundsBuffer;

        public ComputeBuffer<float> tradeprofitsBuffer;
        public ComputeBuffer<short> shortTradeprofitsBuffer;

        public ComputeBuffer<byte> tradearbitrageBuffer;

        public ComputeBuffer<int> totalprofitBuffer;
        //public ComputeBuffer<float> totalprofitBuffer;

        public ComputeBuffer<float> arb_profitBuffer;

        public ComputeBuffer<float> fit_functionBuffer;

        public ComputeBuffer<int> total_sizeBuffer;

        public ComputeBuffer<int> arb_sizeBuffer;

        public ComputeBuffer<float> arb_pcBuffer;

        public CalcFFFullAndCheckTradesOpenCLTask(AutoResetEvent taskCompletedEvent) : base(OpenCLKernel.CalcFFFullAndCheckTrades, taskCompletedEvent) { }

        public override void RunKernel(ComputeContext context, ComputeKernel kernel, ComputeCommandQueue commands, long[] dimensions)
        {
            shortTradeProfits = new short[TradeProfits.Length];
            for (int i = 0; i < TradeProfits.Length; i++)
                shortTradeProfits[i] = (short)TradeProfits[i];

            allvarsBuffer = new ComputeBuffer<int>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, AllVariables);
            loboundsBuffer = new ComputeBuffer<int>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, LowerBounds);
            upboundsBuffer = new ComputeBuffer<int>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, UpperBounds);
            shortTradeprofitsBuffer = new ComputeBuffer<short>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, shortTradeProfits);
            tradearbitrageBuffer = new ComputeBuffer<byte>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, TradeArbitrage);

            intSumProfit = new int[SumProfit.Length];

            totalprofitBuffer = new ComputeBuffer<int>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, intSumProfit);

            arb_profitBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, ArbProfit);
            fit_functionBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, FitFunction);
            total_sizeBuffer = new ComputeBuffer<int>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, Size);
            arb_sizeBuffer = new ComputeBuffer<int>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, ArbSize);
            arb_pcBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, ArbPc);

            kernel.SetMemoryArgument(0, allvarsBuffer);
            kernel.SetMemoryArgument(1, loboundsBuffer);
            kernel.SetMemoryArgument(2, upboundsBuffer);
            kernel.SetMemoryArgument(3, shortTradeprofitsBuffer);
            kernel.SetMemoryArgument(4, tradearbitrageBuffer);
            kernel.SetMemoryArgument(5, totalprofitBuffer);
            kernel.SetMemoryArgument(6, arb_profitBuffer);
            kernel.SetMemoryArgument(7, fit_functionBuffer);
            kernel.SetMemoryArgument(8, total_sizeBuffer);
            kernel.SetMemoryArgument(9, arb_sizeBuffer);
            kernel.SetValueArgument<int>(10, VariablesCount);

            try
            {
                commands.Execute(kernel, null, dimensions, null, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
            try
            {
                commands.ReadFromBuffer(totalprofitBuffer, ref intSumProfit, true, null);
                for (var i = 0; i < intSumProfit.Length; i++)
                    SumProfit[i] = intSumProfit[i];
                commands.ReadFromBuffer(arb_profitBuffer, ref ArbProfit, true, null);
                commands.ReadFromBuffer(fit_functionBuffer, ref FitFunction, true, null);
                commands.ReadFromBuffer(total_sizeBuffer, ref Size, true, null);
                commands.ReadFromBuffer(arb_sizeBuffer, ref ArbSize, true, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
            commands.Finish();
        }

        public override void CreateBuffers()
        {
        }
    }

    public class CalcFFPredictionOpenCLTask : OpenCLTask
    {
        public int[] AllVariables;

        public int[] LowerBounds;

        public int[] UpperBounds;

        public short[] TradeProfits;

        public byte[] TradeArbitrage;

        public float[] FitFunction;

        public int VariablesCount;

        public ComputeBuffer<int> allvarsBuffer;

        public ComputeBuffer<int> loboundsBuffer;

        public ComputeBuffer<int> upboundsBuffer;

        public CalcFFPredictionOpenCLTask(AutoResetEvent taskCompletedEvent) : base(OpenCLKernel.CalcFFPrediction, taskCompletedEvent) { }

        public override void RunKernel(ComputeContext context, ComputeKernel kernel, ComputeCommandQueue commands, long[] dimensions)
        {
            var tradeprofitsBuffer = new ComputeBuffer<short>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, TradeProfits);
            var tradearbitrageBuffer = new ComputeBuffer<byte>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, TradeArbitrage);
            var fit_functionBuffer = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, FitFunction);

            kernel.SetMemoryArgument(0, allvarsBuffer);
            kernel.SetMemoryArgument(1, loboundsBuffer);
            kernel.SetMemoryArgument(2, upboundsBuffer);
            kernel.SetMemoryArgument(3, tradeprofitsBuffer);
            kernel.SetMemoryArgument(4, tradearbitrageBuffer);
            kernel.SetMemoryArgument(5, fit_functionBuffer);
            kernel.SetValueArgument<int>(6, VariablesCount);

            var eventList = new ComputeEventList();

            commands.Execute(kernel, null, dimensions, null, eventList);
            commands.ReadFromBuffer(fit_functionBuffer, ref FitFunction, true, null);
            commands.Finish();
        }

        public override void CreateBuffers()
        {
            loboundsBuffer = new ComputeBuffer<int>(OpenCLService.context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, LowerBounds);
            upboundsBuffer = new ComputeBuffer<int>(OpenCLService.context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, UpperBounds);
        }
    }

    public static class OpenCLService
    {
        private static string CalcFFFullAndCheckTrades = @"
#pragma OPENCL EXTENSION cl_khr_global_int32_base_atomics : enable
#pragma OPENCL EXTENSION cl_khr_local_int32_base_atomics : enable
#pragma OPENCL EXTENSION cl_khr_global_int32_extended_atomics : enable
#pragma OPENCL EXTENSION cl_khr_local_int32_extended_atomics : enable

void atomic_add_global(volatile global float *source, const float operand) {
    union {
        unsigned int intVal;
        float floatVal;
    } newVal;
    union {
        unsigned int intVal;
        float floatVal;
    } prevVal;
 
    do {
        prevVal.floatVal = *source;
        newVal.floatVal = prevVal.floatVal + operand;
    } while (atomic_cmpxchg((volatile global unsigned int *)source, prevVal.intVal, newVal.intVal) != prevVal.intVal);
}

__kernel void CalcFFFullAndCheckTrades(global int * trade_variables,
                        global int * lower_bounds, 
                        global int * upper_bounds, 
                        global short * trade_profits,
                        global uchar * trade_arbitrage,
                        volatile global int * total_profit,
                        volatile global float * arb_profit,
                        volatile global float * fit_function,
                        volatile global int * total_size,
                        volatile global int * arb_size,
                        int count_vars
                        )
{
    int i = get_global_id(0);
    int tradeIndex = get_global_id(1);
	int varsIndex = count_vars * tradeIndex;
	
	for (int varIndex = 0; varIndex < count_vars; varIndex++)
	{
			int curVar = trade_variables[varsIndex + varIndex];

			if (!(curVar > lower_bounds[count_vars*i + varIndex])) 
				return;
				
			if(!(curVar < upper_bounds[count_vars*i + varIndex]))
				return;
	}

    int profit = trade_profits[tradeIndex];
    atomic_inc(&total_size[i]);
	atomic_add(&total_profit[i], profit);
		
	int isArb = trade_arbitrage[tradeIndex];
	atomic_add(&arb_size[i], isArb);
		
	if (isArb)
		atomic_add_global(&arb_profit[i], (float)profit);
		
	if (trade_profits[tradeIndex] < 0) 
        atomic_add_global(&fit_function[i], (float)profit);
}
";

        private static string CalcFFPrediction = @"
void atomic_add_global(volatile global float *source, const float operand) {
    union {
        unsigned int intVal;
        float floatVal;
    } newVal;
    union {
        unsigned int intVal;
        float floatVal;
    } prevVal;
 
    do {
        prevVal.floatVal = *source;
        newVal.floatVal = prevVal.floatVal + operand;
    } while (atomic_cmpxchg((volatile global unsigned int *)source, prevVal.intVal, newVal.intVal) != prevVal.intVal);
}

__kernel void CalcFFPrediction(global int * trade_variables,
                        global int * lower_bounds, 
                        global int * upper_bounds, 
                        global short * trade_profits,
                        global uchar * trade_arbitrage,
                        global float * fit_function,
                        int count_vars
                        )
{
    int i = get_global_id(0);
    int tradeIndex = get_global_id(1);
		int varsIndex = count_vars * tradeIndex;
	
		for (int varIndex = 0; varIndex < count_vars; varIndex++)
		{
				int curVar = trade_variables[varsIndex + varIndex];

				if (!(curVar > lower_bounds[count_vars*i + varIndex])) 
					return;
				
				if(!(curVar < upper_bounds[count_vars*i + varIndex]))
					return;
		}

		if (trade_profits[tradeIndex] < 0 || trade_arbitrage[tradeIndex]) 
        {
            float profit = trade_profits[tradeIndex];
            atomic_add_global(&fit_function[i], profit); 
        }
}
";

        public static ComputeContext context;

        private static List<ComputeDevice> devices;

        private static Dictionary<OpenCLKernel, ComputeKernel> kernels;

        public static ConcurrentQueue<OpenCLTask> tasksQueue = new ConcurrentQueue<OpenCLTask>();

        public static AutoResetEvent TaskEnqueuedEvent = new AutoResetEvent(false);

        public static bool IsRunning { get; set; }

        private static Thread workerThread = new Thread(new ThreadStart(Worker));

        private static ComputeCommandQueue commands;

        private static ComputeEventList eventList = new ComputeEventList();

        public static ComputePlatform Platform;

        public static ComputeDevice Device;

        private static void Worker()
        {
            while (IsRunning)
            {
                OpenCLTask task;
                if (!tasksQueue.TryDequeue(out task))
                    continue;

                if (task == null)
                    continue;

                ComputeKernel kernel;
                if (!kernels.TryGetValue(task.Kernel, out kernel))
                    continue;

                try
                {
                    task.RunKernel(context, kernel, commands, task.Dimensions);
                }
                catch(Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message+"\r\n"+ex.StackTrace);
                }

                task.TaskCompletedEvent.Set();
            }
        }

        public static void Start()
        {
            IsRunning = true;
            workerThread.Start();
        }

        public static void Stop()
        {
            IsRunning = false;
            TaskEnqueuedEvent.Set();
        }

        public static void InitOpenCLService()
        {
            var properties = new ComputeContextPropertyList(Platform);
            context = new ComputeContext(ComputeDeviceTypes.All, properties, null, IntPtr.Zero);
            devices = new List<ComputeDevice>();
            devices.Add(Device);
            commands = new ComputeCommandQueue(context, Device, ComputeCommandQueueFlags.None);
            BuildKernels();
        }

        private static ComputeKernel CreateKernel(string kernelSource, string kernelName)
        {
            try
            {
                var program = new ComputeProgram(context, kernelSource);
                program.Build(devices, "-g", null, IntPtr.Zero);
                return program.CreateKernel(kernelName);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private static void BuildKernels()
        {
            kernels = new Dictionary<OpenCLKernel, ComputeKernel>();

            var CalcFFFullAndCheckTradesKernel = CreateKernel(CalcFFFullAndCheckTrades, "CalcFFFullAndCheckTrades");
            if (CalcFFFullAndCheckTradesKernel != null)
                kernels.Add(OpenCLKernel.CalcFFFullAndCheckTrades, CalcFFFullAndCheckTradesKernel);

            var CalcFFPredictionKernel = CreateKernel(CalcFFPrediction, "CalcFFPrediction");
            if (CalcFFFullAndCheckTradesKernel != null)
                kernels.Add(OpenCLKernel.CalcFFPrediction, CalcFFPredictionKernel);
        }
    }
}
