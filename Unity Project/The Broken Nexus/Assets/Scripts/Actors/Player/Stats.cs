using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheBrokenNexus.Actors.Player
{
    public class Stats : MonoBehaviour
    {
        [SerializeField] Image ManaFill;
        [SerializeField] Image RAMFill;
        [SerializeField] TextMeshProUGUI RAMCountText;

        public int SkillPointsToUse;

        public int ShotPowerLevel = 1;
        public int ShotDamageLevel = 1;
        public int ShotSpeedLevel = 1;
        public int ShotCritialChanceLevel = 1;

        public int CurrentMana;
        public int MaxMana;
        public float ManaRegen;
        public int RegenPerPeriod;

        public int RAMLevel = 1;
        public int CurrentRAM;
        public int RAMTarget;

        int InitialRamTarget = 100;

        float regenTimer;

        void Start()
        {
            CurrentRAM = 0;
            UpdateRAMTarget();
            UpdateRAMUI();
        }

        void Update()
        {
            RegenerateMana();
            UpdateManaUI();
        }

        void RegenerateMana()
        {
            if (CurrentMana >= MaxMana) return;

            regenTimer += Time.deltaTime;

            if (regenTimer >= ManaRegen)
            {
                CurrentMana += RegenPerPeriod;
                CurrentMana = Mathf.Min(CurrentMana, MaxMana);
                regenTimer = 0f;
            }
        }

        public bool SpendMana(int amount)
        {
            if (CurrentMana >= amount)
            {
                CurrentMana -= amount;
                UpdateManaUI();
                return true;
            }
            return false;
        }

        void UpdateManaUI()
        {
            if (ManaFill != null && MaxMana > 0)
            {
                ManaFill.fillAmount = (float)CurrentMana / MaxMana;
            }
        }

        void UpdateRAMTarget()
        {
            RAMTarget = Mathf.CeilToInt(InitialRamTarget * RAMLevel * (Mathf.PI / 2f + 0.5f));
        }

        public void AddRAM(int amount)
        {
            CurrentRAM += amount;

            while (CurrentRAM >= RAMTarget)
            {
                CurrentRAM -= RAMTarget;
                RAMLevel++;
                UpdateRAMTarget();
                Debug.Log($"RAM Level Up! Novo nível: {RAMLevel}");
            }

            UpdateRAMUI();
        }

        void UpdateRAMUI()
        {
            if (RAMFill != null && RAMTarget > 0)
            {
                RAMFill.fillAmount = (float)CurrentRAM / RAMTarget;
            }

            if (RAMCountText != null)
            {
                string currentFormatted = CurrentRAM.ToString("N0").Replace(",", ".");
                string targetFormatted = RAMTarget.ToString("N0").Replace(",", ".");
                RAMCountText.text = $"{currentFormatted} / {targetFormatted} | lv. {RAMLevel}";
            }
        }
    }
}