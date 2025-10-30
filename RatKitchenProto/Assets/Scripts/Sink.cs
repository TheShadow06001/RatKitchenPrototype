using System.Collections.Generic;
using UnityEngine;

public class Sink : KitchenElements
{
    public GameObject plate;
    private Vector3 sinkPosition;
    private float sinkHeigth = 0.9f;

    private void OnEnable()
    {
        GameObject plateOnScene = Instantiate(plate, CountPosition(-0.5f, sinkHeigth, -0.5f), Quaternion.Euler(-90, 0, 0));
        plateOnScene.transform.SetParent(transform);
        

    }
    private Vector3 CountPosition(float x, float y, float z)
    {
        sinkPosition = transform.position;

        float CELL_WIDTH = x / 2;
        float CELL_HEIGHT = z / 2;

        float CENTER_OF_CELL_X = CELL_WIDTH / 2;
        float CENTER_OF_CELL_Z = CELL_WIDTH / 2;

        float randX = Random.Range(0, 2);
        float randY = Random.Range(0, 2);

        float finalX = randX * CELL_WIDTH + CENTER_OF_CELL_X;
        float finalZ = randY * CELL_HEIGHT + CENTER_OF_CELL_Z;
        Vector3 spawnPosition = new Vector3(finalX, y, finalZ) + sinkPosition + new Vector3(0, 0, 1f);

        return spawnPosition;
    }
}
