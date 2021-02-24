using System;
using UnityEngine;

public class Generator : MonoBehaviour
{

    private const int WIDTH = 100;
    private const int HEIGHT = 100;

    private GameObject[][] _map;

    [SerializeField] private GameObject[] _chunkModels;

    private void Awake()
    {
        var time = Time.deltaTime;
        _map = new GameObject[WIDTH][];
        for (int i = 0; i < WIDTH; i++)
        {
            _map[i] = new GameObject[HEIGHT];
        }

        for (int z = -(HEIGHT / 2); z < HEIGHT / 2; z++)
        {
            int zWithoutOffset = z + HEIGHT / 2;
            for (int x = -(WIDTH / 2); x < WIDTH / 2; x++)
            {
                int xWithoutOffset = x + WIDTH / 2;
                int r = GetFromNoise(xWithoutOffset, zWithoutOffset);
                _map[xWithoutOffset][zWithoutOffset] = Instantiate(_chunkModels[r], new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        var concluded = Time.deltaTime;
        Debug.Log(concluded - time);
    }

    private int GetFromNoise(float x, float z)
    {
        float xCoord = x * 10 / WIDTH;
        float zCoord = z * 10 / HEIGHT;

        float noiseFromPosition = Mathf.PerlinNoise(xCoord, zCoord);

        float remaped = Remap(noiseFromPosition, 0.0f, 1.0f, 0.0f, (float)_chunkModels.Length);

        return (int)Math.Floor(remaped);
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
