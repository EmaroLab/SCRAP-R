using System;


namespace Coverage
{
    using  Navigation;

    class MainClass
    {
        public static void Main(string[] args)
        {

            Graph g = new Graph();
           
            Node n =  new Node(0);
            Node n2 = new Node(1);
            Node n3 = new Node(7);
            Node n4 = new Node(6);
            Node n5 = new Node(8);
            Node n6 = new Node(12);
            Node n7 = new Node(22);

            g.nodes.Add(n);
            g.nodes.Add(n2);
            g.nodes.Add(n3);
            g.nodes.Add(n4);
            g.nodes.Add(n5);
            g.nodes.Add(n6);
            g.nodes.Add(n7);


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
            n.addLink(n3,8);
            n.addLink(n4,2);
            n4.addLink(n2, 80);
            n3.addLink(n,666);
            n3.addLink(n4, 9);
            n6.addLink(n3,76);
            n7.addLink(n,24);
            n7.addLink(n5, 21);

            g.computeWeights();
            g.printL();

            n7.removeLink(0);

			g.computeWeights();
			g.printL();

        }
    }
}
