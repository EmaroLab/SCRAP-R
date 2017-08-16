using System;

namespace Coverage
{
    namespace Navigation
    {

        public class CostFunctions{

            public static double vehicleAction = 2;
            public static double alpha = 0;

            public static double euclideanDist(NavNode s, NavNode t){
                double sqrSum = Math.Pow(t.yCoord - s.yCoord, 2) + Math.Pow(t.xCoord - s.xCoord, 2);
                return Math.Sqrt(sqrSum);
            }
			public static double commCost(NavNode s, NavNode t, int nInsideObst = 0)
			{
                //TODO: define it better
                return 0.8 * euclideanDist(s,t) + nInsideObst / 10;    			
            }

        }

        public  class ObstacleFunc
        {

        }

        public class NavNode : Node {

            public double xCoord { get; set; }
            public double yCoord { get; set; }

            public int xCell { get; set; }
            public int yCell { get; set; }

            public int nvisited { get; set; }

            public NavNode(int id, int x, int y) : base(id)
            {
                xCell = x;
                yCell = y;
                nvisited = 0;
            }
            public void setPosition(double x, double y){
                xCoord = x;
                yCoord = y;
            }

        }


        public class Map : Graph
        {

            public int rows { get; }
            public int cols { get; }
            double resolution;
            //False when obstacle is present
            //bool[,] obstMask;

            public double vehicleAction { get; set; }


            public Map(int rows, int cols, double res)
            {
                this.rows = rows;
                this.cols = cols;
                resolution = res;
                vehicleAction = CostFunctions.vehicleAction;
                /*
                //Init obstacles mask
                obstMask = new bool[rows, cols];
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < cols; j++)
					{
						obstMask[i, j] = true;

					}
				}
                */
                //Fill the node list
                generateNodes();
                //Generate weight matrix as Dijkstra input
                initLinks();
                computeWeights();

            }

