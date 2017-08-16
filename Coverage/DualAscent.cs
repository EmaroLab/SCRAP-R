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
                eps_min = 0;
            }
            public void setNVehicles(int n){
                n_vehicles = n;
            }

            //Dual ascent algorithm here
            public LinkedList<Node> run(int source_id, int target_id)
            {
                //Init stuctures
                var qv = new double[map.getNumVert(),2];

                bool done = false;

                var path = map.getShortestPath(source_id, target_id);
                if (path.Count <= n_vehicles)
                  //  done = true;

                while(!done)
                {

                    //Calculate qn,yn
                    int i = 0;
                    foreach (var item in map.nodes)
                    {
                        var path_temp = map.traverse(map.getNodeByID(source_id), item);
                        var hops = path_temp.Count - 2;
                        if (hops < 0)
                            hops = 0;
                        qv[i, 0] = hops;
                        qv[i, 1] = item.distFromSource;
                        i++;
                    }
                    //Check links ???


                    for (int it = 0; it < map.rows; it++)
                    {
                            for (int j = 0; j < map.cols; j++)
                            {
                                Console.Write(qv[map.getNodeIdFromCell(it,j),1] + " ");
                            }
                        Console.WriteLine("\n");
                    }

                    done = true;
                }
                return path;
            }
        }//CLASS DualAscent
    }//NAMESPACE Navigation
}//NAMESPACE Coverage
