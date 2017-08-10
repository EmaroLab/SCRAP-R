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
            //Node unique id set by programmer
            int id;
            //Position in node list
            public int listPosition { get; set; }
            //Is occupied / active ?
            public bool isActive { get; set; }
            //Connections with other nodes
            public List<Link> links;
            //Cost from source node
            public double distFromSource;
            public Node(int id)
            {
                this.id = id;
                isActive = true;
                links = new List<Link>();
                Console.WriteLine("Node" + id + " created");
                distFromSource = Double.MaxValue;
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
            bool weight_generated;
            //Weight representation used for convenience
            double[,] w_matrix;
            public List<Node> nodes = new List<Node>();

            public Graph()
            {
                weight_generated = false;
                Console.WriteLine("Graph created!");
            }

            public void init()
            {
				//Nodes are sorted by ID number
				nodes.Sort((Node x, Node y) => x.getId().CompareTo(y.getId()));
				//Get number of total nodes
				num_vert = nodes.Count;

                //Helper variable listPosition after sorting
                int c = 0;
                foreach (var item in nodes)
                    item.listPosition = c++;
                 
            }
            //Generate weight matric from node list set by programmer
            public void computeWeights()
            {
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
                    weight_generated = false;
                    return;
                }
                weight_generated = true;
            }
            //Generate node list from matrix given by programmer
            public void computeWeights(double[,] matrix,int rank){

                num_vert = rank;

                if (matrix.GetLength(0) != rank || matrix.GetLength(1) != rank)
                {
                    Console.WriteLine("ERROR: matrix must be a " + rank + "X" + rank);
                    return;
                }

                w_matrix = matrix;
             
                //We need to populate node list at this point
                 nodes.Clear();
                for (int i = 0; i < rank; i++)
                {
                    var nodeTemp = new Node(i);
                    nodeTemp.listPosition = i;
                    nodes.Add(nodeTemp);
                }
                //Start linking
                for (int i = 0; i < num_vert; i++)
                {
                    for (int j = 0; j < num_vert; j++)
                    {
                        double tempW = w_matrix[i, j];
                        if ( tempW > 0)
                        {
                            nodes[i].addLink(nodes[j], tempW);
                        }
                    }
                }
                weight_generated = true;
            }
            //Print weight matrix
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
            public double[,] getWeightMatrix()
            {
                return w_matrix;
            }
            //Return the node by its id
            public virtual Node getNodeByID(int id)
            {

                Node temp;
                //Find the Node in list with the corresponding id

                if (nodes[id].getId() == id) //lucky case
                    temp = nodes[id];
                else{                        //at least we tried, search over the list
                    temp = nodes.Find((Node obj) => { return obj.getId() == id; });
                }

                return temp;
            }
            //Get the shortest path
            public LinkedList<Node> getShortestPath(int sourceId,int destId){
                var path = new LinkedList<Node>();
                var source = getNodeByID(sourceId);
                var dest = getNodeByID(destId);
                path.AddFirst(dest);
                if (weight_generated)
                {
                    //Start looking for shortest path
                    double[] dist = Dijkstra2.Dijkstra(w_matrix, source.listPosition, num_vert);
                    //Fill nodes fields, we may need it
                    int c = 0;
                    foreach (var item in dist)
                    {
                        nodes[c].distFromSource = item;
                        c++;
                    }

                    //Traverse the tree
                    int actualPId = dest.listPosition;

                    while(actualPId != source.listPosition)
                    {
                        double mindist = Double.MaxValue;
                        //Find link with lowest distance from source
                        foreach (var item in nodes[actualPId].links)
                        {
                            if (item.getAdj().distFromSource < mindist)
                            {
                                mindist = item.getAdj().distFromSource;
                                actualPId = item.getAdj().listPosition;
                            }
                        }
                        path.AddFirst(nodes[actualPId]);
                    }
                    Console.WriteLine("Path IDs: ");
                    foreach (var item in path)
                    {
                        Console.WriteLine(item.getId());
                    }

                }
                else
                    Console.WriteLine("You need to call generateWeights method first!");


                return path;
            }

           
        }//CLASS Graph
    }//NAMESPACE Navigation
}//NAMESPACE Coverage
