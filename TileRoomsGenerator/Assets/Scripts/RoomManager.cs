using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static GameObject[,] tiles = new GameObject[10, 10];

    private GameObject borderRoomPF;
    private GameObject cornerRoomPF;
    private GameObject fourwayRoomPF;
    private GameObject hallwayRoomPF;
    private GameObject deadendRoomPF;
    private GameObject detourRoomPF;
    private GameObject doubleRoomPF;
    private GameObject storageRoomPF;

    private void Start ()
    {
        borderRoomPF = Resources.Load("Structures/Border Room") as GameObject;
        cornerRoomPF = Resources.Load("Structures/Corner Room") as GameObject;
        deadendRoomPF = Resources.Load("Structures/Deadend Room") as GameObject;
        detourRoomPF = Resources.Load("Structures/Detour Room") as GameObject;
        doubleRoomPF = Resources.Load("Structures/Double Room") as GameObject;
        fourwayRoomPF = Resources.Load("Structures/Fourway Room") as GameObject;
        hallwayRoomPF = Resources.Load("Structures/Hallway Room") as GameObject;
        storageRoomPF = Resources.Load("Structures/Storage Room") as GameObject;

        SpawnRooms();
    }

    public void SpawnRooms ()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                GameObject prefab = DetermineRoomStructure(i, j);
                int tileRotation = DetermineTileRotation(i, j);

                tiles[i, j] = Instantiate(prefab, 
                    new Vector3(i * 10, 0, j * 10), 
                    Quaternion.Euler(new Vector3(0, tileRotation, 0)),
                    transform);
            }
        }

        AssignNeighbors();
        ReplaceNeighbors();
    }

    public void AssignNeighbors ()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                Room tileRoom = tiles[i, j].GetComponent<Room>();

                if (IsBorderTile(i, j) || IsCornerTile(i, j))
                    continue;

                tileRoom.SetNeighborRooms(
                    tiles[i - 1, j].GetComponent<Room>(), 
                    tiles[i, j + 1].GetComponent<Room>(), 
                    tiles[i + 1, j].GetComponent<Room>(), 
                    tiles[i, j - 1].GetComponent<Room>());
            }
        }
    }

    public void ReplaceNeighbors ()
    {

    }

    public GameObject DetermineRoomStructure (int i, int j)
    {
        if (IsBorderTile(i, j))
        {
            return borderRoomPF;
        }
        else if (IsCornerTile(i, j))
        {
            return cornerRoomPF;
        }
        else
        {
            int value = Random.Range(0, System.Enum.GetValues(typeof(RoomType)).Length - 2);

            switch (value)
            {
                case 0:
                    return deadendRoomPF;
                case 1:
                    return detourRoomPF;
                case 2:
                    return doubleRoomPF;
                case 3:
                    return fourwayRoomPF;
                case 4:
                    return hallwayRoomPF;
                case 5:
                    return storageRoomPF;
                default:
                    return fourwayRoomPF;
            }
        }
    }

    public string DetermineParent (int i, int j)
    {
        switch (tiles[i, j].GetComponent<Room>().roomType)
        {
            case RoomType.Border:
                return "Border Rooms";
            case RoomType.Corner:
                return "Corner Rooms";
            case RoomType.Deadend:
                return "Deadend Rooms";
            case RoomType.Detour:
                return "Detour Rooms";
            case RoomType.Double:
                return "Double Rooms";
            case RoomType.Fourway:
                return "Fourway Rooms";
            case RoomType.Hallway:
                return "Hallway Rooms";
            case RoomType.Storage:
                return "Storage Rooms";
            default:
                return "Fourway Rooms";
        }
    }

    public int DetermineTileRotation (int i, int j)
    {
        if (IsNorthBorderTile(i, j) || IsNorthWestCornerTile(i, j))
        {
            return 180;
        }
        else if (IsEastBorderTile(i, j) || IsNorthEastCornerTile(i, j))
        {
            return 270;
        }
        else if (IsSouthBorderTile(i, j) || IsSouthEastCornerTile(i, j))
        {
            return 0;
        }
        else if (IsWestBorderTile(i, j) || IsSouthWestCornerTile(i, j))
        {
            return 90;
        }

        return 0;
    }

    public bool IsNorthBorderTile (int i, int j)
    {
        return i == 0 && j > 0 && j < tiles.GetLength(1) - 1 ?
               true : false;
    }

    public bool IsSouthBorderTile (int i, int j)
    {
        return i == tiles.GetLength(0) - 1 && j > 0 && j < tiles.GetLength(1) - 1 ? 
               true : false;
    }

    public bool IsEastBorderTile (int i, int j)
    {
        return j == tiles.GetLength(1) - 1 && i > 0 && i < tiles.GetLength(0) - 1 ? 
               true : false;
    }

    public bool IsWestBorderTile (int i, int j)
    {
        return j == 0 && i > 0 && i < tiles.GetLength(0) - 1 ? 
               true : false;
    }

    public bool IsNorthWestCornerTile (int i, int j)
    {
        return i == 0 && j == 0 ? 
               true : false;
    }

    public bool IsNorthEastCornerTile (int i, int j)
    {
        return i == 0 && j == tiles.GetLength(0) - 1 ? 
               true : false;
    }

    public bool IsSouthWestCornerTile (int i, int j)
    {
        return i == tiles.GetLength(0) - 1 && j == 0 ? 
               true : false;
    }

    public bool IsSouthEastCornerTile (int i, int j)
    {
        return i == tiles.GetLength(0) - 1 && j == tiles.GetLength(1) - 1 ? 
               true : false;
    }

    public bool IsBorderTile (int i, int j)
    {
        return IsNorthBorderTile(i, j) || IsEastBorderTile(i, j) ||
               IsSouthBorderTile(i, j) || IsWestBorderTile(i, j) ? 
               true : false;
    }

    public bool IsCornerTile (int i, int j)
    {
        return i == 0 && j == 0 ||
               i == 0 && j == tiles.GetLength(0) - 1 ||
               i == tiles.GetLength(0) - 1 && j == 0 ||
               i == tiles.GetLength(0) - 1 && j == tiles.GetLength(1) - 1 ? 
               true : false;
    }
}
