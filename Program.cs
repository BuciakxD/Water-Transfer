using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog_2_prelievanie_vody
{
    class Program
    {
        static int[] state_search(int [] possible_litres, int[] state)
        {
            for (int i = 0; i < 3; i++ )
            {
                if (possible_litres[state[i]] == -1)
                    possible_litres[state[i]] = state[3];
            }
            return possible_litres;
        }
        static int[] bfs(Queue queue, int[] max_volumes, bool[,,] cube)
        {
            int[] possible_litres = new int[(Math.Max(Math.Max(max_volumes[0], max_volumes[1]), max_volumes[2]))+1];
            for (int i = 0; i < possible_litres.Length; i++)
                possible_litres[i] = -1;
            int[] current_state = new int[4];
            int[] new_state = new int[4];
            State state;
            State new_State;
            while (!queue.is_empty())
            {
                state = queue.dequeue();
                current_state[0] = state.x;
                current_state[1] = state.y;
                current_state[2] = state.z;
                current_state[3] = state.rank;
                possible_litres = state_search(possible_litres, current_state);
                for (int i = 0; i < 4; i++)
                    new_state[i] = current_state[i];
               
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (i != j)
                        {
                            while ((new_state[i] != 0) & (new_state[j] != max_volumes[j]))
                            {
                                new_state[i] -= 1;
                                new_state[j] += 1;
                            }
                            if (!cube[new_state[0],new_state[1],new_state[2]])
                            {
                                new_state[3] = current_state[3] + 1;
                                new_State = new State(new_state[0], new_state[1], new_state[2], new_state[3]);
                                queue.enqueue(new_State);
                                cube[new_state[0], new_state[1], new_state[2]] = true;
                            }
                            for (int k = 0; k < 4; k++)
                                new_state[k] = current_state[k];
                        }

                    }
                }
            }
            return possible_litres;
        }
        static void Main(string[] args)
        {
            int[] max_volumes = Input.input_states();
            int[] current_volumes = Input.input_states();
            Queue queue = new Queue(1000);
            State state = new State(current_volumes[0], current_volumes[1], current_volumes[2], current_volumes[3]);
            queue.enqueue(state);
            bool[,,] cube = new bool[11, 11, 11];
            cube[current_volumes[0], current_volumes[1], current_volumes[2]] = true;
            int[] possible_litres = bfs(queue, max_volumes, cube);
            for (int i = 0; i < possible_litres.Length; i++)
            {
                if (possible_litres[i] != -1)
                    Console.Write($"{i}:{possible_litres[i]} ");
            }
            



        }
    }

    struct State
    {
        public int x;
        public int y;
        public int z;
        public int rank;

        public State(int x, int y, int z, int rank)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.rank = rank;
        }
    }
    class Queue
    {
        State[] array;
        int array_lenght;
        int phead = 0;
        int ptail = 0;
        public Queue(int n)
        {
            this.array = new State[n];
            this.array_lenght = n;
        }

        public void enqueue(State position)
        {
            array[phead] = position;
            phead = (phead + 1) % array_lenght;
        }
        public State dequeue()
        {
            State position = array[ptail];
            ptail = (ptail + 1) % array_lenght;
            return position;
        }
        public bool is_empty()
        {
            if (ptail == phead)
                return true;
            else
                return false;
        }
    }
    class Input
    {
        static public int[] input_states()
        {
            int[] array = new int[4];
            for (int i = 0; i < 3; i++)
                array[i] = Citacka.read_int();
            return array;
        }
    }
    class Citacka
    {
        static public int read_int()
        {
            int number = Console.Read();
            bool negative = false;
            while ((number < '0') || (number > '9'))
            {
                negative = false;
                if (number == '-')
                    negative = true;
                number = Console.Read();
            }
            int result = 0;
            while ((number >= '0') && (number <= '9'))
            {
                result = (result * 10) + (number - '0');
                number = Console.Read();
            }
            if (negative)
                return -result;
            return result;
        }
    }
}