            void generateNodes()
            {
                int id = 0;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        var tempNode = new NavNode(id++, i, j); 
                        tempNode.setPosition(resolution * (i + 0.5),resolution * (j + 0.5));
                       // tempNode.isActive = obstMask[i, j];
                        nodes.Add(tempNode);
                    }
                }
                init();
            }
            //Set a cell to be an obstacle.
            public void addObstacle(int xCell,int yCell){
                getNodeByID(getNodeIdFromCell(xCell, yCell)).isActive = false;
               // obstMask[xCell, yCell] = false;
            }
            //Links creation based on obstacles, vehicle action and initial weights
            void initLinks(){


                //Bad way to check if line intersects a cell but fast to implement. 
                var samplingN = resolution * 10;
                var delta = vehicleAction / samplingN;

                foreach (var item in nodes)
                {
                    if (item is NavNode)
					{
                        //Find max y and x respect to vehicle action
                        int maxX;
                        int maxY;

                        double xMaxCoord = ((NavNode)item).xCoord + vehicleAction;
                        double yMaxCoord = ((NavNode)item).yCoord + vehicleAction;

                        maxX = getCellFromCoord(xMaxCoord, yMaxCoord)[0];
                        maxY = getCellFromCoord(xMaxCoord, yMaxCoord)[1];

						//Find min y and x respect to vehicle action
						int minX;
						int minY;

						double xMinCoord = ((NavNode)item).xCoord - vehicleAction;
						double yMinCoord = ((NavNode)item).yCoord - vehicleAction;
						minX = getCellFromCoord(xMinCoord, yMinCoord)[0];
						minY = getCellFromCoord(xMinCoord, yMinCoord)[1];
                       
                        //Clamp
                        if (maxX > rows - 1)
                            maxX = rows - 1;
                        if (minX < 0)
							minX = 0;
                        if (maxY > cols - 1)
                            maxY = cols - 1;
						if (minY < 0)
							minY = 0;

                        //Connect nodes depending on vehicle action and obstacles. Initial weight mased on initial cost function params
                        for (int i = minX; i <= maxX; i++)
                        {
                            for (int j = minY; j <= maxY; j++)
                            {
                                //Find attachable nodes

                                NavNode target = (NavNode)getNodeByID(getNodeIdFromCell(i, j));
                                double euclDistToTarget = CostFunctions.euclideanDist((NavNode)item, target);
                                bool check = (item.getId() != target.getId() && target.isActive && (euclDistToTarget <= vehicleAction));
                                if (check)
                                {

                                    //Target node is not an obstacle and is reachable, check if there is an obstacle in the middle.
                                    double maxx = Math.Max(((NavNode)item).xCell, target.xCell);
                                    double minx = Math.Min(((NavNode)item).xCell, target.xCoord);
                                    double maxy = Math.Max(((NavNode)item).yCell, target.yCell);
                                    double miny = Math.Min(((NavNode)item).yCell, target.yCell);

                                    //Extrapolate line params
                                    double m = (target.yCoord - ((NavNode)item).yCoord) / (target.xCoord - ((NavNode)item).xCoord);
                                    double q = ((NavNode)item).yCoord - m * ((NavNode)item).xCoord;
                                    var versor = new double[2];
                                    versor[0] = (target.xCoord - ((NavNode)item).xCoord) / euclDistToTarget;
                                    versor[1] = (target.yCoord - ((NavNode)item).yCoord) / euclDistToTarget;

                                    //Sample the line and count how many points lie inside an obstacle

                                    int pInside = 0;
                                    for (int s = 0; s < samplingN; s++)
                                    {
                                        double x = versor[0] * delta * s + ((NavNode)item).xCoord;
                                        double y = versor[1] * delta * s + ((NavNode)item).yCoord;

                                        //Check if point lies inside an obstacle. We use mask since is faster than getting a node

                                        if (getNodeByID(getNodeIdFromCell(getCellFromCoord(x,y)[0],getCellFromCoord(x,y)[1])).isActive)
                                            pInside++;
                                    }

                                    //choose weights
                                    int count = target.nvisited;
                                    double alpha = CostFunctions.alpha;
                                    double comm = CostFunctions.commCost((NavNode)item, target);
                                    double W = count + comm + alpha;
                                    // And finally, link
                                    item.addLink(target, W);
                                }

                            }

                        }

                    }

				}

            }
            public void updateWeights(double alpha){

                foreach (var node in nodes)
                {
                    foreach (var link in node.links){
                        NavNode n = (NavNode)link.getAdj();
                        var newWeight = n.nvisited + alpha;
                        link.setWeight(newWeight);
                        w_matrix[node.getId(), n.getId()] = newWeight;
                    }
                }


            }
            //returns the cell x and y from the spatial point
            public int[] getCellFromCoord(double xCoord, double yCoord){

                if (xCoord > rows * resolution || yCoord > cols * resolution || xCoord < 0 || yCoord < 0)
                {
                    Console.WriteLine("ERROR: coordinates outside map space");
                }

                var t = new int[2];
                double x;
                double y;

                if(Math.Abs(xCoord % resolution) <= Double.Epsilon)
                    x = (xCoord / resolution) - 1;
                else
                    x = (xCoord / resolution);

				if (Math.Abs(yCoord % resolution) <= Double.Epsilon)
                    y = (yCoord / resolution) - 1;
				else
                    y = (yCoord / resolution);
                
                if (x<0)
                    x = 0;
                if (y<0)
                    y = 0;
                if (x > rows - 1)
                    x = rows - 1;
				if (y > cols - 1)
					y = cols - 1;
                t[0] = (int)x;
				t[1] = (int)y;



                return t;

			}
            //returns cell center of mass from x and y
			public double[] getCoordFromCell(int xCell, int yCell)
			{
      
                var t = new double[2];
                t[0] =  resolution * (xCell + 0.5);
                t[1] =  resolution * (yCell + 0.5);

                return t;
			}
            public int  getNodeIdFromCell(int xCell, int yCell){
                return xCell*cols + yCell;
            }

        }//Class Map
    }//Namespace Navigation
}//Namespace Coverage
