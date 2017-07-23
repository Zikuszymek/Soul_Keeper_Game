using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour {

    private static string THUNDER = "Thunder";

    private List<Animator> cloudsAnimatorsList;

    void Start() {
        cloudsAnimatorsList = new List<Animator>();
        foreach(Transform child in transform) {
            cloudsAnimatorsList.Add(child.transform.GetChild(0).GetComponent<Animator>());
        }
        InvokeRandomCloudAnimation();
    }

    private void InvokeRandomCloudAnimation() {
        Invoke("InvokeRandomAnimation", Random.Range(2, 8));
    }

    private void InvokeRandomAnimation() {
        int thundersNumber = Random.Range(1, cloudsAnimatorsList.Count + 1);
        List<int> cloudsToTrigger = getCloudsTriggeredByThunder(thundersNumber);
        foreach(int cloudNumber in cloudsToTrigger) {
            cloudsAnimatorsList[cloudNumber].SetTrigger(THUNDER);
        }
        InvokeRandomCloudAnimation();
    }

    private List<int> getCloudsTriggeredByThunder(int numberOfClouds) {
        List<int> cloudsTriggeredList = new List<int>();
        for(int i = 0; i < numberOfClouds; i++) {
            int randomCloud = Random.Range(0, cloudsAnimatorsList.Count);
            while (cloudsTriggeredList.Contains(randomCloud)){
                randomCloud = Random.Range(0, cloudsAnimatorsList.Count);
            }
            cloudsTriggeredList.Add(randomCloud);
        }
        return cloudsTriggeredList;
    }

}
