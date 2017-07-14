using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour {

    private static string DDESTROY_TRIGGER = "DestroySoul";

    public AudioClip[] audioClips;
    public GameObject score;

    private float speed = 10f;
    private float multipler = 1;
    private int soulType;
    public Sprite sprite;
    private SpriteRenderer spriteRenderer;
    private Vector3 targetPosition;
    private Animator animator;
    private AudioSource audioSource;

    public bool isSoulMowing;

    void Start () {
        audioSource = GetComponent<AudioSource>();
        isSoulMowing = true;
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        transform.Translate(Vector3.up * 10, Space.Self);
    }

    private void Update()
    {
        float step = speed * Time.deltaTime * multipler;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        if(transform.position != targetPosition) {
            isSoulMowing = true;
        } else {
            isSoulMowing = false;
        }
    }

    public void setTargetPosition(Vector3 targetPosition, float multipler)
    {
        this.multipler = multipler;
        isSoulMowing = true;
        this.targetPosition = targetPosition;
    }

    public void setSprite(Sprite sprite)
    {
        this.sprite = sprite;
    }

    public void setSoulType(int soulType)
    {
        this.soulType = soulType;
    }

    public int getSoulType()
    {
        return soulType;
    }

    public void StartDestroyAnimations(int i) {
        Invoke("TriggerDestroyAnimations", i * 0.1f);
    }

    private void TriggerDestroyAnimations() {
        SendMessageUpwards("EmpirParticles");
        InvokeScore();
        int randomSong = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[randomSong];
        audioSource.Play();
        animator.SetTrigger(DDESTROY_TRIGGER);
    }

    public void DestroySoul() {
        Destroy(transform.gameObject);
    }

    public void InvokeScore() {
        Instantiate(score, transform.position, Quaternion.identity);
    }
}
