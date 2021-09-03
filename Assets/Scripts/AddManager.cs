using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AddManager : MonoBehaviour, IUnityAdsListener
{
    string GooglePlay_ID = "3762829";
    bool TestMode = false;
    string myPlacementId = "rewardedVideo";
    public MouseMovement_RB mm;
    public GameObject internetError;
    public GameObject skippedAd;
    public GameManager Gm;
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(GooglePlay_ID,TestMode);
    }

    public void DisplayInterstitialAD(){
        Advertisement.Show();
    }
    public void DisplayVideoAD(){
        Advertisement.Show(myPlacementId);
    }
        // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished) {
            // Reward the user for watching the ad to completion.
            mm.Revive();
            Gm.Check();
        } else if (showResult == ShowResult.Skipped) {
            skippedAd.SetActive(true);
            // Do not reward the user for skipping the ad.
        } else if (showResult == ShowResult.Failed) {
            internetError.SetActive(true);
            StartCoroutine(ClearConsole());
        }
    }

    public void OnUnityAdsReady (string placementId) {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == myPlacementId) {
            // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError (string message) {
        // Log the error.
    }

    public void OnUnityAdsDidStart (string placementId) {
        // Optional actions to take when the end-users triggers an ad.
    } 

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy() {
        Advertisement.RemoveListener(this);
    }
    IEnumerator ClearConsole()
    {
        // wait until console visible
        while(!Debug.developerConsoleVisible)
        {
            yield return null;
        }
        yield return null; // this is required to wait for an additional frame, without this clearing doesn't work (at least for me)
        Debug.ClearDeveloperConsole();
    }
}
