using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] regularTiles;
    [SerializeField] GameObject goalTile;
    [SerializeField] GameObject startTile;

    [SerializeField] int mapSize;
    [SerializeField] float tileSize;
    [SerializeField] int numberOfGoalTiles;

    int startTileIndex;
    List<GameObject> maze = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                GameObject tile = Instantiate(regularTiles[UnityEngine.Random.Range(0, regularTiles.Length)],
                    new Vector3(i * tileSize, 0, j * tileSize),
                    Quaternion.Euler(-90, 0, UnityEngine.Random.Range(0, 4) * 90), transform);
                maze.Add(tile);
            }
        }

        startTileIndex = UnityEngine.Random.Range(0, maze.Count);
        ReplaceTile(startTileIndex, startTile);
        FindObjectOfType<PlayerBehaviour>().transform.position = maze[startTileIndex].transform.position + Vector3.up * 10;

        int goalTileIndex;
        for (int i = 0; i < numberOfGoalTiles; i++)
        {
            do
            {
                goalTileIndex = UnityEngine.Random.Range(0, maze.Count);
            } while (goalTileIndex == startTileIndex);

            ReplaceTile(goalTileIndex, goalTile);
        }
    }

    void ReplaceTile(int replaceIndex, GameObject replaceObject)
    {
        GameObject start = Instantiate(replaceObject, maze[replaceIndex].transform.position, Quaternion.identity, transform);
        Destroy(maze[replaceIndex]);
        maze[replaceIndex] = start;
    }
}

