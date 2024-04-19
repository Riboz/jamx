using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlanetRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
         GetComponent<Transform>().DORotate(new Vector3(0,0,360),5f,RotateMode.FastBeyond360).SetLoops(-1,LoopType.Restart).SetRelative().SetEase(Ease.Linear);
    }

    // Update is called once per frame
    
}
