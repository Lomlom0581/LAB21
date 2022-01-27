using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB21
{

    public enum CellType
    {
        Unknown = 0,
        Empty = 1,
        FlowerSeed = 2,
        Flower = 3,
        BushUncut = 4,
        BushCut = 5,
        Trash = 6,
        Gardener = 7
    }

    class Garden
    {

        public readonly uint Width, Length;

        private Dictionary<ushort, GardernerPostion> GardenerPositions = new Dictionary<ushort, GardernerPostion>();

        //Шанс % того что в клетке окажется куст
        private const float BushChance = 10;

        //Шанс % того что в клетке окажется цветок
        private const float FlowerChance = 10;

        //Шанс % того что в клетке окажется мусор
        private const float TrashChance = 1;

        private readonly ushort[,] Map;

        public Garden(uint length, uint width)
        {
            Width = width;
            Length = length;
            Map = new ushort[width, length];
        }

        private Random rnd = new Random();

        /// <summary>
        /// Генерирует случайную ячейку для сада
        /// </summary>
        /// <returns>тип ячейки</returns>
        private CellType GenCell()
        {
            if (rnd.Next(0, 100) < BushChance) return CellType.BushUncut;
            if (rnd.Next(0, 100) < FlowerChance) return CellType.FlowerSeed;
            if (rnd.Next(0, 100) < TrashChance) return CellType.Trash;
            return CellType.Empty;
        }


        private object key = new object();

        /// <summary>
        /// Шаг садовника в саду
        /// </summary>
        /// <param name="x">Желаемая координата </param>
        /// <param name="y">Желаемая координата </param>
        /// <param name="Gardener">ID садовника</param>
        /// <returns>тип ячейки, куда наступил садовник</returns>
        public CellType Step(uint x, uint y, ushort Gardener)
        {
            lock (key)
            {
             
                //Проверить что не пересеклись садовники
                foreach(var g in GardenerPositions)
                {
                    if (g.Key == Gardener) continue;
                    if (g.Value.x == x && g.Value.y == y) throw new InvalidMoveException("Недопустимый шаг");
                }    

                //записать новую координату
                if (!GardenerPositions.ContainsKey(Gardener)) GardenerPositions[Gardener] = new GardernerPostion();
                GardenerPositions[Gardener].x = x;
                GardenerPositions[Gardener].y = y;
                return (CellType)Map[x, y];
            }
        }

        //Изменить тип ячейки по координатам
        public void ChangeCell(uint y, uint x, CellType newtype) => Map[x, y] = (ushort)newtype;


        /// <summary>
        /// Сгенерировать новый сад
        /// </summary>
        public void RegenGarden()
        {
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Length; j++)
                    Map[i, j] = (ushort)GenCell();
        }


        //Перевести тип ячейки в условное обозначение
        private char GetChar(ushort c)
        {
            switch ((CellType)c)
            {
                case CellType.BushCut: return 'П';
                case CellType.BushUncut: return '@';
                case CellType.Empty: return '.';
                case CellType.Flower: return 'Y';
                case CellType.FlowerSeed: return ',';
                case CellType.Trash: return '#';
                default: return '.';
            }
        }

        //Вывести содержимое сада в строку
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    foreach (var gp in GardenerPositions)
                        if (gp.Value.x == i && gp.Value.y == j)
                        {
                            sb.Append((char)gp.Key);
                            goto end;
                        }
                    
                    sb.Append(GetChar(Map[i, j]));

                end:;
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }




    }
}
