using UnityEngine;

namespace BigRookGames.Weapons
{
    /// <summary>
    /// RPG-7 rocket flight using RK4 integration (no Rigidbody).
    /// Models: booster launch, short motor burn, drag, gravity, wind, and dispersion.
    /// Also does continuous collision (sphere cast) to avoid tunneling at high speed.
    /// </summary>
    public class ProjectileControllerRK4 : MonoBehaviour
    {
        [Header("Initial Conditions")]
        [Tooltip("Muzzle velocity at tube exit (m/s). RPG-7 ≈ 115–120 m/s; 117 m/s is common.")]
        public float initialSpeed = 117f;

        [Tooltip("Optional sustain max speed (m/s) during short burn. RPG-7 ~280–300 m/s depending on round.")]
        public float motorTargetSpeed = 295f;

        [Tooltip("Time the motor provides thrust after ~10m (s). ~0.15–0.20s typical.")]
        public float motorBurnTime = 0.18f;

        [Tooltip("Distance after which the motor lights (m). RPG-7 motor ignites ≈ 8–12m out of tube.")]
        public float motorIgniteDistance = 10f;

        [Header("Aerodynamics")]
        [Tooltip("Rocket mass in kg (PG-7V ~2.0kg warhead; total ~2.2–2.3kg).")]
        public float mass = 2.2f;

        [Tooltip("Drag coefficient (dimensionless). Slender finned body ~0.4–0.8; tune by feel.")]
        [Range(0.2f, 1.2f)] public float dragCoefficient = 0.6f;

        [Tooltip("Reference area (m^2). Cross-section πr^2; for ~85mm dia, A≈0.0057.")]
        public float referenceArea = 0.0057f;

        [Tooltip("Air density (kg/m^3). Sea level ~1.225.")]
        public float airDensity = 1.225f;

        [Header("Environment")]
        public Vector3 gravity = new Vector3(0f, -9.81f, 0f);

        [Tooltip("Constant wind in world space (m/s).")]
        public Vector3 windWorld = Vector3.zero;

        [Header("Uncertainty / Dispersion")]
        [Tooltip("1-sigma launch angle error in milliradians (mrad). 1 mrad ≈ 0.0573°")]
        public float launchDispersionMrad = 2.0f;

        [Tooltip("1-sigma muzzle velocity variation (%)")]
        public float muzzleVelSigmaPct = 2.0f;

        [Tooltip("Small random crosswind gust (m/s), constant per shot.")]
        public float windSigma = 0.5f;

        [Header("Lifetime / Safety")]
        [Tooltip("Max flight time in seconds before auto-destroy (failsafe).")]
        public float maxLifeTime = 6f;

        [Tooltip("Radius for continuous collision checking and explosion overlap (meters).")]
        public float collisionRadius = 0.06f;

        [Tooltip("What layers the rocket can hit.")]
        public LayerMask collisionMask;

        [Header("Visuals / Audio / FX (optional)")]
        public MeshRenderer projectileMesh;
        public ParticleSystem inFlightTrail;
        public AudioSource inFlightAudio;
        public GameObject explosionPrefab;

        // --- Internal state ---
        private Vector3 _pos;
        private Vector3 _vel;
        private float _life;
        private float _distanceTraveled;
        private bool _ignited;
        private float _burnRemaining;
        private Vector3 _lastPos;
        private Vector3 _windThisShot;

        // RK4 step subdivisions for stability at high speed
        [SerializeField] private int rkSubsteps = 1;

        private void Start()
        {
            // Seed per-shot randoms
            var rng = new System.Random(System.Environment.TickCount ^ GetInstanceID());

            // Launch angle dispersion (mrad → radians)
            float sigmaRad = launchDispersionMrad * 0.001f;
            float yawErr = (float)NextGaussian(rng, 0f, sigmaRad);
            float pitchErr = (float)NextGaussian(rng, 0f, sigmaRad);

            // Tiny rotation from yaw/pitch error
            Quaternion dispersion =
                Quaternion.AngleAxis(Mathf.Rad2Deg * yawErr, Vector3.up) *
                Quaternion.AngleAxis(Mathf.Rad2Deg * pitchErr, Vector3.right);

            // Initial direction with dispersion
            Vector3 dir = (transform.rotation * dispersion) * Vector3.forward;

            // Muzzle velocity variation
            float velScale = 1f + (float)NextGaussian(rng, 0f, muzzleVelSigmaPct / 100f);

            _pos = transform.position;
            _vel = dir.normalized * initialSpeed * velScale;
            _burnRemaining = motorBurnTime;
            _life = 0f;
            _distanceTraveled = 0f;
            _lastPos = _pos;

            // Random wind gust (constant per shot)
            _windThisShot = windWorld + new Vector3(
                (float)NextGaussian(rng, 0f, windSigma),
                (float)NextGaussian(rng, 0f, 0.2f * windSigma),
                (float)NextGaussian(rng, 0f, windSigma)
            );

            if (inFlightAudio) inFlightAudio.Play();
        }

