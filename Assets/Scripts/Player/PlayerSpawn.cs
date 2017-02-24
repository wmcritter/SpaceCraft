using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        Vector3 spawn = new Vector3(
            UnityEngine.Random.Range(-TerrainManager.ColdBiomeMin / 2, TerrainManager.ColdBiomeMin/2),
            UnityEngine.Random.Range(-TerrainManager.ColdBiomeMin / 2, TerrainManager.ColdBiomeMin/2),
            UnityEngine.Random.Range(-TerrainManager.ColdBiomeMin / 2, TerrainManager.ColdBiomeMin/2)
            );

        int count = 0;
        while (!IsSpawnPointValid(spawn))
        {
            spawn = new Vector3(
            UnityEngine.Random.Range(-TerrainManager.ColdBiomeMin / 2, TerrainManager.ColdBiomeMin/2),
            UnityEngine.Random.Range(-TerrainManager.ColdBiomeMin / 2, TerrainManager.ColdBiomeMin/2),
            UnityEngine.Random.Range(-TerrainManager.ColdBiomeMin / 2, TerrainManager.ColdBiomeMin/2)
            );
            count++;
            if (count > 10)
            {
                Debug.Log("unable to find valid spawn in 5 tries");
                break;
            }
        }

        transform.position = spawn;
	}

    bool IsSpawnPointValid(Vector3 position)
    {
        //see if point is in forbidden zone (too close to sun)
        if ((position.x > -TerrainManager.NormalBiomeMin && position.x < TerrainManager.NormalBiomeMin) ||
            (position.y > -TerrainManager.NormalBiomeMin && position.y < TerrainManager.NormalBiomeMin) ||
            (position.z > -TerrainManager.NormalBiomeMin && position.z < TerrainManager.NormalBiomeMin))
            return false;

        //check 1 block in all directions around starting position
            for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                for (int z = -1; z < 2; z++)
                {
                    //if any spot is not empty, return false
                    if (Chunk.GetTheoreticalBrick(position + new Vector3(x, y, z)) != BlockType.None)
                    {
                        Debug.Log("invalid spawn point " + position);
                        return false;
                    }
                }
            }
        }

        return true;
    }
}
