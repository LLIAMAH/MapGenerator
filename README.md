# MapGenerator
Generate matrix map

We have the matrix like next one (generates by Map->GetInitialTerrain() function):  
0 0 0 0 0 0 0  
0 1 0 1 0 0 0  
0 1 0 0 0 0 0  
0 0 1 1 1 0 0  
0 0 1 1 1 0 0  
0 0 1 1 1 0 0  
0 0 0 0 0 0 0  
Where:  
 0 - water,  
 1 - land,  
 each cell has up to 8 neighbors.  
  
Task:  
1) Randomly generate new matrix like in example.  
2) Build new matrix with calculated cell types (Map->GetOutputTerrain(int[,] array)):  
 0 - sea => water without land neighbors,  
 1 - shallow => water with land neighbors,  
 2 - coast => land with water neighbors,  
 3 - mountains => land without water neighbors.  
3) Output to console original and final matrixes.  
