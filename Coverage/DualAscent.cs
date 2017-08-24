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

            //Dual ascent algorithm here, return empty list if it fails
            public LinkedList<Node> run(int source_id, int target_id)
            {
                //Init stuctures
                var qy = new double[map.getNumVert(),2];
                var shorterLinks = new List<Link>();
                bool done = false;
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
                    Console.WriteLine(done);
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

                            if(check){
                                shorterPathFound = true;
                                double eps = (qy[n,1] + link.getWeight() - qy[n_p,1]) / (qy[n_p,0] - (qy[n,0] + 1));
                                if (eps < eps_min)
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
                       // return path;

                    }
                  
                    alpha_actual += eps_min;
                    Console.WriteLine(alpha_actual);
                    map.updateWeights(alpha_actual);
                    //map.printL();
                }
				//Calculated path;

                return path;
            }
        }//CLASS DualAscent
    }//NAMESPACE Navigation
}//NAMESPACE Coverage
