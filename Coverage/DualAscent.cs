using System;
using System.Collections.Generic;
namespace Coverage
{
    namespace Navigation
    {
        public class DualAscent
        {
            Map map;
            double alpha_actual;
            int n_vehicles;
            double eps_min;

            public DualAscent(Map m, int n_vehicles)
            {
                map = m;
                alpha_actual = CostFunctions.alpha;
                this.n_vehicles = n_vehicles;
                eps_min = Double.MaxValue;

            }
            public void setNVehicles(int n){
                n_vehicles = n;
            }
			public void reset()
			{
                alpha_actual = 0;
				eps_min = Double.MaxValue;
                map.updateWeights();
			}
            //Checks if current link is in a general path
            bool isLinkInPath(LinkedList<Node> path, Link link){

                int id_s = link.getOwnerNode().getId();
                int id_t = link.getAdj().getId();

                bool sourceFound = false;

                List<int> tempId = new List<int>();
                foreach (var item in path)
                {
                    if(sourceFound){
                        if (tempId.Exists((obj) => { return obj == id_t; }))
                            return true;
                    }

                    //Link starts in this node
                    if (item.getId() == id_s)
                    {
                        sourceFound = true;
                        tempId.Clear();
                        foreach (var testLink in item.links)
                        {
                            //Store adjoint ids for next iteration
                            tempId.Add(testLink.getAdj().getId());
                        }

                    }
                    else
                        sourceFound = false;
                    
                }
                //no mathing link
                return false;;
            }


            //Dual ascent algorithm here, return empty list if it fails
            public LinkedList<Node> run(int source_id, int target_id)
            {
                //Init stuctures
                var qy = new double[map.getNumVert(),2];
                bool done = false;
                var alpha_prev = alpha_actual;
                var path = new LinkedList<Node>();
                path = map.getShortestPath(source_id, target_id);
				if (path.Count - 2 <= n_vehicles)
					done = true;
                /*
                foreach (var item in path)
				{
					Console.WriteLine(item.getId());
				}
                Console.WriteLine("***");
*/             // map.printL();
                while(!done)
		        {
                    
                    path = map.getShortestPath(source_id, target_id);
                    if (path.Count - 2 <= n_vehicles)
                    {
						Console.WriteLine("Path found!");
						foreach (var item in path)
						{
							Console.WriteLine(item.getId());
						}
                        done = true;
                        break;
                    }
                    foreach (var item in path)
					{
						Console.WriteLine(item.getId());
					}
					
					Console.WriteLine("***");
	                //Calculate qn,yn
                    int i = 0;		                    
                    foreach (var item in map.nodes)		               
                    {	                        	                      	                     
                        var path_temp = map.traverse(map.getNodeByID(source_id), item);             
                        var hops = path_temp.Count - 1 ;		                     
                        if (hops < 0)		                     
                            hops = 0;
		                      
                        qy[item.getId(), 0] = hops;		                     
                        qy[item.getId(), 1] = item.distFromSource;
                        i++;		                   
                    }
                    //Check for shorter links 
                   
                    bool shorterPathFound = false;
                    foreach (var node in map.nodes)
                    {
                        foreach (var link in node.links)
                        {
                            int n = node.getId();
                            int n_p = link.getAdj().getId();
                            //q_n [n'] > q_n[n] + 1
                            //Debug
                            double qnp = qy[n_p, 0];
                            double qn = qy[n, 0];
                            bool check = qnp > qn + 1;

                            if (check)
                            {
                                shorterPathFound = true;
                                double eps = (qy[n, 1] + link.getWeight() - qy[n_p, 1]) / (qy[n_p, 0] - (qy[n, 0] + 1));
                                if (eps < eps_min  && eps > 0.001) //Magic number, avoid to get stuck with eps very low
                                    eps_min = eps;
                            }
                        }
                    }

                    //See if we found a better path, otherwise return an empty list
                    if (!shorterPathFound)
                    {
                        done = true;
                        path.Clear();
                        Console.WriteLine("Path not found");
                        return path;

                    }

                    alpha_actual += eps_min;
                    Console.WriteLine("alpha: " + alpha_actual + " EpsMin: " + eps_min);
                    alpha_prev = alpha_actual;
                    map.updateWeights(alpha_actual);
                    //map.printL();
                }
				//Calculated path;

                return path;
            }
        }//CLASS DualAscent
    }//NAMESPACE Navigation
}//NAMESPACE Coverage
