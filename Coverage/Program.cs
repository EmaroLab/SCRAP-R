using System;


namespace Coverage
{
    using  Navigation;

    class MainClass
    {
        public static void Main(string[] args)
        {

            Graph g = new Graph();
           
            Node n0 =  new Node(0);
            Node n1 = new Node(1);
            Node n2 = new Node(2);
            Node n3 = new Node(3);
            Node n4 = new Node(4);
            Node n5 = new Node(5);
            Node n6 = new Node(6);
            Node n7 = new Node(7);
           

            g.nodes.Add(n0);
            g.nodes.Add(n1);
            g.nodes.Add(n2);
            g.nodes.Add(n3);
            g.nodes.Add(n4);
            g.nodes.Add(n5);
            g.nodes.Add(n6);

            Console.WriteLine("list size: "+g.nodes.Count);
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

            //Dijk test
            var d = new Dijkstra(7, L);

			// d.Run();
            g.init();
			g.computeWeights(L);
			g.printL();
            g.getShortestPath(n0,n4);


        }
    }
}
