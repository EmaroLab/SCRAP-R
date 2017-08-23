using System;
namespace Coverage
{

    namespace Navigation
    {
        public class Dijkstra
        {
			private int rank = 0;
			private double[,] L;
			private int[] C;
			public double[] D;
			private int trank = 0;
			
            public Dijkstra(int paramRank, double[,] paramArray)
			{
				L = new double[paramRank, paramRank];
				C = new int[paramRank];
				D = new double[paramRank];
				rank = paramRank;
				for (int i = 0; i < rank; i++)
				{
					for (int j = 0; j < rank; j++)
					{
						L[i, j] = paramArray[i, j];
					}
				}

				for (int i = 0; i < rank; i++)
				{
					C[i] = i;
				}
				C[0] = -1;
				for (int i = 1; i < rank; i++)
					D[i] = L[0, i];
			}
			public void DijkstraSolving()
			{
                double minValue = Double.MaxValue;
				int minNode = 0;
				for (int i = 0; i < rank; i++)
				{
					if (C[i] == -1)
						continue;
					if (D[i] > 0 && D[i] < minValue)
					{
						minValue = D[i];
						minNode = i;
					}
				}
				C[minNode] = -1;
				for (int i = 0; i < rank; i++)
				{
					if (L[minNode, i] < 0)
						continue;
					if (D[i] < 0)
					{
						D[i] = minValue + L[minNode, i];
						continue;
					}
					if ((D[minNode] + L[minNode, i]) < D[i])
						D[i] = minValue + L[minNode, i];
				}
			}
			public void Run()
			{
               
				for (trank = 1; trank < rank; trank++)
				{
					DijkstraSolving();
                    Console.Write("D: ");
                    for (int i = 0; i < rank; i++)
                    {
                        
                        Console.Write(D[i] + " ");
                    }
					Console.WriteLine("");
                    Console.Write("C: ");
                    for (int i = 0; i < rank; i++)
                    {
                        
                        Console.Write(C[i] + " ");
                    }
					Console.WriteLine("");
				}

             
			}
        }//CLASS Dijkstra

        public class Dijkstra2
        {
            private static int MinimumDistance(double[,] distance, bool[] shortestPathTreeSet, int verticesCount)
            {
                double min = double.MaxValue;
                int minIndex = 0;

                for (int v = 0; v < verticesCount; ++v)
                {
                    if (shortestPathTreeSet[v] == false && distance[v,0] <= min)
                    {
                        min = distance[v,0];
                        minIndex = v;
                    }
                }

                return minIndex;
            }

            private static void Print(double[] distance, int verticesCount)
            {
                Console.WriteLine("Vertex    Distance from source");

                for (int i = 0; i < verticesCount; ++i)
                    Console.WriteLine("{0}\t  {1}", i, distance[i]);
            }

            public static double[,] Dijkstra(double[,] graph, int source, int verticesCount)
            {
                double[,] distance = new double[verticesCount,2];
                bool[] shortestPathTreeSet = new bool[verticesCount];

                for (int i = 0; i < verticesCount; ++i)
                {
                    distance[i,0] = int.MaxValue;
                    shortestPathTreeSet[i] = false;
                }

                distance[source,0] = 0;

                for (int count = 0; count < verticesCount - 1; ++count)
                {
                    int u = MinimumDistance(distance, shortestPathTreeSet, verticesCount);
                    shortestPathTreeSet[u] = true;

                    for (int v = 0; v < verticesCount; ++v)
                    {
                        var res = graph[u, v] != -1;
                        if (!shortestPathTreeSet[v] && res && distance[u,0] != int.MaxValue && distance[u,0] + graph[u, v] < distance[v,0])
                        {
                            distance[v,0] = distance[u,0] + graph[u, v];
                            //Store predecessors
                            distance[v, 1] = u;
                        }

                    }
                }

                return distance;
            }


        }//CLASS Dijkstra2
	}//NAMESPACE Navigation
}//NAMESPACE Coverage
