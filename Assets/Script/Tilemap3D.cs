using UnityEngine;
using UnityEngine.Tilemaps;

public class Tilemap3D : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject tilePrefab;

    void Start()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (tilemap.HasTile(localPlace))
            {
                Vector3 place = tilemap.CellToWorld(localPlace);
                Instantiate(tilePrefab, place, Quaternion.identity);
            }
        }
    }
}
