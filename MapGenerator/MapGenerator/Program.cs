using System;

namespace MapGenerator
{
    class Program
    {
        const int mapSize = 16;

        static void Main(string[] args)
        {
            var map = new Map(mapSize);
            var initial = map.GetInitialTerrain();
            OutputArray(initial, mapSize);
            var result = map.GetOutputTerrain(initial);
            OutputArray(result, mapSize);
        }

        private static void OutputArray(int[,] array, int mapSize)
        {
            // output generated array
            for (var i = 0; i < mapSize; i++)
            {
                for (var j = 0; j < mapSize; j++)
                    Console.Write($"{array[i, j]}");

                Console.WriteLine();
            }

            Console.WriteLine("=========================================");
        }
    }

    public class Map
    {
        protected Random _rnd;
        private int _size;
        private const double _groundCoefficient = 0.5;

        public Map(int size)
        {
            _size = size;
            _rnd = new Random();
        }

        protected int RandomSimplify()
        {
            return _rnd.NextDouble() > _groundCoefficient ? 1 : 0;
        }

        public int[,] GetInitialTerrain()
        {
            var array = new int[this._size, this._size];
            for (var i = 0; i < this._size; i++)
                for (var j = 0; j < this._size; j++)
                    array[i, j] = RandomSimplify();

            return array;
        }

        public int[,] GetOutputTerrain(int[,] array)
        {
            var tempArray = GetExtendedMap(array);
            var output = new int[_size, _size];
            // output generated array
            for (var i = 0; i < this._size; i++)
            {
                for (var j = 0; j < this._size; j++)
                {
                    switch (array[i, j])
                    {
                        case 0:
                            {
                                if (tempArray[i, j].Value == 0)
                                    output[i, j] = 0;
                                else
                                    output[i, j] = 1;
                                break;
                            }
                        case 1:
                            {
                                if (tempArray[i, j].Value == tempArray[i, j].SumItemsNew)
                                    output[i, j] = 3;
                                else
                                    output[i, j] = 2;
                                break;
                            }
                    }
                }
            }

            return output;
        }

        public MergeResult[,] GetExtendedMap(int[,] array)
        {
            var output = new MergeResult[this._size, this._size];
            for (var i = 0; i < this._size; i++)
                for (var j = 0; j < this._size; j++)
                    output[i, j] = GetTerrainEx(array, i, j);

            return output;
        }

        private MergeResult GetTerrainEx(int[,] inArray, int icoor, int jcoor)
        {
            return new X9(inArray, _size, icoor, jcoor).GetAround();
        }
    }

    public class XCell
    {
        public int I { get; private set; }
        public int J { get; private set; }
        public int V { get; private set; }
        public bool Consider { get; private set; }

        public XCell(int i, int j, int[,] v, int maxSize)
        {
            try
            {
                this.V = v[i, j];
                this.Consider = true;
            }
            catch (IndexOutOfRangeException)
            {
                this.Consider = false;
            }

            this.I = i;
            this.J = j;
        }
    }

    public class X9
    {
        private const int _defaultItemsCount = 8;
        private readonly XCell[,] _x9 = new XCell[3, 3];

        // ReSharper disable once InconsistentNaming
        protected XCell NW => _x9[0, 0];
        protected XCell N => _x9[1, 0];
        // ReSharper disable once InconsistentNaming
        protected XCell NE => _x9[2, 0];

        protected XCell W => _x9[0, 1];
        protected XCell C => _x9[1, 1];
        protected XCell E => _x9[2, 1];

        // ReSharper disable once InconsistentNaming
        protected XCell SW => _x9[0, 2];
        protected XCell S => _x9[1, 2];
        // ReSharper disable once InconsistentNaming
        protected XCell SE => _x9[2, 2];

        public X9(int[,] array, int maxSize, int i, int j)
        {
            _x9[0, 0] = new XCell(i - 1, j - 1, array, maxSize);
            _x9[1, 0] = new XCell(i, j - 1, array, maxSize);
            _x9[2, 0] = new XCell(i + 1, j - 1, array, maxSize);

            _x9[0, 1] = new XCell(i - 1, j, array, maxSize);
            _x9[1, 1] = new XCell(i, j, array, maxSize);
            _x9[2, 1] = new XCell(i + 1, j, array, maxSize);

            _x9[0, 2] = new XCell(i - 1, j + 1, array, maxSize);
            _x9[1, 2] = new XCell(i, j + 1, array, maxSize);
            _x9[2, 2] = new XCell(i + 1, j + 1, array, maxSize);
        }

        public MergeResult AddItem(XCell item, MergeResult mr)
        {
            if (item.Consider)
            {
                return new MergeResult()
                {
                    Value = mr.Value + item.V,
                    SumItemsNew = mr.SumItemsNew,
                };
            }

            return new MergeResult()
            {
                Value = mr.Value,
                SumItemsNew = --mr.SumItemsNew
            };
        }

        public MergeResult GetAround()
        {
            var data = AddItem(NW, new MergeResult() { Value = 0, SumItemsNew = _defaultItemsCount });
            data = AddItem(N, data);
            data = AddItem(NE, data);
            data = AddItem(W, data);
            data = AddItem(E, data);
            data = AddItem(SW, data);
            data = AddItem(S, data);
            data = AddItem(SE, data);

            return data;
        }
    }

    public class MergeResult
    {
        public int Value { get; set; }
        public int SumItemsNew { get; set; }

        public override string ToString()
        {
            return $"({Value}, {SumItemsNew})";
        }
    }
}
