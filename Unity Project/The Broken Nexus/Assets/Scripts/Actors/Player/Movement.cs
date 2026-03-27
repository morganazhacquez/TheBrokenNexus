using UnityEngine;

namespace TheBrokenNexus.Actors.Player
{
    public class Movement : MonoBehaviour
    {
        [Header("Movimento")]
        public float speed = 6f;
        public float rotationSpeed = 10f;

        [Header("Flutuação")]
        public float floatAmplitude = 0.1f;
        public float floatFrequency = 2f;

        [Header("Idle Look Around")]
        public float idleLookAngle = 30f;
        public float idleLookSpeed = 2f;
        public float idleDelay = 5f;

        [Header("Sonic 360 Vertical + Forward")]
        public float sonicHeight = 3f;
        public float sonicForwardDistance = 2f;
        public float sonicDuration = 2f;

        float initialY;
        float idleTimer = 0f;

        Vector3 sonicStartPos;
        Vector3 sonicForwardDir;
        bool isSonic;
        float sonicTimer;
        Quaternion sonicStartRot;

        bool isShooting;

        void Start()
        {
            initialY = transform.position.y;
        }

        void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 input = new Vector3(horizontal, 0f, vertical);

            if (input.magnitude > 0.01f)
            {
                ResetIdle();
                input.Normalize();
                Vector3 direction = Quaternion.Euler(0, 320f, 0) * input;

                transform.position += direction * speed * Time.deltaTime;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                ApplyFloating();
            }
            else
            {
                idleTimer += Time.deltaTime;

                if (!isShooting && idleTimer >= idleDelay)
                {
                    if (!isSonic)
                    {
                        isSonic = true;
                        sonicStartPos = transform.position;
                        sonicForwardDir = transform.forward;
                        sonicTimer = 0f;
                        sonicStartRot = transform.rotation;
                    }

                    if (isSonic)
                    {
                        DoSonic360Forward();
                    }
                    else
                    {
                        float angle = Mathf.Sin(Time.time * idleLookSpeed) * idleLookAngle;
                        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                        ApplyFloating();
                    }
                }
                else
                {
                    ApplyFloating();
                }
            }

            isShooting = false;
        }

        void ApplyFloating()
        {
            if (isSonic)
            {
                return;
            }

            float floatOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            Vector3 pos = transform.position;
            pos.y = initialY + floatOffset;
            transform.position = pos;
        }

        void DoSonic360Forward()
        {
            sonicTimer += Time.deltaTime;
            float t = Mathf.Clamp01(sonicTimer / sonicDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            Vector3 pos = sonicStartPos;
            pos += sonicForwardDir * Mathf.Sin(smoothT * Mathf.PI) * sonicForwardDistance;
            pos.y = initialY + Mathf.Sin(smoothT * Mathf.PI) * sonicHeight;
            transform.position = pos;

            float rotationAngle = smoothT * 360f;
            Quaternion targetRot = Quaternion.Euler(rotationAngle, sonicStartRot.eulerAngles.y, sonicStartRot.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            if (sonicTimer >= sonicDuration)
            {
                transform.position = sonicStartPos;
                transform.rotation = sonicStartRot;
                isSonic = false;
                idleTimer = 0f;
            }
        }

        public void ResetIdle()
        {
            idleTimer = 0f;
            isSonic = false;
            sonicTimer = 0f;
            isShooting = true;
        }
    }
}