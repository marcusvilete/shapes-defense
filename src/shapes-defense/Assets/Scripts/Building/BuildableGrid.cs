using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableGrid : MonoBehaviour
{
    public float cellSize;
    public int width;
    public int length;
    private BuildableCell[,] cells;

    public BuildableCell cellPrefab;

    private void Awake()
    {
        ResizeCollider();
        SetUpGrid();
    }

    private void SetUpGrid()
    {
        if (cellPrefab != null)
        {
            // Create a container that will hold the cells.
            var cellsParent = new GameObject("Cells");
            cellsParent.transform.parent = transform;
            cellsParent.transform.localPosition = Vector3.zero;
            cellsParent.transform.localRotation = Quaternion.identity;
            cells = new BuildableCell[width, length];

            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 targetPos = GridToWorld(new Vector2Int(x, y), new Vector2Int(1, 1));
                    targetPos.y += 0.01f;
                    BuildableCell newCell = Instantiate(cellPrefab);
                    newCell.transform.parent = cellsParent.transform;
                    newCell.transform.position = targetPos;
                    newCell.transform.localRotation = Quaternion.identity;

                    cells[x, y] = newCell;
                    //newTile.SetState(PlacementTileState.Empty);
                }
            }
        }
    }

    void ResizeCollider()
    {
        var myCollider = GetComponent<BoxCollider>();
        Vector3 size = new Vector3(width, 0, length) * cellSize;
        myCollider.size = size;

        // Collider origin is our bottom-left corner
        myCollider.center = size * 0.5f;
    }

    public Vector2Int WorldToGrid(Vector3 worldLocation, Vector2Int sizeOffset)
    {
        
        Vector3 localLocation = transform.InverseTransformPoint(worldLocation);

        // Scale by inverse grid size
        localLocation *= (1 / cellSize);

        // Offset by half size
        var offset = new Vector3(sizeOffset.x * 0.5f, 0.0f, sizeOffset.y * 0.5f);
        localLocation -= offset;

        int xPos = Mathf.RoundToInt(localLocation.x);
        int yPos = Mathf.RoundToInt(localLocation.z);

        return new Vector2Int(xPos, yPos);
    }

    public Vector3 GridToWorld(Vector2Int gridPosition, Vector2Int sizeOffset)
    {
        // Calculate scaled local position
        Vector3 localPos = new Vector3(gridPosition.x + (sizeOffset.x * 0.5f), 0, gridPosition.y + (sizeOffset.y * 0.5f)) *
                           cellSize;

        return transform.TransformPoint(localPos);
    }

    public Vector3 SnapToGrid(Vector3 worldPosition, Vector2Int sizeOffset)
    {
        return GridToWorld(WorldToGrid(worldPosition, sizeOffset), sizeOffset);
    }

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        float offset = cellSize / 2;

        int xCount = (int)(position.x / cellSize);
        int yCount = (int)(position.y / cellSize);
        int zCount = (int)(position.z / cellSize);

        Vector3 result = new Vector3(
            (float)xCount * cellSize + offset,
            (float)yCount * cellSize,
            (float)zCount * cellSize + offset);

        result += transform.position;

        return result;
    }

    public BuildableCell GetNearestBuildableCell(Vector3 position)
    {
        position -= transform.position;

        float offset = cellSize / 2;

        int xCount = (int)(position.x / cellSize);
        //int yCount = (int)(position.y / cellSize);
        int zCount = (int)(position.z / cellSize);

        return cells[xCount, zCount];
    }


    void OnDrawGizmos()
    {
        Color prevCol = Gizmos.color;
        Gizmos.color = Color.cyan;

        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;

        // Draw local space flattened cubes
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var position = new Vector3((x + 0.5f) * cellSize, 0, (y + 0.5f) * cellSize);
                Gizmos.DrawWireCube(position, new Vector3(cellSize, 0, cellSize));
            }
        }

        Gizmos.matrix = originalMatrix;
        Gizmos.color = prevCol;

        // Draw icon too, in center of position
        Vector3 center = transform.TransformPoint(new Vector3(cellSize * width * 0.5f,
                                                              1,
                                                              cellSize * length * 0.5f));
    }
}
