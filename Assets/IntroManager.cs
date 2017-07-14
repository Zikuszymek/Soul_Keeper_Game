using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour {

    private static string THUNDER = "Thunder";
    private static string JUMP = "Jump";
    private static string RUN = "Run";
    private static string WIND = "WIND";

    public GameObject cloudLeft;
    public GameObject cloudRight;
    public GameObject scythe;

    private Animator cloudLeftAnimator;
    private Animator cloudRightAnimator;
    private Animator grimAnimator;
    private Animator scytheAnimator;

    private GrimReaper grimReaper;

    private bool levelLoading = false;
   
	void Start () {
        cloudLeftAnimator = cloudLeft.transform.GetChild(0).GetComponent<Animator>();
        cloudRightAnimator = cloudRight.transform.GetChild(0).GetComponent<Animator>();
        scytheAnimator = scythe.transform.GetComponent<Animator>();
        grimReaper = FindObjectOfType<GrimReaper>();
        grimAnimator = grimReaper.transform.GetChild(0).GetComponent<Animator>();
        InvokeRandomJunp();
        InvokeRandomAnimation();
    }

    private void DoRandomThunder() {

    }

    private void InvokeRandomJunp() {
        grimAnimator.SetTrigger(JUMP);
        Invoke("InvokeRandomJunp", Random.Range(2, 8));
    }

    private void ScytheThrow() {
        scytheAnimator.SetTrigger(WIND);
    }

    private void InvokeRandomAnimation() {
        int randomCase = Random.Range(0, 3);
        switch (randomCase) {
            case 0:
                cloudLeftAnimator.SetTrigger(THUNDER);
                break;
            case 1:
                cloudRightAnimator.SetTrigger(THUNDER);
                break;
            case 2:
                cloudLeftAnimator.SetTrigger(THUNDER);
                cloudRightAnimator.SetTrigger(THUNDER); ;
                break;
        }
        Invoke("InvokeRandomAnimation", Random.Range(2, 8));
    }

    public void StartGameAnimation() {
        if (!levelLoading) {
            scytheAnimator.SetTrigger(WIND);
            Invoke("RunGrimmer",2f);
            levelLoading = true;
        }
    }

    private void RunGrimmer() {
        grimReaper.Running(true);
        grimAnimator.SetTrigger(RUN);
    }

    public void LoadGameLevel() {
        SceneManager.LoadScene(1);
    }
}
