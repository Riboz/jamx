using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

public class DisasterManager : MonoBehaviour
{
    // 0 -> Solar Storm
    // 1 -> Earthquak
    // 2 -> Plague
    // 3 -> Electricity Failure
    [SerializeField] private string[] disasterTypes;
    [SerializeField] private CinemachineVirtualCamera mainCamera;

    public bool isDisasterHappening = false;
    public int disasterIndex = -1;

    private float timer = 0f;
    private CinemachineBasicMultiChannelPerlin noise;
   


    //Solar
    [Header("Solar Variables")]
    [SerializeField] private Transform sun;
    [SerializeField] private Image sunFilter;
    [SerializeField] private Transform goToPosSun;
    [SerializeField] private ParticleSystem sunParticles;
    [SerializeField] private Transform initialSunPos;




    private void Awake()
    {
        DOTween.Init();
        
    }

    private void Start()
    {

       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SolarStormEnd();
            StopCameraShake();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SolarStorm();
            ShakeScreen();
        }

     

        timer += Time.deltaTime;
        if (timer > 5f)
        {
            CallDisaster(NextDisasterDecide());
            isDisasterHappening = true;
            timer = 0f;

            Debug.Log("Disaster is happening");

        }


    }

    //Earthquake
    private void ShakeScreen()
    {

        noise =mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise)
        {
            noise.m_AmplitudeGain = 2f;
            noise.m_FrequencyGain = 35f;
        }

    }

    private void StopCameraShake()
    {
        noise =mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise)
        {
            noise.m_AmplitudeGain = 0f;
            disasterIndex = -1;
        }

    }

    private void CallDisaster(string disasterName)
    {
        switch (disasterName)
        {
            case "Solar":
                SolarStorm();
                disasterIndex = 0;
                break;

            case "Earthquake":
                disasterIndex = 1;

                break;

            case "Plague":

                break;

            case "Electricity":

                break;

        }

    }

    private string NextDisasterDecide()
    {
        int index = Random.Range(0, disasterTypes.Length);
        return disasterTypes[index];
    }


    private void SolarStorm()
    {
        sun.transform.DOMove(goToPosSun.position, 3f, false);
        sun.transform.DOScale(1.5f, 1f);
        sunParticles.startSpeed = 50f;
        sunParticles.maxParticles = 3000;
        sunFilter.gameObject.SetActive(true);
        StartCoroutine(nameof(ChangeFade));
        Debug.Log("Its storming");
    }

    private void SolarStormEnd()
    {

        sun.transform.DOMove(initialSunPos.position, 1f);
        sun.transform.DOScale(1f, 1f);
        sunFilter.gameObject.SetActive(false);
        sunParticles.startSpeed = 5f;
        sunParticles.maxParticles = 1000;
        StopCoroutine(nameof(ChangeFade));
        disasterIndex = -1;
        Debug.Log("Solar is storm is resolved.");

    }

    private IEnumerator ChangeFade()
    {
        while (true)
        {
            sunFilter.GetComponent<Image>().DOFade(0.15f, 2f);
            yield return new WaitForSeconds(2.5f);
            sunFilter.GetComponent<Image>().DOFade(0.5f, 0f);
        }

    }

     



}
