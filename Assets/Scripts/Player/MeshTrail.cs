using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float activeTime = 4f;

    [Header("Mesh")]
    public float meshRefreshRate = 0.1f;
    public Transform PosSpawn;
    // public float meshDestroyDelay= 0.3f;

    [Header("Shader")]
    public Material mat;

    [Header("Color")]
    [SerializeField] private Color[] lColors;
    [SerializeField] private float lerpTime;
    int colorIndex = 0, len;
    float t = 0f;
    private Color slowColor;

    private bool SlowCooldown;
    private bool isTrailActive;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    private ManuseioTemp _manuseioTemp;

    private void Start()
    {
        _manuseioTemp = GetComponent<ManuseioTemp>();
        len = lColors.Length;
    }
    void Update()
    {


        slowColor = Color.Lerp(slowColor, lColors[colorIndex], lerpTime * Time.unscaledDeltaTime);
        t = Mathf.Lerp(t, 1, lerpTime * Time.unscaledDeltaTime);

        if (t > 0.9f)
        {
            t = 0;
            colorIndex++;
            colorIndex = (colorIndex == lColors.Length) ? 0 : colorIndex;
        }
        if (_manuseioTemp.timeToggle && !isTrailActive && !_manuseioTemp.startcooldown)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
        }

        //Deletar clones apos o uso da habilidade
        if (_manuseioTemp.startcooldown)
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
        if (_manuseioTemp.timeToggle)
        {
            while (timeActive > 0 && _manuseioTemp.timeToggle)
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

                    // Deletar clones um apos o outro ap�s um tempo da cria��o
                    //Destroy(gObj, meshDestroyDelay);
                }

                yield return new WaitForSecondsRealtime(meshRefreshRate);
            }

            isTrailActive = false;
        }
    }
}
