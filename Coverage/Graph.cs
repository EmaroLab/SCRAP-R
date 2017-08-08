using System;
using System.Collections.Generic;
namespace Coverage
{
    namespace Navigation
    {

        //Basic class for link representation between nodes
        public class Link
        {

            Node adj;
            double weight;
            public Link(Node adj, double weight)
            {
                this.adj = adj;
                this.weight = weight;
                Console.WriteLine("Link  created");
            }
            public Node getAdj()
            {
                return adj;
            }
            public double getWeight()
            {
                return weight;
            }
            public void setWeight(double w)
            {
                weight = w;
            }
        }//CLASS Link

        //Simple graph node
        public class Node
        {

            int id;
            public bool isActive { get; set; }
            public List<Link> links;

            public Node(int id)
            {
                this.id = id;
                isActive = true;
                links = new List<Link>();
                Console.WriteLine("Node" + id + " created");
            }

            public int getId()
            {
                return id;
            }
            public void setId(int idValue)
            {
                id = idValue;
            }
            public void addLink(Node adj, double weight)
            {
                links.Add(new Link(adj, weight));
            }
            public void removeLink(int linkId)
            {

                links.Remove(links[linkId]);

            }
        }//CLASS Node

        //Simple graph
        public class Graph
        {

            int num_vert = 0;
            //Weight representation used for convenience
            double[,] w_matrix;
            public List<Node> nodes = new List<Node>();

            public Graph()
            {
                Console.WriteLine("Graph created!");
            }

            public void computeWeights()
            {

                //Nodes are sorted by ID number
                nodes.Sort((Node x, Node y) => x.getId().CompareTo(y.getId()));

                foreach (var item in nodes)
                {
                    Console.WriteLine(item.getId());
                }

                //Get number of total nodes
                num_vert = nodes.Count;

                //Only if we added every node to the list
                if (num_vert > 0)
                {
                    w_matrix = new double[num_vert, num_vert];

                    //Fill weighted matrix
                    for (int i = 0; i < num_vert; i++)
                    {
                        for (int j = 0; j < num_vert; j++)
                        {
                            //Set diag to zero
                            if (i == j)
                                w_matrix[i, j] = -1;
                            else
                            {
                                //Unreachable by default
                                w_matrix[i, j] = -1;

                                //Check possible links
                                if (nodes[i].links.Count > 0)
                                {
                                    foreach (var l in nodes[i].links)
                                    {

                                        if (l.getAdj().getId() == nodes[j].getId())
                                        {

                                            Console.WriteLine("Node " + nodes[i].getId() + " is connected to node: " + nodes[j].getId() + " with a weight of: " + l.getWeight());
                                            w_matrix[i, j] = l.getWeight();

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("ERROR, node list is empty");
                    return;
                }
            }

            public void printL()
            {

                for (int i = 0; i < num_vert; i++)
                {
                    for (int j = 0; j < num_vert; j++)
                    {
                        Console.Write(w_matrix[i, j] + " ");
                    }
                    Console.Write("\n");
                }   
            }
        }//CLASS Graph
    }//NAMESPACE Navigation
}//NAMESPACE Coverage
