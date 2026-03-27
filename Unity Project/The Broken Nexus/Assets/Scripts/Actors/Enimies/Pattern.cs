using TheBrokenNexus.Actors.Player;
using UnityEngine;
using UnityEngine.UI;

namespace TheBrokenNexus.Actors.Enimies
{
    public class Pattern : MonoBehaviour
    {
        public Data Data;

        float floatAmplitude = 0.1f;
        float floatFrequency = 2f;

        Vector3 startPosition;

        [SerializeField] Image Life;
        [SerializeField] GameObject deathCubePrefab;
        [SerializeField] int cubesCount = 10;
        [SerializeField] float explosionForce = 10f;
        [SerializeField] float cubeLifetime = 2f;

        Stats PlayerStats;

        int currentLife;
        int startLife;

        void Start()
        {
            PlayerStats = GameObject.Find("Player").GetComponent<Stats>();

            startPosition = transform.position;

            if (Data != null)
            {
                currentLife = Data.BaseLife;
                startLife = Data.BaseLife;
                UpdateLifeBar();
            }
        }

        void Update()
        {
            FloatAnimation();
        }

        void FloatAnimation()
        {
            Vector3 newPos = startPosition;
            newPos.y += Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            transform.position = newPos;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerProjectile"))
            {
                TakeDamage(1);
            }
        }

        void TakeDamage(int amount)
        {
            currentLife -= amount;
            UpdateLifeBar();

            if (currentLife <= 0)
            {
                Die();
            }
        }

        void UpdateLifeBar()
        {
            if (Life != null)
            {
                Life.fillAmount = (float)currentLife / startLife;
            }
        }

        void Die()
        {
            for (int i = 0; i < cubesCount; i++)
            {
                GameObject cube = Instantiate(deathCubePrefab, transform.position, Random.rotation);

                Rigidbody rb = cube.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = cube.AddComponent<Rigidbody>();
                }

                rb.isKinematic = false;

                Vector3 randomDir = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(0.5f, 1.5f),
                    Random.Range(-1f, 1f)
                ).normalized;

                rb.AddForce(randomDir * explosionForce, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * explosionForce, ForceMode.Impulse);

                Destroy(cube, cubeLifetime);
            }

            if (PlayerStats != null)
            {
                int ramGain = Random.Range(25, 50);
                PlayerStats.AddRAM(ramGain);
                Debug.Log($"Player ganhou {ramGain} de RAM!");
            }

            Destroy(gameObject);
        }
    }
}