using System.Collections.Generic;
using UnityEngine;

namespace TheBrokenNexus.Actors.Enimies
{
    public class Spawner : MonoBehaviour
    {
        public List<GameObject> Enemies;
        public int spawnCount = 5;
        public float spawnRadius = 2f;
        public Transform spawnParent;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnEnemies();
            }
        }

        void SpawnEnemies()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                if (Enemies.Count == 0)
                {
                    return;
                }

                GameObject enemyPrefab = Enemies[Random.Range(0, Enemies.Count)];

                Vector3 randomOffset = new Vector3(Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius));

                Vector3 spawnPos = transform.position + randomOffset;

                Instantiate(enemyPrefab, spawnPos, Quaternion.identity, spawnParent);
            }
        }
    }
}