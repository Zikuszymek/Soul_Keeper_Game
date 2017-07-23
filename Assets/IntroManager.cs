using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour {

    private static string JUMP = "Jump";
    private static string RUN = "Run";
    private static string WIND = "WIND";

    public GameObject scythe;

    private Animator grimAnimator;
    private Animator scytheAnimator;
    private GrimReaper grimReaper;

    private bool levelLoading = false;
   
	void Start () {
        scytheAnimator = scythe.transform.GetComponent<Animator>();
        grimReaper = FindObjectOfType<GrimReaper>();
        grimAnimator = grimReaper.transform.GetChild(0).GetComponent<Animator>();
        InvokeRandomJunp();
    }

    private void InvokeRandomJunp() {
        grimAnimator.SetTrigger(JUMP);
        Invoke("InvokeRandomJunp", Random.Range(2, 8));
    }

    private void ScytheThrow() {
        scytheAnimator.SetTrigger(WIND);
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
