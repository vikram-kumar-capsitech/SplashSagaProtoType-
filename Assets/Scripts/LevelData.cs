using UnityEngine;

[System.Serializable]
public class SpawnObject
{
    public string prefab;
    public float x;
    public float y;
    public float z;
    public float rx;
    public float ry;
    public float rz;
}

[System.Serializable]
public class LevelData
{
    public int levelNumber;
    public bool levelLock;
    public bool levelComplete;
    public SpawnObject[] objects;
}

public static class LevelsData
{
    public static LevelData[] levels =
    {
        new LevelData
        {
            levelNumber = 1,
            levelLock = true,
            levelComplete = false,
            objects = new SpawnObject[]
            {
                new SpawnObject { prefab = "Sponge", x = 0, y = 3.6f, z = 0, rx = 0, ry = 0, rz = 0},
                new SpawnObject { prefab = "Square", x = 0.1f, y = 1.8f, z = 0, rx = 0, ry = 0, rz = 0 },
                new SpawnObject { prefab = "Basin", x = 0f, y = -6.5f, z = 0, rx = 0, ry = 0, rz = 0 }
            }
        },

        new LevelData
        {
            levelNumber = 2,
            levelLock = false,
            levelComplete = false,
            objects = new SpawnObject[]
            {
                new SpawnObject { prefab = "Sponge", x = 0, y = 0.9f, z = 0, rx = 0, ry = 0, rz = 0 },
                new SpawnObject { prefab = "V-Shape", x = 0.11f  , y = 2.36f, z = 0 , rx = 0, ry = 0, rz = -134.38f},
                new SpawnObject { prefab = "Basin", x = 0f, y = -6.5f, z = 0, rx = 0, ry = 0, rz = 0 }
            }
        },

        new LevelData
        {
            levelNumber = 3,
            levelLock = false,
            levelComplete = false,
            objects = new SpawnObject[]
            {
                new SpawnObject { prefab = "Sponge", x = 0, y = 3.6f, z = 0, rx = 0, ry = 0, rz = 0 },
                new SpawnObject { prefab = "TriangleTile", x = -0.2f, y = 1.35f, z = 0, rx = 0, ry = 0, rz = 0 },
                new SpawnObject { prefab = "Tile", x = -1.69f, y = -0.82f, z = 0 , rx = 0, ry = 0, rz = 0},
                new SpawnObject { prefab = "Tile", x = 1.56f, y = -0.82f, z = 0, rx = 0, ry = 0, rz = 0 },
                new SpawnObject { prefab = "Basin", x = 0f, y = -6.5f, z = 0, rx = 0, ry = 0, rz = 0 }
            }
        },

        new LevelData
        {
            levelNumber = 4,
            levelLock = false,
            levelComplete = false,
            objects = new SpawnObject[]
            {
                new SpawnObject { prefab = "Sponge", x = 0, y = 2.5f, z = 0, rx = 0, ry = 0, rz = 0 },
                new SpawnObject { prefab = "L-Shape", x = 0f, y =1.15f, z = 0, rx = 0, ry = 0, rz = 0},
                new SpawnObject { prefab = "L-Shape", x = 0f, y = 3f, z = 0, rx = 0, ry = 0, rz = 180 },
                new SpawnObject { prefab = "Basin", x = 0f, y = -6.5f, z = 0 , rx = 0, ry = 0, rz = 0}
            }
        },

        new LevelData
        {
            levelNumber = 5,
            levelLock = false,
            levelComplete = false,
            objects = new SpawnObject[]
            {
                new SpawnObject { prefab = "Sponge", x = 0, y = 3.5f, z = 0, rx = 0, ry = 0, rz = 0 },
                new SpawnObject { prefab = "Vertical", x = -3f, y = 3.5f, z = 0, rx = 0, ry = 0, rz = 0 },
                new SpawnObject { prefab = "Vertical", x = 3.17f, y = 3.5f ,z = 0, rx = 0, ry = 0, rz = 00 },
                new SpawnObject { prefab = "Square", x = 0f, y = 1.5f, z = 0, rx = 0, ry = 0, rz = 0},
                new SpawnObject { prefab = "Basin1", x = 0f, y = -5.47f, z = 0, rx = 0, ry = 0, rz = 0}
            }
        }
    };
}
