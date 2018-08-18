using UnityEngine;

public enum RoomType { Corner, Border, Fourway, Hallway, Double, Detour, Deadend, Storage };

public class Room : MonoBehaviour
{
    public RoomType roomType;
    
    /*
    public Pillar nePillar;
    public Pillar sePillar;
    public Pillar swPillar;
    public Pillar nwPillar;
    */

    public Room nRoom;
    public Room eRoom;
    public Room sRoom;
    public Room wRoom;

    public string category;
    public int xCoordinate;
    public int yCoordinate;
    public int crateLimit;
    public int crateAmount;

    public void SetRoomType (RoomType rType)
    {
        roomType = rType;
    }

    public void SetNeighborRooms (Room nr, Room er, Room sr, Room wr)
    {
        nRoom = nr;
        eRoom = er;
        sRoom = sr;
        wRoom = wr;
    }

    /*
    public void SetNeighborPillars (Pillar nep, Pillar sep, Pillar swp, Pillar nwp)
    {
        nePillar = nep;
        sePillar = sep;
        swPillar = swp;
        nwPillar = nwp;
    }
    */

    public void SetCrateAmount (int limit)
    {
        crateLimit = limit;
    }

    public void SetCoordinates (int x, int y)
    {
        xCoordinate = x;
        yCoordinate = y;
    }

    public void UpdatePillarWeaken () { }

    public void UpdatePillarStrength () { }
}
