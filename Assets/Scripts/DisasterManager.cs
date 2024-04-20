using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DisasterManager : MonoBehaviour
{
    [SerializeField] private string[] disasterTypes;

    public bool isDisasterHappening = false;

    //Solar
    [Header("Solar Variables")]
    [SerializeField] private Transform sun;
    [SerializeField] private Image sunFilter;
    [SerializeField] private Transform goToPosSun;
    [SerializeField] private ParticleSystem sunParticles;

    private void Awake(){ DOTween.Init(); }

    private void Start()
    {
        DisasterMake(NextDisasterDecide());
    }

    private string NextDisasterDecide()
    {
        int index = Random.Range(0, disasterTypes.Length);
        return disasterTypes[index];   
    }


    private void SolarStorm()
    {



    }


    private void DisasterMake(string disaster)
    {
        switch(disaster) 
        {

            case "Solar":

                break;

            case "Earthquake":
                break;

            case "Freeze":
                break;
            case "Sand Storm":
                break;

            case "Plague":
                break;

        }


    }
    


}
