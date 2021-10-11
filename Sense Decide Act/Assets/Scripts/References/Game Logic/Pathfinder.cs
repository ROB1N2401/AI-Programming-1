using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    private static int GetCheapestDistance(Tile tileA, Tile tileB)
    {
        var xDistance = Mathf.Abs(tileB.GridPosX - tileA.GridPosX);
        var yDistance = Mathf.Abs(tileB.GridPosY - tileA.GridPosY);
        var sum = 10 * xDistance + 10 * yDistance;

        return sum;
    }

    private static List<Tile> RetracePath(Tile startTile, Tile targetTile)
    {
        var path = new List<Tile>();
        var currentTile = targetTile;
        
        while(currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }

        path.Add(currentTile);
        path.Reverse();

        return path;
    }
    
    public static List<Tile> FindPath(Tile startTile, Tile targetTile)
    {
        var openSet = new List<Tile>();
        var closedSet = new HashSet<Tile>();

        openSet.Add(startTile);

        while(openSet.Count > 0)
        {
            var currentTile = openSet[0]; 

            for(var i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].FCost < currentTile.FCost || openSet[i].FCost == currentTile.FCost && openSet[i].hCost < currentTile.hCost)
                {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if(currentTile == targetTile)
            {
                return RetracePath(startTile, targetTile);
            }

            foreach (var neighbour in WorldGrid.Instance.GetNeighbourNodes(currentTile))
            {
                if (!neighbour.IsAlive || closedSet.Contains(neighbour)) continue;
                
                var newMovementCostToNeighbour = currentTile.gCost + GetCheapestDistance(currentTile, neighbour);

                if (neighbour.gCost <= newMovementCostToNeighbour && openSet.Contains(neighbour)) continue;

                neighbour.gCost = newMovementCostToNeighbour;
                neighbour.hCost = GetCheapestDistance(neighbour, targetTile);
                neighbour.parent = currentTile;

                if(!openSet.Contains(neighbour))
                {
                    openSet.Add(neighbour);
                }
            }
        }

        //Main.Instance.simulationIsRunning = false;
        //Main.Instance.Entities["Starchaser"].StopAllCoroutines();
        Debug.LogWarning("path between two tiles could not be found");
        return null;
    }
}
