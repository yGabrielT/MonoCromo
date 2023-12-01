using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraDetect : MonoBehaviour
{
    [SerializeField]private Transform CameraPos;
    private InimigoFov _fov;
    [SerializeField] Light _light;
    [SerializeField] Color _colorActivated;
    [SerializeField] Color _colorDeactivated;
    private bool isRotating;
    [SerializeField] private GameObject[] _torretaGO;
    // Start is called before the first frame update
    void Start()
    {
        _fov = GetComponent<InimigoFov>();
        isRotating = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_fov.canSeePlayer){
            for(int i =0; i < _torretaGO.Length; i++){
                _torretaGO[i].GetComponent<Inimigo>().AtacarBool = true;
            }
            
            _light.DOColor(_colorActivated,.5f);
            DOTween.Pause(CameraPos);
            this.CameraPos.transform.LookAt(_fov.playerRef.transform);
            Debug.Log(_fov.playerRef.transform);
            var angle = transform.rotation.eulerAngles;
            angle.x = 0;
            angle.z = 0;
            transform.rotation = Quaternion.Euler(angle);
        }
        else{
            _light.DOColor(_colorDeactivated,.5f);
            
            if(isRotating){
                isRotating = false;
                CameraPos.DORotate(new Vector3(0,-50f,0),2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
            }
        }
    }
}
