using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class Ads : MonoBehaviour
{
    private string gameId = "3587398", type = "video";
    private bool testMode = true;
    private void Start()
    {
        Advertisement.Initialize(gameId, testMode);
        StartCoroutine(ShowAd());

        IEnumerator ShowAd()
        {
            while(true)
            {
                if(Advertisement.IsReady(type))
                { 
                    Debug.Log("Ready");
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
