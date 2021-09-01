using UnityEngine;
using System;
using Rnd = UnityEngine.Random;
using Position = System.Tuple<int, int>;
using System.Collections;
using System.Linq;

public class AdvancedGenerator : MonoBehaviour
{
    private const int SIZE = 100;
    private const int OFFSET = SIZE / 2;

    [SerializeField] private GameObject[] _chunkModels;

    private Pivot[] _pivots;

    private GameObject[,] _map;

    private void Awake()
    {
        _map = new GameObject[SIZE, SIZE];
        GeneratePivots();
        ExpandRegions();
    }

    private void GeneratePivots()
    {
        int amountOfPivots = (int)Math.Floor(_chunkModels.Length * Rnd.Range(1.0f, 2.0f));
        _pivots = new Pivot[amountOfPivots];
        for (int i = 0; i < amountOfPivots; i++)
        {
            int randX = Rnd.Range(-OFFSET, OFFSET);
            int randZ = Rnd.Range(-OFFSET, OFFSET);

            var randomChunk = _chunkModels[Rnd.Range(0, _chunkModels.Length)];
            var position = new Position(randX, randZ);
            var randomGrowthRate = Rnd.Range(1, 4);

            var pivot = new Pivot(randomChunk, position, randomGrowthRate);
            _pivots[i] = pivot; 
            InstantiatePivot(pivot);
        }
    }

    private void ExpandRegions()
    {
        foreach(var pivot in _pivots)
        {
            var (x, z) = pivot.Position;
            var type = pivot.Type;
            var growthRate = pivot.GrowthRate;
            IEnumerator contaminator = Contaminate(type, x, z, growthRate);
            StartCoroutine(contaminator);
        }
    }

    private IEnumerator Contaminate(GameObject type, int x, int z, int growthRate)
    {
        var neighbors = new Position[4];
        neighbors[0] = new Position(x, z + 1);
        neighbors[1] = new Position(x + 1, z);
        neighbors[2] = new Position(x, z - 1);
        neighbors[3] = new Position(x - 1, z);

        foreach((int nextX, int nextZ) in neighbors)
        {
            Collider[] c = Physics.OverlapSphere(new Vector3(nextX, 0, nextZ), 0.5f);
            if (nextX <= 50 && nextX >= -50 && nextZ <= 50 && nextZ >= -50 && !c.Any(o => o.CompareTag("Terrain")))
            {
                InstantiateNext(type, nextX, nextZ);
                // if (Rnd.Range(0, 2) == 1)
                // {
                yield return StartCoroutine(Contaminate(type, nextX, nextZ, growthRate));
                // }
            }
        }

        yield return new WaitForSeconds(0.1f * growthRate);
    }

    private void InstantiatePivot(Pivot pivot)
    {
        var type = pivot.Type;
        var (x, z) = pivot.Position;
        x += OFFSET;
        z += OFFSET;

        _map[x, z] = Instantiate(type, new Vector3(x, 0, z), Quaternion.identity);
    }

    private void InstantiateNext(GameObject type, int x, int z)
    {
        if (x + OFFSET < -49 || x + OFFSET > 49 || z + OFFSET < -49 || z + OFFSET > 49) Debug.Log(x + " - " + z);
        _map[x + OFFSET - 1, z + OFFSET - 1] = Instantiate(type, new Vector3(x, 0, z), Quaternion.identity);
    }
}
