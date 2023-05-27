using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float activeTime = 4f;
    private InputManager inputManager;

    [Header("Mesh")]
    public float meshRefreshRate = 0.1f;
    public Transform PosSpawn;
    public float meshDestroyDelay= 0.3f;

    [Header("Shader")]
    public Material mat;

    [Header("Color")]
    [SerializeField] private Color[] lColors;
    [SerializeField] private float lerpTime;
    int colorIndex = 0, len;
    float t = 0f;
    private Color slowColor;

    private bool SlowAtiv;
    private bool SlowCooldown;
    private bool isTrailActive;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    private void Start()
    {
        len = lColors.Length;
    }
    void Update()
    {
        SlowAtiv = ManuseioTemp.timeToggle;
        SlowCooldown = ManuseioTemp.startcooldown;

        slowColor = Color.Lerp(slowColor, lColors[colorIndex], lerpTime * Time.unscaledDeltaTime);
        t = Mathf.Lerp(t, 1, lerpTime * Time.unscaledDeltaTime);

        if (t > 0.9f)
        {
            t = 0;
            colorIndex++;
            colorIndex = (colorIndex == lColors.Length) ? 0 : colorIndex;
        }
        if (SlowAtiv && !isTrailActive && !SlowCooldown)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
        }
        if (SlowCooldown)
        {
            GameObject[] clones = GameObject.FindGameObjectsWithTag("Clone");
            for (int j = 0; j < clones.Length; j++)
            {
                Destroy(clones[j]);
            }
        }

    }

    IEnumerator ActivateTrail(float timeActive)
    {
        if (SlowAtiv)
        {
            while (timeActive > 0 && SlowAtiv)
            {
                timeActive -= meshRefreshRate;

                if (skinnedMeshRenderers == null)
                {
                    skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
                }
                for (int i = 0; i < skinnedMeshRenderers.Length; i++)
                {
                    GameObject gObj = new GameObject();
                    gObj.transform.SetPositionAndRotation(PosSpawn.position, PosSpawn.rotation);
                    gObj.gameObject.tag = "Clone";
                    MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                    MeshFilter mf = gObj.AddComponent<MeshFilter>();


                    Mesh mesh = new Mesh();
                    skinnedMeshRenderers[i].BakeMesh(mesh);
                    mf.mesh = mesh;
                    mr.material = mat;
                    mr.material.color = slowColor;



                }

                yield return new WaitForSecondsRealtime(meshRefreshRate);
            }

            isTrailActive = false;
        }
    }
}
