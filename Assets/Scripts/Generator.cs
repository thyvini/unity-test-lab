using System;
using UnityEngine;

public class Generator : MonoBehaviour
{

    private const int WIDTH = 100;
    private const int HEIGHT = 100;
    

    [SerializeField] private GameObject[] _chunkModels;

    private void Awake()
    {
        for (int z = -(HEIGHT / 2); z < HEIGHT / 2; z++)
        {
            for (int x = -(WIDTH / 2); x < WIDTH / 2; x++)
            {
                int xWithoutOffset = x + WIDTH / 2;
                int zWithoutOffset = z + HEIGHT / 2;
                int r = GetFromNoise(xWithoutOffset, zWithoutOffset);
                Instantiate(_chunkModels[r], new Vector3(x, 0, z), Quaternion.identity);
            }
        }
    }

    private int GetFromNoise(float x, float z) {
        float xCoord = x * 10 / WIDTH;
        float zCoord = z * 10 / HEIGHT;

        float noiseFromPosition = Mathf.PerlinNoise(xCoord, zCoord);

        float remaped = Remap(noiseFromPosition, 0.0f, 1.0f, 0.0f, (float)_chunkModels.Length);
        
        Debug.Log(remaped);

        return (int)Math.Floor(remaped);
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        Debug.Log(value);
        Debug.Log(from1);
        Debug.Log(to1);
        Debug.Log(from2);
        Debug.Log(to2);
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
