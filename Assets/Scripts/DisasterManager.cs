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
    // 3 -> Freeze
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


    //Freeze
    [Header("Freeze Variables")]
    [SerializeField] private Image winterFilter;
    [SerializeField] private Transform winterObject;
    [SerializeField] private Transform sunFarPosition;

    //Plague
    [Header("Plague Variables")]
    [SerializeField] private Image plagueFilter;
    [SerializeField] private Transform plagueObject;


    [SerializeField] private AudioSource earthquakeSound;
    [SerializeField] private AudioSource sunSound;
    [SerializeField] private AudioSource freezeSound;
    [SerializeField] private AudioSource plagueSound;



    private void Awake()
    {
        DOTween.Init();

    }

  

    private void Update()
    {


        timer += Time.deltaTime;
        if (timer > 15f)
        {
            CallDisaster(NextDisasterDecide());
            isDisasterHappening = true;
            timer = 0f;

            Debug.Log("Disaster is happening");

        }


    }



    private string NextDisasterDecide()
    {
        int index = Random.Range(0, disasterTypes.Length);

        switch (disasterIndex)
        {
            case 0:
                StopSolarStorm();
                break;
            case 1:
                StopEarthquake();
                break;
            case 2:
                StopPlague();
                break;
            case 3:
                StopFreeze();
                break;
        }

        return disasterTypes[index];
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
                Earthquake();
                disasterIndex = 1;
                break;

            case "Plague":
                Plague();
                disasterIndex = 2;
                break;

            case "Freeze":
                Freeze();
                disasterIndex = 3;
                break;

        }

    }

    //Earthquake
    private void Earthquake()
    {

        noise = mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise)
        {
            noise.m_AmplitudeGain = 1f;
            noise.m_FrequencyGain = 15f;
        }
        earthquakeSound.PlayOneShot(earthquakeSound.clip);

        Debug.Log("Earthquake is happening gg");
    }


    private void SolarStorm()
    {
        sun.transform.DOMove(goToPosSun.position, 3f, false);
        sun.transform.DOScale(1.5f, 1f);
        sunParticles.startSpeed = 50f;
        sunParticles.maxParticles = 3000;
        sunFilter.gameObject.SetActive(true);
        StartCoroutine((ChangeFade(disasterIndex)));
        sunSound.PlayOneShot(sunSound.clip);
        Debug.Log("Its storming");
    }

    private void Freeze()
    {

        winterFilter.gameObject.SetActive(true);
        winterObject.gameObject.SetActive(true);
        StartCoroutine(ChangeFade(disasterIndex));
        freezeSound.PlayOneShot(freezeSound.clip);
        Debug.Log("Its freezing");

    }


    private void Plague()
    {
        plagueFilter.gameObject.SetActive(true);
        plagueObject.gameObject.SetActive(true);
        plagueSound.PlayOneShot(plagueSound.clip);
        StartCoroutine(ChangeFade(disasterIndex));

    }

    public void StopPlague()
    {

        plagueFilter.gameObject.SetActive(false);
        plagueObject.gameObject.SetActive(false);
        StopCoroutine(ChangeFade(disasterIndex));
        disasterIndex = -1;
        plagueSound.Stop();
        Debug.Log("Plague is stopped");
    }



    public void StopEarthquake()
    {
        noise = mainCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise)
        {
            noise.m_AmplitudeGain = 0f;
            disasterIndex = -1;
        }
        earthquakeSound.Stop();

    }


    private void StopSolarStorm()
    {

        sun.transform.DOMove(initialSunPos.position, 1f);
        sun.transform.DOScale(1f, 1f);
        sunFilter.gameObject.SetActive(false);
        sunParticles.startSpeed = 5f;
        sunParticles.maxParticles = 1000;
        StartCoroutine(ChangeFade(disasterIndex));
        disasterIndex = -1;
        sunSound.Stop();
        Debug.Log("Solar is storm is resolved.");

    }

    private void StopFreeze()
    {
        disasterIndex = -1;
        winterFilter.gameObject.SetActive(false);
        winterObject.gameObject.SetActive(false);
        freezeSound.Stop();
        StopCoroutine(ChangeFade(disasterIndex));

    }

    private IEnumerator ChangeFade(int disaster)
    {
        while (true)
        {
            switch (disasterIndex)
            {
                case 0:
                    sunFilter.GetComponent<Image>().DOFade(0.1f, 2f);
                    yield return new WaitForSeconds(2f);
                    sunFilter.GetComponent<Image>().DOFade(0.5f, 0.5f);
                    break;

                case 1:

                     yield return new WaitForSeconds(0.5f);
                     break;

                case 2:
                    plagueFilter.GetComponent<Image>().DOFade(0.15f, 2f);
                    yield return new WaitForSeconds(2f);
                    plagueFilter.GetComponent<Image>().DOFade(0.5f, 0.5f);
                    break;

                case 3:
                    winterFilter.GetComponent<Image>().DOFade(0.1f, 2f);
                    yield return new WaitForSeconds(2f);
                    winterFilter.GetComponent<Image>().DOFade(0.5f, 0.5f);
                    break;

                default:
                    yield return new WaitForSeconds(1f);
                    break;



            }

            yield return new WaitForSeconds(0.5f);
        }

    }





}
