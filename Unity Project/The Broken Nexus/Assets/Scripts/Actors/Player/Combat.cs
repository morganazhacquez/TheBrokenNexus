using TheBrokenNexus.Actors.Player;
using TheBrokenNexus.Actors.Player.Projectile;
using UnityEngine;

namespace TheBrokenNexus.Actors.Player
{
    public class Combat : MonoBehaviour
    {
        [Header("Projectile")]
        public GameObject projectilePrefab;
        public float projectileSpeed = 10f;
        public float fireRate = 0.3f;

        Movement Movement;
        Stats Stats;

        float fireTimer = 0f;

        void Start()
        {
            Stats = GetComponent<Stats>();
            Movement = GetComponent<Movement>();
        }

        void Update()
        {
            fireTimer -= Time.deltaTime;

            if (Input.GetMouseButton(0) && fireTimer <= 0f)
            {
                if (Stats != null && Stats.SpendMana(1))
                {
                    ShootProjectile();

                    if (Movement != null)
                    {
                        Movement.ResetIdle();
                    }

                    fireTimer = fireRate;
                }
                else
                {
                    Debug.Log("Not enough mana!");
                }
            }
        }

        void ShootProjectile()
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 targetPos = ray.GetPoint(distance);

                Vector3 direction = (targetPos - transform.position);
                direction.y = 0f;
                direction.Normalize();

                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

                SimpleProjectile sp = projectile.GetComponent<SimpleProjectile>();

                if (sp != null)
                {
                    sp.SetDirection(direction, projectileSpeed);
                }
            }
        }
    }
}