using UnityEngine;

namespace TheBrokenNexus.Actors.Player.Projectile
{
    public class SimpleProjectile : MonoBehaviour
    {
        Vector3 moveDirection;
        float speed;
        Transform targetEnemy;

        public float lifeTime = 5f;

        void Start()
        {
            Destroy(gameObject, lifeTime);

            GameObject enemyObj = FindClosestEnemy();

            if (enemyObj != null)
            {
                targetEnemy = enemyObj.transform;
            }
        }

        void Update()
        {
            if (targetEnemy != null)
            {
                Vector3 dir = (targetEnemy.position - transform.position).normalized;
                dir.y = 0f;
                moveDirection = dir;

                if (moveDirection != Vector3.zero)
                {
                    Vector3 lookDir = moveDirection;
                    lookDir.y = 0f;
                    transform.rotation = Quaternion.LookRotation(lookDir);
                }
            }

            Vector3 newPos = transform.position + moveDirection * speed * Time.deltaTime;
            newPos.y = 0f;
            transform.position = newPos;
        }

        public void SetDirection(Vector3 direction, float projectileSpeed)
        {
            moveDirection = direction.normalized;
            speed = projectileSpeed;

            if (moveDirection != Vector3.zero)
            {
                Vector3 lookDir = moveDirection;
                lookDir.y = 0f;
                transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }

        GameObject FindClosestEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject closest = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;

            foreach (GameObject enemy in enemies)
            {
                float dist = Vector3.Distance(currentPos, enemy.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = enemy;
                }
            }

            return closest;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }
}