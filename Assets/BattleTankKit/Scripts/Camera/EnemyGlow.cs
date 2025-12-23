using UnityEngine;

public class EnemyGlow : MonoBehaviour
{
    public Color glowColor = Color.red;
    [Range(0f, 5f)] public float intensity = 2f;

    void Start()
    {
        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
        {
            foreach (Material mat in rend.materials)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", glowColor * intensity);
            }
        }

        DynamicGI.UpdateEnvironment(); // refresh lighting if GI is on
    }
}
