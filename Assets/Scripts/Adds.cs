using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using System;

public class Adds : MonoBehaviour
{
    private string gameId = "3992959", type = "video";
    private bool testMode=true, needToStop;
    private Coroutine showAd;
    private static int countLoses;

    private void Start()
    {
        Advertisement.Initialize(gameId, testMode);
        countLoses++;
        if(countLoses%5 == 0)
            showAd = StartCoroutine(ShowAds());
    }

    private void Update()
    {
        if (needToStop)
        {
            needToStop = false;
            StopCoroutine(showAd);
        }

    }

    IEnumerator ShowAds()
    {
        while (true)
        {
            if (Advertisement.IsReady(type))
            {
                Advertisement.Show(type);
                needToStop = true;
            }

            yield return new WaitForSeconds(1f);
        }

    }
}
