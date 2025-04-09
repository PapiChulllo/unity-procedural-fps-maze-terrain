using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinMapGenerator : MonoBehaviour
{
    [Header("World Properties")]
    [Range(8, 64)] public int height = 8;
    [Range(8, 64)] public int width = 8;
    [Range(8, 64)] public int depth = 8;

    [Header("Scaling Values")]
    [Range(8, 64)] public float min = 16.0f;
    [Range(8, 64)] public float max = 24.0f;

    [Header("Tile Properties")]
    public Transform tileParent;
    public GameObject threeDTile;
    List<GameObject> grid;
    HashSet<Vector3Int> occupiedTiles;

    void Start()
    {
        grid = new List<GameObject>();
        occupiedTiles = new HashSet<Vector3Int>();
        StartCoroutine(RegenerateCoroutine());
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            Reset();
            StartCoroutine(RegenerateCoroutine());
        }
    }

    IEnumerator RegenerateCoroutine()
    {
        float randomScale = UnityEngine.Random.Range(min, max);
        float offsetX = UnityEngine.Random.Range(-1024.0f, 1024.0f);
        float offsetZ = UnityEngine.Random.Range(-1024.0f, 1024.0f);

        for (int y = 0; y < height; y++)
        {
            for (int z = 0; z < depth; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    var perlinValue = Mathf.PerlinNoise((x + offsetX) / randomScale, (z + offsetZ) / randomScale) * depth * 0.5f;

                    if (y < perlinValue)
                    {
                        GameObject tile = Instantiate(threeDTile, new Vector3(x, y, z), Quaternion.identity);
                        tile.transform.parent = tileParent;
                        grid.Add(tile);
                        occupiedTiles.Add(new Vector3Int(x, y, z));
                    }
                }
                yield return null;
            }
        }

        DisableCollidersAndRenderers();
    }

    private void DisableCollidersAndRenderers()
    {
        Vector3Int[] directions = {
            Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right,
            new Vector3Int(0, 0, 1), new Vector3Int(0, 0, -1)
        };

        foreach (var tile in grid)
        {
            Vector3Int pos = Vector3Int.RoundToInt(tile.transform.position);
            bool isHidden = true;

            foreach (var dir in directions)
            {
                if (!occupiedTiles.Contains(pos + dir))
                {
                    isHidden = false;
                    break;
                }
            }

            if (isHidden)
            {
                var boxCollider = tile.GetComponent<BoxCollider>();
                var meshRenderer = tile.GetComponent<MeshRenderer>();

                if (boxCollider != null) boxCollider.enabled = false;
                if (meshRenderer != null) meshRenderer.enabled = false;
            }
        }
    }

    private void Reset()
    {
        foreach (var tile in grid)
        {
            Destroy(tile);
        }
        grid.Clear();
        occupiedTiles.Clear();
    }
}