        private void FixedUpdate()
        {
            if (_life > maxLifeTime)
            {
                SelfDestruct(); // failsafe
                return;
            }

            float dt = Time.fixedDeltaTime;
            _life += dt;

            // Integrate with RK4
            int n = Mathf.Max(1, rkSubsteps);
            float h = dt / n;
            for (int i = 0; i < n; i++)
                RK4Step(h);

            // Orient nose to velocity
            if (_vel.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(_vel.normalized, Vector3.up);

            // Continuous collision check
            Vector3 travel = _pos - _lastPos;
            float dist = travel.magnitude;
            if (dist > 0f)
            {
                if (Physics.SphereCast(_lastPos, collisionRadius, travel.normalized,
                    out RaycastHit hit, dist, collisionMask, QueryTriggerInteraction.Ignore))
                {
                    OnHit(hit);
                    return;
                }
            }

            // Update transform and bookkeeping
            transform.position = _pos;
            _distanceTraveled += dist;
            _lastPos = _pos;

            // Ignite motor after specified distance
            if (!_ignited && _distanceTraveled >= motorIgniteDistance)
                _ignited = true;
        }

        // ---- RK4 integrator ----
        private void RK4Step(float h)
        {
            Vector3 p0 = _pos;
            Vector3 v0 = _vel;

            Vector3 a0 = Accel(p0, v0);
            Vector3 v1 = v0 + 0.5f * h * a0;
            Vector3 p1 = p0 + 0.5f * h * v0;

            Vector3 a1 = Accel(p1, v1);
            Vector3 v2 = v0 + 0.5f * h * a1;
            Vector3 p2 = p0 + 0.5f * h * v1;

            Vector3 a2 = Accel(p2, v2);
            Vector3 v3 = v0 + h * a2;
            Vector3 p3 = p0 + h * v2;

            Vector3 a3 = Accel(p3, v3);

            _pos = p0 + (h / 6f) * (v0 + 2f * v1 + 2f * v2 + v3);
            _vel = v0 + (h / 6f) * (a0 + 2f * a1 + 2f * a2 + a3);
        }

        /// <summary>
        /// Acceleration from gravity + drag + (optional) motor thrust during burn.
        /// </summary>
        private Vector3 Accel(Vector3 position, Vector3 velocity)
        {
            // Relative air velocity
            Vector3 vRel = velocity - _windThisShot;
            float speed = vRel.magnitude;

            // Drag: Fd = 0.5 * rho * Cd * A * v^2
            Vector3 drag = Vector3.zero;
            if (speed > 0.001f)
            {
                Vector3 vHat = vRel / speed;
                float q = 0.5f * airDensity * speed * speed;
                Vector3 Fd = -dragCoefficient * referenceArea * q * vHat;
                drag = Fd / mass;
            }

            // Thrust during burn
            Vector3 thrustAcc = Vector3.zero;
            if (_ignited && _burnRemaining > 0f)
            {
                Vector3 dir = (velocity.sqrMagnitude > 0.001f)
                    ? velocity.normalized
                    : transform.forward;

                float currentSpeed = velocity.magnitude;
                float desired = motorTargetSpeed;
                float error = Mathf.Max(0f, desired - currentSpeed);

                // Gain so rocket reaches target roughly within burn time
                float gain = 6f / Mathf.Max(0.05f, motorBurnTime);
                float thrust = gain * error;

                thrustAcc = dir * thrust;

                float dt = Time.fixedDeltaTime / Mathf.Max(1, rkSubsteps);
                _burnRemaining -= dt;
                if (_burnRemaining <= 0f)
                    _burnRemaining = 0f;
            }

            return gravity + drag + thrustAcc;
        }

        private void OnHit(in RaycastHit hit)
        {
            _pos = hit.point;
            transform.position = _pos;

            if (inFlightAudio) inFlightAudio.Stop();
            if (inFlightTrail) inFlightTrail.Stop();
            if (projectileMesh) projectileMesh.enabled = false;

            if (explosionPrefab)
                Instantiate(explosionPrefab, hit.point, Quaternion.LookRotation(hit.normal));

            foreach (var col in GetComponents<Collider>())
                col.enabled = false;

            enabled = false;
            Destroy(gameObject, 5f);
        }

        private void SelfDestruct()
        {
            if (explosionPrefab)
                Instantiate(explosionPrefab, transform.position, transform.rotation);

            if (inFlightAudio) inFlightAudio.Stop();
            if (inFlightTrail) inFlightTrail.Stop();
            if (projectileMesh) projectileMesh.enabled = false;

            foreach (var col in GetComponents<Collider>())
                col.enabled = false;

            Destroy(gameObject, 3f);
        }

        // --- Utility: Gaussian RNG ---
        private static double NextGaussian(System.Random rng, double mean = 0.0, double stdDev = 1.0)
        {
            // Box–Muller transform
            double u1 = 1.0 - rng.NextDouble();
            double u2 = 1.0 - rng.NextDouble();
            double r = System.Math.Sqrt(-2.0 * System.Math.Log(u1));
            double theta = 2.0 * System.Math.PI * u2;
            return mean + stdDev * r * System.Math.Cos(theta);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
            Gizmos.DrawWireSphere(transform.position, collisionRadius);
        }
#endif
    }
}
