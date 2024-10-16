using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathFindingManager : MonoBehaviour
{
    public int width = 22;
    public int height = 12;
    public float offset = 0.5f;

    int iterations = 1;

    // Gizmos
    Node[][] grid;
    List<Node> nodesPath;
    Node end;
    Node origin;

    private static PathFindingManager instance;

    public static PathFindingManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject singletonObject = new GameObject("PathFindingManager");
                instance = singletonObject.AddComponent<PathFindingManager>();
                DontDestroyOnLoad(singletonObject);
            }
            return instance;
        }
    }

    public void FindNextStepCoroutine(Action<List<Node>> callback, Vector2 origin, Vector2 end, Node[][] grid)
    {
        StopCoroutine(FindPath(callback, origin, end, grid));
        StartCoroutine(FindPath(callback, origin, end, grid));
    }

    public void FindNextStepRangeCoroutine(Action<List<Node>> callback, Vector2 origin, Vector2 end, Node[][] grid)
    {
        StopCoroutine(FindRangePath(callback, origin, end, grid));
        StartCoroutine(FindRangePath(callback, origin, end, grid));
    }

    private IEnumerator FindPath(Action<List<Node>> callback, Vector2 originPosition, Vector2 endPositon, Node[][] grid)
    {
        Node[][] copy = CopyGrid(grid);

        Node end = NodeFromWorldPoint(endPositon, copy);
        Node origin = NodeFromWorldPoint(originPosition, copy);

        // Si no existe camino abortamos
        if(end == origin || end.isObstacle)
        {
            callback.Invoke(new List<Node>());
            yield return null;
        }
        else
        {
            // Evitamos usar el nodo de inicio en la operaciones
            copy[(int)origin.gridPosition.x][(int)origin.gridPosition.y].visited = true;
            List<Node> firstList = new List<Node> { origin };

            List<Node> nodesPath = FindPathRecursive(new List<Node>(), firstList, end, copy);

            // Gizmos
            this.origin = origin;
            this.end = end;
            this.nodesPath = nodesPath;
            this.grid = copy;

            callback.Invoke(nodesPath);

            yield return null;
        }
    }

    private IEnumerator FindRangePath(Action<List<Node>> callback, Vector2 originPosition, Vector2 endPositon, Node[][] grid)
    {
        Node[][] copy = CopyGrid(grid);

        Node player = NodeFromWorldPoint(endPositon, copy);
        Node origin = NodeFromWorldPoint(originPosition, copy);
        Node end = EndNodeForRanged(origin, player, copy);

        // Si no existe camino abortamos
        if (end == origin || end.isObstacle)
        {
            callback.Invoke(new List<Node>());
            yield return null;
        }
        else
        {
            // Evitamos usar el nodo de inicio en la operaciones
            copy[(int)origin.gridPosition.x][(int)origin.gridPosition.y].visited = true;
            List<Node> firstList = new List<Node> { origin };

            List<Node> nodesPath = FindPathRecursive(new List<Node>(), firstList, end, copy);

            // Gizmos
            this.origin = origin;
            this.end = end;
            this.nodesPath = nodesPath;
            this.grid = copy;

            callback.Invoke(nodesPath);

            yield return null;
        }
    }

    private List<Node> FindPathRecursive(List<Node> nodesPath, List<Node> nodesSearch, Node end, Node[][] grid)
    {
        //iterations--;
        HashSet<Node> neighbors = new HashSet<Node>();

        foreach (Node node in nodesSearch)
        {
            HashSet<Node> nodeNeighbors = FindNeighbors(node, end, grid);
            foreach (Node neighbor in nodeNeighbors)
            {
                neighbor.visited = true;
                neighbors.Add(neighbor);
            }
        }

        if (neighbors.Count == 0)
        {
            return nodesPath;
        }

        List<Node> neighborsList = new List<Node>(neighbors);
        neighborsList.Sort((node1, node2) => node1.distanceToTarget.CompareTo(node2.distanceToTarget));

        if (neighbors.Contains(end)) {
            nodesPath.Add(end);
            return nodesPath;
        }
        else
        {
            //if(iterations > 0)
            FindPathRecursive(nodesPath, neighborsList, end, grid);
            if (nodesPath.Count != 0 && nodesPath[0] == end)
            {
                foreach (Node node in neighborsList)
                {
                    // ortogonales distancia 1, diagonales distancia 1,4 aprox
                    if(Vector2.Distance(nodesPath[nodesPath.Count - 1].gridPosition, node.gridPosition) < 1.5f)
                    {
                        // Añadir el nodo màs proximo
                        nodesPath.Add(node);
                        break;
                    }
                }
            }
        }

        return nodesPath;
    }


    // Obtine las corrdenadas de la cuadricula atraves de un punto del mundo
    private Node NodeFromWorldPoint(Vector2 worldPosition, Node[][] grid)
    {
        int x = Mathf.RoundToInt(((worldPosition.x - width / 2) % width) - offset);
        int y = Mathf.RoundToInt(((worldPosition.y - height / 2) % height) - offset);

        if (x < 0) x = width + x;
        if (y < 0) y = height + y;

        //Debug.Log("WorldPosition: " + worldPosition + "x: " + x + "y: " + y);
        //Debug.Log("GridLengthX: " + grid.Length + "GridLengthX: " + grid[0].Length);

        return grid[x][y];
    }


    // Copia la cuadricula
    public Node[][] CopyGrid(Node[][] original)
    {
        Node[][] copy = new Node[original.Length][];

        for (int i = 0; i < original.Length; i++)
        {
            copy[i] = new Node[original[i].Length];

            for (int j = 0; j < original[i].Length; j++)
            {
                Node originalNode = original[i][j];
                Node copyNode = new Node(originalNode.worldPosition, originalNode.gridPosition, originalNode.isObstacle);
                copy[i][j] = copyNode;
            }
        }

        return copy;
    }


    //Busca los vecinos
    private HashSet<Node> FindNeighbors(Node node, Node end, Node[][] grid)
    {
        HashSet<Node> neighbors = new HashSet<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int neighborX = (int)node.gridPosition.x + x;
                int neighborY = (int)node.gridPosition.y + y;

                // Evita salirse de los limites de la cuadricula
                if (neighborX >= 0 && neighborX < grid.Length && neighborY >= 0 && neighborY < grid[0].Length)
                {
                    if (InvalidDiagonal(x, y, node, grid)) continue;

                    Node neighborNode = grid[neighborX][neighborY];

                    if (!neighborNode.isObstacle && !neighborNode.visited)
                    {
                        float distanceToTarget = Vector2.Distance(neighborNode.gridPosition, end.gridPosition);
                        neighborNode.distanceToTarget = distanceToTarget;

                        neighbors.Add(neighborNode);
                    }
                }
            }
        }
        return neighbors;
    }


    // Compueba que la diagonal no tenga sus 2 direcciones adyacentes ocupadas
    private bool InvalidDiagonal(int x, int y, Node node, Node[][] grid)
    {
        if (x != 0 && y != 0)
        {
            int nodeX = (int)node.gridPosition.x;
            int nodeY = (int)node.gridPosition.y;

            if (grid[nodeX + x][nodeY + y].isObstacle || grid[nodeX + x][nodeY + y].visited) return true;

            if (grid[nodeX + x][nodeY].isObstacle || grid[nodeX + x][nodeY].visited) return true;

            if (grid[nodeX][nodeY + y].isObstacle || grid[nodeX][nodeY + y].visited) return true;
        }
        return false;
    }

    private Node EndNodeForRanged(Node origin, Node player, Node[][] grid)
    {
        int destinationX = (int)player.gridPosition.x;
        int destinationY = (int)player.gridPosition.y;        
        
        int originX = (int)origin.gridPosition.x;
        int originY = (int)origin.gridPosition.y;

        int emptyNodesTop;
        int emptyNodesBottom;
        int emptyNodesRight;
        int emptyNodesLeft;

        if (destinationY > originY)
        {
            emptyNodesTop = EmptyColumnPositive(destinationX, destinationY, Mathf.Clamp(destinationY + 3, 1, height - 1), grid);
            emptyNodesBottom = EmptyColumnNegative(destinationX, destinationY, destinationY - originY < 3 ? Mathf.Clamp(destinationY - 3, 1, height - 1) : originY, grid);
        }
        else
        {
            emptyNodesTop = EmptyColumnPositive(destinationX, destinationY, originY - destinationY < 3 ? Mathf.Clamp(destinationY + 3, 1, height - 1) : originY, grid);
            emptyNodesBottom = EmptyColumnNegative(destinationX, destinationY, Mathf.Clamp(destinationY - 3, 1, height - 1), grid);
        }
        if (destinationX > originX)
        {
            emptyNodesRight = EmptyRowPositive(destinationY, destinationX, Mathf.Clamp(destinationX + 3, 1, width - 1), grid);
            emptyNodesLeft = EmptyRowNegative(destinationY, destinationX, destinationX - originX < 3 ? Mathf.Clamp(destinationX - 3, 1, width - 1) : originX, grid);
        }
        else
        {
            emptyNodesRight = EmptyRowPositive(destinationY, destinationX, originX - destinationX  < 3 ? Mathf.Clamp(destinationX + 3, 1, width - 1) : originX, grid);
            emptyNodesLeft = EmptyRowNegative(destinationY, destinationX, Mathf.Clamp(destinationX - 3, 1, width - 1), grid);
        }

        Vector2 target = new Vector2(originX, originY);
        Vector2 destination = new Vector2(destinationX, destinationY);

        Vector2 top = new Vector2(destinationX, destinationY + emptyNodesTop);
        Vector2 bottom = new Vector2(destinationX, destinationY - emptyNodesBottom);
        Vector2 right = new Vector2(destinationX + emptyNodesRight, destinationY);
        Vector2 left = new Vector2(destinationX - emptyNodesLeft, destinationY);

        // Crea una lista de tuplas que almacena el vector y su nombre
        List<(Vector2 vector, string name)> vectors = new List<(Vector2, string)>()
        {
            (top, "top"), (bottom, "bottom"), (right, "right"), (left, "left")
        };

        // Listas separadas para los vectores más lejos o más cerca de 3 unidades de "destination"
        List<(Vector2 vector, string name)> farFromDestination = new List<(Vector2, string)>();
        List<(Vector2 vector, string name)> closeToDestination = new List<(Vector2, string)>();

        // Divide los vectores en dos listas basadas en la distancia a "destination"
        foreach (var v in vectors)
        {
            if (Vector2.Distance(v.vector, destination) >= 3)
            {
                farFromDestination.Add(v);
            }
            else
            {
                closeToDestination.Add(v);
            }
        }

        // Si hay al menos un vector a más de 3 unidades de "destination", ordenamos ese grupo
        if (farFromDestination.Count > 0)
        {
            farFromDestination.Sort((a, b) => Vector2.Distance(target, a.vector).CompareTo(Vector2.Distance(target, b.vector)));
            foreach (var v in farFromDestination)
            {
                return grid[(int)v.vector.x][(int)v.vector.y];
            }
        }
        else // Si no, ordenamos todos los vectores por su cercanía a "origin"
        {
            vectors.Sort((a, b) => Vector2.Distance(target, a.vector).CompareTo(Vector2.Distance(target, b.vector)));
            foreach (var v in vectors)
            {
                return grid[(int)v.vector.x][(int)v.vector.y];
            }
        }

        Debug.Log(destinationX + " " + destinationY);

        return grid[destinationX][destinationY];
    }

    private int EmptyRowPositive(int y, int destination, int enemy, Node[][] grid)
    {
        for (int i = destination + 1; i <= enemy; i++)
        {
            if (grid[i][y].isObstacle)
            {
                return i - destination - 1;
            }
        }

        return enemy - destination;
    }

    private int EmptyRowNegative(int y, int destination, int enemy, Node[][] grid)
    {
        for (int i = destination - 1; i >= enemy; i--)
        {
            if (grid[i][y].isObstacle)
            {
                return destination - i - 1;
            }
        }

        return destination - enemy;
    }

    private int EmptyColumnPositive(int x, int destination, int enemy, Node[][] grid)
    {
        for (int i = destination + 1; i <= enemy; i++)
        {
            if (grid[x][i].isObstacle)
            {
                return i - destination - 1;
            }
        }

        return enemy - destination;
    }

    private int EmptyColumnNegative(int x, int destination, int enemy, Node[][] grid)
    {
        for (int i = destination - 1; i >= enemy; i--)
        {
            if (grid[x][i].isObstacle)
            {
                return destination - i - 1;
            }
        }

        return destination - enemy;
    }

    // Pinta el camino y la cuadricula
    private void OnDrawGizmos()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Gizmos.color = grid[i][j].isObstacle ? Color.red : grid[i][j].visited ? Color.yellow : Color.blue;
                Gizmos.DrawCube(grid[i][j].worldPosition, Vector3.one * 0.4f);
            }
        }

        foreach (Node node in nodesPath)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(node.worldPosition, Vector3.one * 0.4f);
        }
        
        Gizmos.color = Color.white;
        Gizmos.DrawCube(origin.worldPosition, Vector3.one * 0.4f);

        Gizmos.color = Color.black;
        Gizmos.DrawCube(end.worldPosition, Vector3.one * 0.4f);
    }


    /*
// Pathfinding
private List<Node> FindPathRecursive(Node node, Node end, Node avoid, Node[][] grid, List<Node> nodesPath)
{
    // Evitamos usar el nodo de inicio en la operaciones
    grid[(int)node.gridPosition.x][(int)node.gridPosition.y].visited = true;

    List<Node> neighbors = FindNeighbors(node, end, avoid, grid);

    foreach (Node neighbor in neighbors)
    {
        // Encontrar destino
        if (neighbor.gridPosition == end.gridPosition)
        {
            nodesPath.Add(neighbor);
            return nodesPath;
        }

        // Marcar nodos vecinos como visitados
        foreach (Node neighborNode in neighbors)
        {
            if (neighborNode.visited)
            {
                neighbors.Remove(neighborNode);
            }
            else
            {
                neighborNode.visited = true;
            }
        }

        // Siguiente paso
        nodesPath = FindPathRecursive(neighbor, end, avoid, grid, nodesPath);

        // Añade el nodo al volver desde el final
        if (nodesPath.Count != 0 && nodesPath[0] == end)
        {
            nodesPath.Add(neighbor);
            break;
        }

        // Desvisitar nodos vecinos si no tenemos exito
        foreach (Node neighborNode in neighbors)
        {
            if (neighborNode != neighbor)
                neighborNode.visited = false;
        }
    }

    // Volvemos al paso antrerior con el camino
    grid[(int)node.gridPosition.x][(int)node.gridPosition.y].visited = false;
    return nodesPath;
}
*/
}
