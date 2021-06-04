using System;
using System.Reflection;
using System.Threading;
using Parcs;


namespace Queens
{
    class MainIModule : MainModule
    {
        private static int workers;
        private static int n;
        public static void Main(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                throw new ArgumentException("Options is not correct");
            }
            Int32.TryParse(args[0], out workers);
            Int32.TryParse(args[1], out n);

            var job = new Job();
            job.AddFile(Assembly.GetExecutingAssembly().Location);

            (new MainIModule()).Run(new ModuleInfo(job, null));
            Console.WriteLine("Press ESC to exit");
            while (Console.ReadKey().Key != ConsoleKey.Escape) ;
        }

        public override void Run(ModuleInfo info, CancellationToken token = default(CancellationToken))
        {
            Console.WriteLine("workers: "+ workers);
            Console.WriteLine("n: " + n);

            var points = new IPoint[workers];
            var channels = new IChannel[workers];

            for (int i = 0; i < workers; ++i)
            {
                points[i] = info.CreatePoint();
                channels[i] = points[i].CreateChannel();
                points[i].ExecuteClass("Queens.QueensIModule");
            }

            int step = n / workers;
            for (int i = 0; i < workers - 1; ++i)
            {
                channels[i].WriteData(i * step);
                channels[i].WriteData(i * step + step);
                channels[i].WriteData(n);
                // Console.WriteLine("chanel " + i + " range: " + (i * step).ToString() + " " + (i * step + step).ToString());
            }
            channels[workers - 1].WriteData((workers - 1) * step);
            channels[workers - 1].WriteData(n);
            channels[workers - 1].WriteData(n);
            // Console.WriteLine("chanel " + (workers - 1) + " range: " + ((workers - 1) * step).ToString() + " " + (n).ToString());

            DateTime time = DateTime.Now;
            Console.WriteLine("Waiting for result...");

            int res = 0;
            for (int i = 0; i < workers; ++i)
            {
                res += channels[i].ReadInt();
            }

            Console.WriteLine("Result found: res = {0}, time = {1}", res, Math.Round((DateTime.Now - time).TotalSeconds, 3));
        }
    }
}
