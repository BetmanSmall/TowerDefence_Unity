using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class VoxelTilePlacerSimple : VoxelTileGenerator {
    private void Start() {
        Debug.Log("VoxelTilePlacerSimple::Start(); -start- ");
        Init();
        StartCoroutine(Generate());
        Debug.Log("VoxelTilePlacerSimple::Start(); -end- ");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.D)) {
            StopAllCoroutines();

            foreach (VoxelTile spawnedTile in spawnedTiles) {
                if (spawnedTile != null) Destroy(spawnedTile.gameObject);
            }

            StartCoroutine(Generate());
        }
    }

    public IEnumerator Generate() {
        for (int x = 1; x < MapSize.x - 1; x++) {
            for (int y = 1; y < MapSize.y - 1; y++) {
                yield return new WaitForSeconds(0.02f);

                PlaceTile(x, y);
            }
        }

        yield return new WaitForSeconds(0.8f);
        foreach (VoxelTile spawnedTile in spawnedTiles) {
            if (spawnedTile != null) Destroy(spawnedTile.gameObject);
        }

        StartCoroutine(Generate());
    }

    private void PlaceTile(int x, int y) {
        List<VoxelTile> availableTiles = new List<VoxelTile>();

        foreach (VoxelTile tilePrefab in TilePrefabs) {
            if (CanAppendTile(spawnedTiles[x - 1, y], tilePrefab, VoxelDirection.LEFT) &&
                CanAppendTile(spawnedTiles[x + 1, y], tilePrefab, VoxelDirection.RIGHT) &&
                CanAppendTile(spawnedTiles[x, y - 1], tilePrefab, VoxelDirection.DOWN) &&
                CanAppendTile(spawnedTiles[x, y + 1], tilePrefab, VoxelDirection.UP)) {
                availableTiles.Add(tilePrefab);
            }
        }

        if (availableTiles.Count == 0) return;

        VoxelTile selectedTile = GetRandomTile(availableTiles);
        Vector3 position = selectedTile.VoxelSize * selectedTile.TileSideVoxels * new Vector3(x, 0, y);
        spawnedTiles[x, y] = Instantiate(selectedTile, position, selectedTile.transform.rotation);
    }
}
