using UnityEngine;
using UnityEngine.UI; // ✅ for normal UI Text
using TMPro;


[RequireComponent(typeof(Rigidbody))]
public class SimpleRPGRocket : MonoBehaviour
{
    [Header("Flight Settings")]
    public RocketFlightSettings flightSettings;
    [Header("Flight")]
    public float initialSpeed = 60f;
    public float thrustForce = 250f;
    public float burnTime = 0.8f;
    public float linearDrag = 0.05f;
    public float gravityMultiplier = 1.0f;
    public float autoDestroyTime = 20f;

    [Header("Options")]
    public bool useInitialImpulse = true;
    public bool debugDraw = false;

    [Header("Explosion Settings")]
    public GameObject explosionEffect;
    public float explodeDistance = 400f;
    public float explosionDestroyDelay = 1f;

    Rigidbody rb;
    Vector3 startPosition;
    Vector3 aimDirection; // for dispersion
    bool hasExploded = false;
    float burnTimer = 0f;
    bool burnedOut = false;
    float destroyTimer = 0f;

    [Header("UI Display")]
    public TextMeshProUGUI statsDisplay; // Assign in Inspector

    // --- 📊 Stats Tracking ---
    float launchTime;
    float flightTime;
    float maxSpeed;
    float maxAltitude;
    float impactSpeed;
    float range;
    float dispersion;
    string targetName = "None";
    bool hitTarget = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 🔗 Get global rocket settings
        if (GameSettings.Instance != null)
            flightSettings = GameSettings.Instance.currentRocketSettings;

        // 🧠 Apply settings
        if (flightSettings != null)
        {
            initialSpeed = flightSettings.initialSpeed;
            thrustForce = flightSettings.thrustForce;
            burnTime = flightSettings.burnTime;
            linearDrag = flightSettings.linearDrag;
            gravityMultiplier = flightSettings.gravityMultiplier;
            autoDestroyTime = flightSettings.autoDestroyTime;
        }

        rb.drag = linearDrag;
        rb.useGravity = false;

        startPosition = transform.position;
        aimDirection = -transform.up;
        launchTime = Time.time;

        if (useInitialImpulse && initialSpeed != 0f)
        {
            rb.velocity = aimDirection * initialSpeed;
        }
    }



    void FixedUpdate()
    {
        // --- Apply thrust while burning ---
        if (burnTimer < burnTime)
        {
            rb.AddForce(aimDirection * thrustForce, ForceMode.Force);
            burnTimer += Time.fixedDeltaTime;
            if (debugDraw)
                Debug.DrawRay(transform.position, aimDirection * 2f, Color.green, 0.1f);
        }
        else if (!burnedOut)
        {
            burnedOut = true;
            rb.useGravity = true;
        }

        // --- Apply extra gravity ---
        if (burnedOut && gravityMultiplier > 1f)
        {
            Vector3 extraGravity = (gravityMultiplier - 1f) * Physics.gravity * rb.mass;
            rb.AddForce(extraGravity, ForceMode.Force);
        }

        // --- Update flight stats ---
        float speed = rb.velocity.magnitude;
        if (speed > maxSpeed) maxSpeed = speed;
        if (transform.position.y > maxAltitude) maxAltitude = transform.position.y;

        // --- Auto explode if traveled too far ---
        range = Vector3.Distance(startPosition, transform.position);
        if (range >= explodeDistance && !hasExploded)
        {
            Explode();
        }

        // --- Cleanup timer ---
        destroyTimer += Time.fixedDeltaTime;
        if (autoDestroyTime > 0f && destroyTimer > autoDestroyTime)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;

        // ✅ Hit check
        hitTarget = collision.gameObject.CompareTag("Enemy");
        if (hitTarget)
        {
            targetName = collision.gameObject.name;
            Destroy(collision.gameObject);
        }
        else
        {
            targetName = collision.gameObject.name;
        }

        // 💥 Record impact data and explode
        impactSpeed = rb.velocity.magnitude;
        flightTime = Time.time - launchTime;

        // --- Dispersion (deviation from initial aim) ---
        Vector3 toImpact = (transform.position - startPosition).normalized;
        dispersion = Vector3.Distance(
            Vector3.ProjectOnPlane(transform.position - startPosition, aimDirection),
            Vector3.zero
        );

        Explode();
        ShowFlightStats();
        SaveFlightStats();
    }

    void SaveFlightStats()
    {
        RocketFlightRecord record = new RocketFlightRecord
        {
            hitTarget = hitTarget,
            targetName = targetName,
            range = range,
            flightTime = flightTime,
            impactSpeed = impactSpeed,
            maxSpeed = maxSpeed,
            maxAltitude = maxAltitude,
            dispersion = dispersion,
            timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        if (RocketSessionManager.Instance != null)
        {
            RocketSessionManager.Instance.AddRecord(record);
        }
    }



    void Explode()
    {
        hasExploded = true;

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        if (TryGetComponent<MeshRenderer>(out var mesh)) mesh.enabled = false;
        if (TryGetComponent<Collider>(out var col)) col.enabled = false;

        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        Destroy(gameObject, explosionDestroyDelay);
    }

    void ShowFlightStats()
    {
        string result = $@"
RPG-7 Flight Stats ────────────
Hit: {(hitTarget ? "YES" : "NO")}
Target: {targetName}
Range: {range:F1} m
Flight Time: {flightTime:F2} s
Impact Velocity: {impactSpeed:F1} m/s
Max Speed: {maxSpeed:F1} m/s
Max Altitude: {maxAltitude:F1} m
Lateral error: {dispersion:F2} m";

        // ✅ Send the text to the UI manager
        if (RocketUIManager.Instance != null)
        {
            RocketUIManager.Instance.ShowStats(result);
        }

        // (Optional) Also log it in console
        Debug.Log(result);
    }

}

