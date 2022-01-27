using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB21
{

    //Садовник который двигается слева направо сверху вниз
    internal class GardenerHorizontal : Gardener
    {
        public override GardernerPostion GetNextPosition()
        {
            if (x + 1 == garden.Length)
                return y + 1 == garden.Width ? null : new GardernerPostion() { x = 0, y = y + 1 };
            else return new GardernerPostion() { x = x + 1, y = y };
        }

        public GardenerHorizontal(Garden g, ushort id) :base(g,id)
        {

        }

        public override void SetInitialPostion()
        {
            this.x = 0;
            this.y = 0;
        }
    }

    //Садовник который двигается снизу вверх  справа налево
    internal class GardenerVertical : Gardener
    {
        public override GardernerPostion GetNextPosition()
        {
            if (y - 1 == 0)
                return x - 1 == 0 ? null : new GardernerPostion() { x =x- 1, y = garden.Width-1 };
            else return new GardernerPostion() { x = x , y = y - 1 };
        }

        public GardenerVertical(Garden g, ushort id) : base(g, id)
        {

        }

        public override void SetInitialPostion()
        {
            this.x = garden.Length-1;
            this.y = garden.Width-1;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Создаём новый сад
            Garden g = new Garden(80, 25);
            //Создаём работу для садовников
            g.RegenGarden();
            //Выводим сад на экран
            Console.Write(g.ToString());

            //Создаём двух садовников
            GardenerHorizontal gh = new GardenerHorizontal(g, '1');
            GardenerVertical gv = new GardenerVertical(g,'2');

            //Пускаем их в параллели
            List<Task> Tasks = new List<Task>();
            Tasks.Add(Task.Run(() => gh.Go()));
            Tasks.Add(Task.Run(() => gv.Go()));

            //Создаём еще таск, который кончится когда закончат садовники
            Task Awaiter = Task.WhenAll(Tasks);

            for (; ; )
            {
                //Постоянно выводим на экран сад
                Console.Clear();
                Console.Write(g.ToString());
                System.Threading.Thread.Sleep(500);

                if (Awaiter.Status == TaskStatus.RanToCompletion)
                    break;

            }
            Console.WriteLine("");
            Console.WriteLine("DONE!");
            Console.ReadLine();

        }
    }
}
