using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB21
{

    public class InvalidMoveException : Exception
    {
        public InvalidMoveException(string message) : base(message)
        {

        }

    }

    abstract class Gardener
    {
        /*
                private const int StepTimeMs = 100;
                private const int BushTimeMs = 400;
                private const int FlowerTimeMs = 60;
                private const int TrashTimeMs = 1000;*/

        private const int StepTimeMs = 1;
        private const int BushTimeMs = 2;
        private const int FlowerTimeMs = 3;
        private const int TrashTimeMs = 4;


        private readonly ushort GardenerId;

        /// <summary>
        /// Провести работу над ячейкой
        /// </summary>
        /// <param name="t">Ячейка над которой работаем</param>
        /// <returns>Вид ячейки после работы</returns>
        private CellType DoWork(CellType t)
        {
            System.Threading.Thread.Sleep(10);
            switch (t)
            {
                case CellType.BushCut:
                    System.Threading.Thread.Sleep(StepTimeMs);
                    return t;
                case CellType.Empty:
                    System.Threading.Thread.Sleep(StepTimeMs);
                    return t;
                case CellType.Flower:
                    System.Threading.Thread.Sleep(StepTimeMs);
                    return t;
                case CellType.Gardener:
                    throw new Exception("Садовники пересеклись");
                case CellType.Unknown:
                    throw new Exception("Садовник наступил на несуществующий участок");
                case CellType.FlowerSeed:
                    System.Threading.Thread.Sleep(FlowerTimeMs);
                    return CellType.Flower;
                case CellType.BushUncut:
                    System.Threading.Thread.Sleep(BushTimeMs);
                    return CellType.BushCut;
                case CellType.Trash:
                    System.Threading.Thread.Sleep(TrashTimeMs);
                    return CellType.Empty;
            }
            return CellType.Empty;
        }


        protected Garden garden;
        public Gardener(Garden g, ushort id)
        {
            garden = g;
            if (id > 59) throw new Exception("Больше 10 садовников не поддерживается!");
            GardenerId = id;
        }

        public abstract GardernerPostion GetNextPosition();
        public abstract void SetInitialPostion();

        protected uint x, y;

        /// <summary>
        /// Запускает садовника по саду.
        /// </summary>
        public void Go()
        {
            //Задать начальную позицию
            SetInitialPostion();

            for (; ; )
            {
                try
                {
                    //Получить следующую ячейку
                    var cell = garden.Step(y, x, GardenerId);
                    //Проделать работу и заменить ячейку
                    garden.ChangeCell(x, y, DoWork(cell));
                }
                catch (InvalidMoveException e)
                {
                    continue;
                }

                var Pos = GetNextPosition();
                if (Pos == null) return;
                x = Pos.x;
                y = Pos.y;
            }

        }
    }
}
