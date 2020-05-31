using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class VoxelTileGenerator : MonoBehaviour {
    public List<VoxelTile> TilePrefabs;
    public Vector2Int MapSize = new Vector2Int(10, 10);

    protected VoxelTile[,] spawnedTiles;

    protected void Init() {
        Debug.Log("VoxelTileGenerator::Start(); -start- ");
        spawnedTiles = new VoxelTile[MapSize.x, MapSize.y];

        int vTi = 0;
        foreach (VoxelTile tilePrefab in TilePrefabs) {
            // Bounds bounds = tilePrefab.gameObject.GetComponentInChildren<MeshCollider>().bounds;
            Debug.Log("tilePrefab.rend.bounds.size:" + tilePrefab.rend.bounds.size);
            tilePrefab.gameObject.transform.localPosition = new Vector3(0, 0, tilePrefab.rend.bounds.size.z * vTi++);
            tilePrefab.CalculateSidesColors();
        }

        int countBeforeAdding = TilePrefabs.Count;
        for (int i = 0; i < countBeforeAdding; i++) {
            VoxelTile clone;
            VoxelTile tilePrefab = TilePrefabs[i];
            switch (tilePrefab.Rotation) {
                case VoxelTile.RotationType.OnlyRotation:
                    break;

                case VoxelTile.RotationType.TwoRotations:
                    tilePrefab.Weight /= 2;
                    if (tilePrefab.Weight <= 0) tilePrefab.Weight = 1;

                    clone = Instantiate(tilePrefab, tilePrefab.transform.position + Vector3.right, Quaternion.identity);
                    clone.Rotate90();
                    TilePrefabs.Add(clone);
                    break;

                case VoxelTile.RotationType.FourRotations:
                    tilePrefab.Weight /= 4;
                    if (tilePrefab.Weight <= 0) tilePrefab.Weight = 1;

                    clone = Instantiate(tilePrefab, tilePrefab.transform.position + Vector3.right * 1 * tilePrefab.rend.bounds.size.x, Quaternion.identity);
                    clone.Rotate90();
                    TilePrefabs.Add(clone);

                    clone = Instantiate(tilePrefab, tilePrefab.transform.position + Vector3.right * 2 * tilePrefab.rend.bounds.size.x,
                        Quaternion.identity);
                    clone.Rotate90();
                    clone.Rotate90();
                    TilePrefabs.Add(clone);

                    clone = Instantiate(tilePrefab, tilePrefab.transform.position + Vector3.right * 3 * tilePrefab.rend.bounds.size.x,
                        Quaternion.identity);
                    clone.Rotate90();
                    clone.Rotate90();
                    clone.Rotate90();
                    TilePrefabs.Add(clone);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        Debug.Log("VoxelTileGenerator::Start(); -end- ");
    }

    protected VoxelTile GetRandomTile(List<VoxelTile> availableTiles) {
        List<float> chances = new List<float>();
        for (int i = 0; i < availableTiles.Count; i++) {
            chances.Add(availableTiles[i].Weight);
        }

        float value = Random.Range(0, chances.Sum());
        float sum = 0;

        for (int i = 0; i < chances.Count; i++) {
            sum += chances[i];
            if (value < sum) {
                return availableTiles[i];
            }
        }

        return availableTiles[availableTiles.Count - 1];
    }

    protected bool CanAppendTile(VoxelTile existingTile, VoxelTile tileToAppend, VoxelDirection direction) {
        if (existingTile == null) return true;

        if (direction == VoxelDirection.RIGHT) {
            return Enumerable.SequenceEqual(existingTile.ColorsRight, tileToAppend.ColorsLeft);
        } else if (direction == VoxelDirection.LEFT) {
            return Enumerable.SequenceEqual(existingTile.ColorsLeft, tileToAppend.ColorsRight);
        } else if (direction == VoxelDirection.UP) {
            return Enumerable.SequenceEqual(existingTile.ColorsForward, tileToAppend.ColorsBack);
        } else if (direction == VoxelDirection.DOWN) {
            return Enumerable.SequenceEqual(existingTile.ColorsBack, tileToAppend.ColorsForward);
        } else {
            throw new ArgumentException("Wrong direction value, should be Vector3.left/right/back/forward",
                nameof(direction));
        }
    }
}
