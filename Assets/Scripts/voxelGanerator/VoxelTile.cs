using System;
using UnityEngine;

public class VoxelTile : MonoBehaviour {
    public float VoxelSize = 0.1f;
    public int TileSideVoxels = 8;

    [Range(1, 100)] public int Weight = 50;

    public RotationType Rotation;
    public Renderer rend;
    // public Collider meshCollider;

    public enum RotationType {
        OnlyRotation,
        TwoRotations,
        FourRotations
    }

    private void Awake() {
        if (rend == null) {
            rend = gameObject.GetComponentInChildren<Renderer>();
            rend.gameObject.transform.localPosition = new Vector3(rend.bounds.size.x / 2, 0, rend.bounds.size.z / 2);
            TileSideVoxels = (int)(rend.bounds.size.x * 10);
        }
        // if (meshCollider == null) {
            // meshCollider = gameObject.GetComponentInChildren<Collider>();
        // }
        // Debug.Log("1rend:" + rend);
    //     Debug.Log("1meshCollider:" + meshCollider);
    }

    void OnDrawGizmosSelected() {
        if (rend != null) {
            // Debug.Log("rend:" + rend);
            // Debug.Log("rend.bounds:" + rend.bounds);
            // A sphere that fully encloses the bounding box.
            Vector3 center = rend.bounds.center;
            // Debug.Log("center:" + center);
            float radius = rend.bounds.extents.magnitude;

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(center, radius);
        }
    }

    [HideInInspector] public byte[] ColorsRight;
    [HideInInspector] public byte[] ColorsForward;
    [HideInInspector] public byte[] ColorsLeft;
    [HideInInspector] public byte[] ColorsBack;

    public void CalculateSidesColors() {
        ColorsRight = new byte[TileSideVoxels * TileSideVoxels];
        ColorsForward = new byte[TileSideVoxels * TileSideVoxels];
        ColorsLeft = new byte[TileSideVoxels * TileSideVoxels];
        ColorsBack = new byte[TileSideVoxels * TileSideVoxels];

        for (int y = 0; y < TileSideVoxels; y++) {
            for (int i = 0; i < TileSideVoxels; i++) {
                ColorsRight[y * TileSideVoxels + i] = GetVoxelColor(y, i, VoxelDirection.RIGHT);
                ColorsForward[y * TileSideVoxels + i] = GetVoxelColor(y, i, VoxelDirection.UP);
                ColorsLeft[y * TileSideVoxels + i] = GetVoxelColor(y, i, VoxelDirection.LEFT);
                ColorsBack[y * TileSideVoxels + i] = GetVoxelColor(y, i, VoxelDirection.DOWN);
            }
        }
    }

    public void Rotate90() {
        transform.Rotate(0, 90, 0);

        byte[] colorsRightNew = new byte[TileSideVoxels * TileSideVoxels];
        byte[] colorsForwardNew = new byte[TileSideVoxels * TileSideVoxels];
        byte[] colorsLeftNew = new byte[TileSideVoxels * TileSideVoxels];
        byte[] colorsBackNew = new byte[TileSideVoxels * TileSideVoxels];

        for (int layer = 0; layer < TileSideVoxels; layer++) {
            for (int offset = 0; offset < TileSideVoxels; offset++) {
                colorsRightNew[layer * TileSideVoxels + offset] = ColorsForward[layer * TileSideVoxels + TileSideVoxels - offset - 1];
                colorsForwardNew[layer * TileSideVoxels + offset] = ColorsLeft[layer * TileSideVoxels + offset];
                colorsLeftNew[layer * TileSideVoxels + offset] = ColorsBack[layer * TileSideVoxels + TileSideVoxels - offset - 1];
                colorsBackNew[layer * TileSideVoxels + offset] = ColorsRight[layer * TileSideVoxels + offset];
            }
        }

        ColorsRight = colorsRightNew;
        ColorsForward = colorsForwardNew;
        ColorsLeft = colorsLeftNew;
        ColorsBack = colorsBackNew;
    }

    private byte GetVoxelColor(int verticalLayer, int horizontalOffset, VoxelDirection direction) {
        float vox = VoxelSize;
        float half = VoxelSize / 2;
        // Debug.Log("2meshCollider:" + meshCollider);
        // Debug.Log("2rend:" + rend);

        Vector3 rayStart;
        Vector3 rayDir;
        if (direction == VoxelDirection.RIGHT) {
            rayStart = rend.bounds.min +
                       new Vector3(-half, 0, half + horizontalOffset * vox);
            rayDir = Vector3.right;
        } else if (direction == VoxelDirection.UP) {
            rayStart = rend.bounds.min +
                       new Vector3(half + horizontalOffset * vox, 0, -half);
            rayDir = Vector3.forward;
        } else if (direction == VoxelDirection.LEFT) {
            rayStart = rend.bounds.max +
                       new Vector3(half, 0, -half - (TileSideVoxels - horizontalOffset - 1) * vox);
            rayDir = Vector3.left;
        } else if (direction == VoxelDirection.DOWN) {
            rayStart = rend.bounds.max +
                       new Vector3(-half - (TileSideVoxels - horizontalOffset - 1) * vox, 0, half);
            rayDir = Vector3.back;
        } else {
            throw new ArgumentException("Wrong direction value, should be VoxelDirection.left/right/back/forward",
                nameof(direction));
        }

        rayStart.y = rend.bounds.min.y + half + verticalLayer * vox;

        // Debug.DrawRay(rayStart, direction * 0.1f, Color.blue, 2);

        if (Physics.Raycast(new Ray(rayStart, rayDir), out RaycastHit hit, vox)) {
            byte colorIndex = (byte) (hit.textureCoord.x * 256);

            if (colorIndex == 0) Debug.LogWarning("Found color 0 in mesh palette, this can cause conflicts");

            return colorIndex;
        }

        return 0;
    }
}
