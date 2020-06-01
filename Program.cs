using System;
using System.Threading;

namespace ITimer
{
    class Program
    {



        public class TimerEventArgs : EventArgs
        {
            public TimerEventArgs(string message)
            {
                Message = message;
            }
            public string Message { get; private set; }
        }

        class Timer
        {
            public event EventHandler<TimerEventArgs> TimeMessage;

            public int Sec { get; set; }
            public string Name { get; set; }

            public Timer(string Name, int Sec)
            {
                this.Name = Name;
                this.Sec = Sec;
            }

            public void Foo()
            {
                for (int i = Sec; i >= 0; i--)
                {
                    Console.WriteLine("Sec: " + i);
                    Thread.Sleep(1000);
                    Console.Clear();
                }

                if (TimeMessage != null)
                    TimeMessage(this, new TimerEventArgs("Vi podpisalis na timer"));
            }

            public void MessageFromTimer1(object sender, TimerEventArgs e)
            {
                Console.WriteLine(e.Message + " Perviy podpischik " + Name);
            }


        }

        interface ICutDownNotifier
        {
            void Init();
            void Run();
        }

        /// <summary>
        /// Анонимный делегат
        /// </summary>
        class Tick1 : ICutDownNotifier
        {
            public Action<int, string> timeRefer;
            public Action<string> timeRefer1;



            public Tick1(string Name, int Sec)
            {
                this.Name = Name;
                this.Sec = Sec;
            }
            public int Sec { get; set; }
            public string Name { get; set; }


            public void Init()
            {
                timeRefer = StartTime;
                timeRefer += MessageFromTimerDelegat;
                timeRefer1 = EndTime;
            }

            public void Run()
            {
                if (timeRefer != null)
                    timeRefer(Sec, Name);

                if (timeRefer1 != null)
                    timeRefer1(Name);
            }
        }
        /// <summary>
        /// с помощью лямда выражений
        /// </summary>
        class Tick2 : ICutDownNotifier
        {
            public Action<int, string> ReferDelegat;
            public Action<string> ReferDelegat1;

            public Tick2(string Name, int Sec)
            {
                this.Name = Name;
                this.Sec = Sec;
            }

            public int Sec { get; set; }
            public string Name { get; set; }

            public void Init()
            {
                ReferDelegat = (Sec, Name) => StartTime(Sec, Name);
                ReferDelegat += (Sec, Name) => MessageFromTimerDelegat(Sec, Name);
                ReferDelegat1 = (Name) => EndTime(Name);
            }

            public void Run()
            {
                if (ReferDelegat != null)
                    ReferDelegat(Sec, Name);

                if (ReferDelegat1 != null)
                    ReferDelegat1(Name);
            }

        }
        /// <summary>
        /// с ПОМОЩЬЮ ДЕЛЕГАТОВ
        /// </summary>
        class Tick3 : ICutDownNotifier
        {
            public delegate void SomeDel(int Sec, string Name);
            public delegate void SomeDel1(string Name);

            SomeDel someDel;
            SomeDel1 someDel1;

            public Tick3(string Name, int Sec)
            {
                this.Name = Name;
                this.Sec = Sec;
            }

            public int Sec { get; set; }
            public string Name { get; set; }

            public void Init()
            {
                someDel = StartTime;
                someDel += MessageFromTimerDelegat;
                someDel1 = EndTime;
            }

            public void Run()
            {
                if (someDel != null)
                    someDel(Sec, Name);

                if (someDel1 != null)
                    someDel1(Name);
            }
        }

        public static void StartTime(int Sec, string Name)
        {
            Console.WriteLine("Start obr. otscheta " + Name + " Kol-vo sekund: " + Sec);
        }

        public static void MessageFromTimerDelegat(int Sec, string Name)
        {
            for (int i = Sec; i >= 0; i--)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Seconds =  " + i + " Name: " + Name);
            }

        }

        public static void EndTime(string Name)
        {
            Console.WriteLine("Конец обратного отсчета " + Name + " Количество секунд:" + "\n");
        }

        static void Main(string[] args)
        {


            Tick1 tick1 = new Tick1("Bishkek", 12);
            tick1.Init();
            tick1.Run();

            Tick2 tick2 = new Tick2("Tashkiria", 4);
            tick2.Init();
            tick2.Run();

            Tick3 tick3 = new Tick3("Tadzhik", 7);
            tick3.Init();
            tick3.Run();

            Timer timer1 = new Timer("lol", 3);
            timer1.TimeMessage += timer1.MessageFromTimer1;
            timer1.Foo();

        }

    }
}