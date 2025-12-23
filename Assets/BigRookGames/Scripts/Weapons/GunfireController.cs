using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;


namespace BigRookGames.Weapons
{
    public class GunfireController : MonoBehaviour
    {
        // --- Audio ---
        public AudioClip GunShotClip;
        public AudioClip ReloadClip;
        public AudioSource source;
        public AudioSource reloadSource;
        public Vector2 audioPitch = new Vector2(0.9f, 1.1f);

        // --- Muzzle ---
        public GameObject muzzlePrefab;
        public GameObject muzzlePosition;
        public GameObject muzzleTailPrefabPosition;

        // --- Config ---
        public float shotDelay = 0.5f;

        [Header("Ammo")]
        public int currentAmmo = 0;

        // --- Projectile ---
        [Tooltip("The projectile gameobject to instantiate each time the weapon is fired.")]
        public GameObject projectilePrefab;

        [Tooltip("Sometimes a mesh will want to be disabled on fire. For example: when a rocket is fired, we instantiate a new rocket, and disable the visible rocket attached to the rocket launcher.")]
        public GameObject projectileToDisableOnFire;

        // --- Timing ---
        [SerializeField] private float timeLastFired;

        // --- XR Input ---
        private InputDevice rightController;
        private bool triggerPressedLastFrame = false;


        private void Start()
        {
            if (source != null)
                source.clip = GunShotClip;

            timeLastFired = 0;

            // Get right-hand controller
            var rightHandDevices = new System.Collections.Generic.List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);

            if (rightHandDevices.Count > 0)
                rightController = rightHandDevices[0];


        }


        private void Update()
        {
            // --- Check for trigger press on Meta Quest controller ---
            if (rightController.isValid)
            {
                if (rightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
                {
                    if (triggerPressed && !triggerPressedLastFrame && (Time.time >= timeLastFired + shotDelay))
                    {
                        FireWeapon();
                    }

                    triggerPressedLastFrame = triggerPressed;
                }
            }

            // --- Keyboard test key ---
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FireWeapon();
            }

            if (currentAmmo <= 0)
            {
                Debug.Log("Out of ammo");
                EndGame();
                return;
            }

        }

        public void SetAmmo(int amount)
        {
            currentAmmo = amount;

            if (projectileToDisableOnFire != null)
                projectileToDisableOnFire.SetActive(currentAmmo > 0);


        }



        public void FireWeapon()
        {
            

            currentAmmo--;
            Debug.Log("Ammo left: " + currentAmmo);



            timeLastFired = Time.time;

            // --- Spawn muzzle flash ---
            if (muzzlePrefab && muzzleTailPrefabPosition)
                Instantiate(muzzlePrefab, muzzleTailPrefabPosition.transform);

            // --- Shoot projectile ---
            if (projectilePrefab && muzzlePosition)
                Instantiate(projectilePrefab, muzzlePosition.transform.position, muzzlePosition.transform.rotation);

            // --- Disable any attached projectile ---
            if (projectileToDisableOnFire)
            {
                projectileToDisableOnFire.SetActive(false);
                Invoke(nameof(ReEnableDisabledProjectile), 3);
            }

            // --- Strong controller vibration (Quest friendly) ---
            if (rightController.isValid)
            {
                rightController.SendHapticImpulse(
                    0u,     // channel
                    1.0f,   // MAX intensity (0–1)
                    0.25f   // longer duration
                );
            }

            // --- Handle Audio ---
            if (source != null)
            {
                if (source.transform.IsChildOf(transform))
                {
                    source.Play();
                }
                else
                {
                    AudioSource newAS = Instantiate(source);
                    if (newAS != null)
                    {
                        newAS.pitch = Random.Range(audioPitch.x, audioPitch.y);
                        newAS.PlayOneShot(GunShotClip);
                        Destroy(newAS.gameObject, 4);
                    }
                }
            }
        }

        private void UpdateProjectileVisual()
        {
            if (projectileToDisableOnFire != null)
            {
                projectileToDisableOnFire.SetActive(currentAmmo > 0);
            }
        }

        public void ReplenishAmmo(int amount)
        {
            currentAmmo = amount;
            Debug.Log("Ammo replenished: " + currentAmmo);

            UpdateProjectileVisual();
        }


        void EndGame()
        {
            Debug.Log("GAME OVER: Base Destroyed");

            // Load Menu scene
            SceneManager.LoadScene("Menu 3D");


        }

        private void ReEnableDisabledProjectile()
        {
            if (currentAmmo <= 0)
                return;

            if (reloadSource != null)
                reloadSource.Play();

            if (projectileToDisableOnFire != null)
                projectileToDisableOnFire.SetActive(true);
        }

    }
}
