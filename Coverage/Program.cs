using System;
using System.Collections.Generic;

namespace Coverage
{
    using Navigation;
    class MainClass
    {
        public static void printMap(Map m, LinkedList<Node> p)
        {
			//Print map
			var stamp = new int[m.rows, m.cols];
			for (int r = 0; r < m.rows; r++)
			{
				for (int c = 0; c < m.cols; c++)
				{
					stamp[r, c] = 0;
					if (!m.getNodeByID(m.getNodeIdFromCell(r, c)).isActive)
						stamp[r, c] = 9;

				}
			}
			foreach (NavNode item in p)
			{
				stamp[item.xCell, item.yCell] = 1;
			}
			for (int r = 0; r < m.rows; r++)
			{
				for (int c = 0; c < m.cols; c++)
				{
					Console.Write(stamp[r, c] + " ");
				}
				Console.WriteLine("");
			}
        }

        public static void Main(string[] args)
        {

            //You can create an arbitrary graph
            Graph g = new Graph();
            Node n0 = new Node(0);
            Node n1 = new Node(1);
            Node n2 = new Node(2);
            Node n3 = new Node(3);
            Node n4 = new Node(4);
            Node n5 = new Node(5);
            Node n6 = new Node(6);
            Node n7 = new Node(7);

            //Add nodes to it
            g.nodes.Add(n0);
            g.nodes.Add(n1);
            g.nodes.Add(n2);
            g.nodes.Add(n3);
            g.nodes.Add(n4);
            g.nodes.Add(n5);
            g.nodes.Add(n6);
            /*
             try
             {
                 Console.WriteLine(g.nodes.Find((Node obj) => { return obj.getId() == 1; }).getId());
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);

             }

             Console.WriteLine("list size: " + g.nodes.Count);
 */
            //You can directly provide a weight matrix instead
            double[,] L ={
                {-1,  5, -1, -1, -1,  3, -1, -1},
                { 5, -1,  2, -1, -1, -1,  3, -1},
                {-1,  2, -1,  6, -1, -1, -1, 10},
                {-1, -1,  6, -1,  3, -1, -1, -1},
                {-1, -1, -1,  3, -1,  8, -1,  5},
                { 3, -1, -1, -1,  8, -1,  7, -1},
                {-1,  3, -1, -1, -1,  7, -1,  2},
                {-1, -1, 10, -1,  5, -1,  2, -1}
            };

            //Init must be called once before operations
            g.init();

            //This generates nodes structure from matrix. Otherwise you can use computeWeights(); if you provided nodes; this will create the weight matrix
			g.computeWeights(L, 8);

            //Return a linked list with the shortest path
            var path = g.getShortestPath(0, 2);

            //Dual ascent test
            Map m = new Map(30, 30, 1);
            m.addObstacle(1, 1);
            m.addObstacle(3, 2);
            m.addObstacle(2, 1);
            m.addObstacle(1, 0);
            m.addObstacle(1, 2);
            //Init must be called after adding obstacles
            m.init();
            var asc = new DualAscent(m,3);
            var s = new int[2];
            s[0] = 0;
            s[1] = 1;
			var t = new int[2];
            t[0] = 4;
            t[1] = 0;
            var p = asc.run(m.getNodeIdFromCell(s[0], s[1]), m.getNodeIdFromCell(t[0], t[1]));

            //Print map
            printMap(m, p);
            Console.WriteLine("*******************************");
            asc.reset();
            p = asc.run(m.getNodeIdFromCell(s[0], 0), m.getNodeIdFromCell(t[0], t[1]));
            printMap(m, p);

        }
    }
}
