using UnityEngine;

public class Oven : KitchenElements
{
    public GameObject fryingPan;
    public GameObject bigPan;
    float ovenHeigth;

    private void Awake()
    {
        Generate();
    }

    private Vector3 CoutPosition(int x, int z)
    {
        int CELL_WIDTH = x / 2;
        int CELL_HEIGHT = z / 2;

        int CENTER_OF_CELL_X = CELL_WIDTH / 2;
        int CENTER_OF_CELL_Z = CELL_WIDTH / 2;

        int randX = Random.Range(0, 2);
        int randY = Random.Range(0, 2);

        int finalX = randX * CELL_WIDTH + CENTER_OF_CELL_X;
        int finalZ = randY * CELL_HEIGHT + CENTER_OF_CELL_Z;

        return new Vector3(finalX, 0, finalZ);
    }
    private void Generate()
    {
        GameObject prefabToSpawn = null;
        int randomType = Random.Range(0, 2);
        switch (randomType)
        {
            case 0:
                prefabToSpawn = fryingPan;
                break;
            case 1:
                prefabToSpawn = bigPan;
                break;
        }

        if (prefabToSpawn == null)
        {
            GameObject obstacle = Instantiate(prefabToSpawn, CoutPosition(8,7), Quaternion.identity);
        }
    }
}
