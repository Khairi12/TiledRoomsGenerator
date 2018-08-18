using UnityEngine;
using System.Collections.Generic;

public class CrateManager : MonoBehaviour
{
    public static List<GameObject> crates = new List<GameObject>();
    private GameObject cratePF;

    private void Start ()
    {
        cratePF = Resources.Load("Objects/Crate") as GameObject;

        CreateCrates();
    }

    public void CreateCrates ()
    {
        for (int i = 0; i < RoomManager.tiles.GetLength(0); i++)
        {
            for (int j = 0; j < RoomManager.tiles.GetLength(1); j++)
            {
                Room currentRoom = RoomManager.tiles[i, j].GetComponent<Room>();
                List<KeyValuePair<Transform, int>> crateSpawns = GetCrateSpawns(i, j);

                // new spawner - get list of all room spawners, randomly select one and spawn a crate in that spot. 
                // if position already has crate, spawn on top (max 3 stack)

                while (currentRoom.crateAmount < currentRoom.crateLimit)
                {
                    int randomValue = Random.Range(0, crateSpawns.Count);
                    
                    if (ShouldStack(crateSpawns[randomValue].Value))
                    {
                        Vector3 stackPos = new Vector3(
                            crateSpawns[randomValue].Key.transform.position.x,
                            crateSpawns[randomValue].Key.transform.position.y + (1.4f * crateSpawns[randomValue].Value),
                            crateSpawns[randomValue].Key.transform.position.z);

                        SpawnCrate(stackPos, crateSpawns[randomValue].Key);
                        crateSpawns[randomValue] = new KeyValuePair<Transform, int>(crateSpawns[randomValue].Key, crateSpawns[randomValue].Value + 1);
                        currentRoom.crateAmount += 1;
                    }
                }

                // old spawner
                /*
                foreach (Transform child in RoomManager.tiles[i, j].GetComponentsInChildren<Transform>())
                {
                    if (child.transform.tag == "CrateSpawn" && currentRoom.crateAmount < currentRoom.crateLimit)
                    {
                        if (ShouldStack())
                        {
                            Vector3 stackPos = new Vector3(
                                child.transform.position.x,
                                child.transform.position.y + 1.4f,
                                child.transform.position.z);

                            SpawnCrate(child.transform.position, child.transform);
                            SpawnCrate(stackPos, child.transform);
                            currentRoom.crateAmount += 2;
                        }
                        else
                        {
                            SpawnCrate(child.transform.position, child.transform);
                            currentRoom.crateAmount += 1;
                        }
                    }
                }
                */

            }
        }
    }

    public List<KeyValuePair<Transform, int>> GetCrateSpawns (int i, int j)
    {
        List<KeyValuePair<Transform, int>> crateSpawns = new List<KeyValuePair<Transform, int>>();

        foreach (Transform child in RoomManager.tiles[i, j].GetComponentsInChildren<Transform>())
        {
            if (child.transform.tag == "CrateSpawn")
            {
                crateSpawns.Add(new KeyValuePair<Transform, int>(child.transform, 0));
            }
        }

        return crateSpawns;
    }

    public void SpawnCrate (Vector3 position, Transform parent)
    {
        crates.Add(Instantiate(cratePF, position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), parent));
    }

    public bool ShouldStack (int stackAmount)
    {
        return Random.Range(0, 2) < 1 && stackAmount <= 3 ? true : false;
    }
}
