using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontGrimReaper : MonoBehaviour {

    private static string HAPPY = "Happy";
    private static string DANCE1 = "Dance1";
    private static string DANCE2 = "Dance2";

    private Animator grimAnimator;

    private bool levelLoading = false;

    void Start() {
        grimAnimator = transform.GetChild(0).GetComponent<Animator>();
        InvokeRandomGrimAnimation();
    }

    private void InvokeRandomGrimAnimation() {
        int randomCase = Random.Range(0, 3);
        switch (randomCase) {
            case 0:
                grimAnimator.SetBool(HAPPY, true);
                break;
            case 1:
                grimAnimator.SetBool(DANCE1, true);
                break;
            case 2:
                grimAnimator.SetBool(DANCE2, true);
                break;
        }
        Invoke("InvokeRandomBack", Random.Range(2, 4));
    }

    private void InvokeRandomBack() {
        grimAnimator.SetBool(HAPPY, false);
        grimAnimator.SetBool(DANCE1, false);
        grimAnimator.SetBool(DANCE2, false);
        Invoke("InvokeRandomGrimAnimation", Random.Range(6, 8));
    }

}
