using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spookable : MonoBehaviour
{
    [SerializeField] AudioClip momScaredSFX,
        dadScaredSFX,
        normalScare;

    [SerializeField] Animator momObj,
        dadObj;
    [SerializeField] bool isMom,
        isDad,
        isNormalScare;

    Animator flame;
    public Color[] colorToggle;

    public bool needsCompletion = true;

    public Animator animator;
    public string animationToPlay;

    public GameObject[] objectsToActivate;
    public bool startObjectsAsDeactivated = false;
    public GameObject[] objectsToDeactivate;

    public bool isActive = false;
    bool completed = false;

    float reboundTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        flame = GetComponentInChildren<Animator>();

        for (int i = 0; i < objectsToActivate.Length; i++) objectsToActivate[i].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        // if we need to-- reactivate flame after time is up
        if (needsCompletion && !completed && isActive && Time.time > reboundTimer)
        {
            flame.Play("BlueFlameIntro");
            flame.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            isActive = false;

            // deactivate thingies
            for (int i = 0; i < objectsToActivate.Length; i++) objectsToActivate[i].SetActive(false);
        }
    }

    void DoTrigger()
    {
        if (isActive) return;

        flame.SetTrigger("Fizzle");

        isActive = true;
        if (needsCompletion) reboundTimer = Time.time + 2;

        // play animations
        if (animator != null) animator.Play(animationToPlay);

        // activate objects
        for (int i = 0; i < objectsToActivate.Length; i++) objectsToActivate[i].SetActive(true);

        // deactivate objects
        for (int i = 0; i < objectsToDeactivate.Length; i++) objectsToDeactivate[i].SetActive(false);

        if (isMom)
            scareMom();

        if (isDad)
            scareDad();

        if (isNormalScare)
            normalScareSFx();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("triggered!");
        DoTrigger();

    }

    public void SetCompleted()
    {
        completed = true;
    }

    public void scareMom()
    {
        if(Vector3.Distance(transform.position, momObj.transform.position) < 5)
        {
            momObj.GetComponent<AudioSource>().PlayOneShot(momScaredSFX);
            momObj.SetTrigger("scared");
        }
    }

    public void scareDad()
    {
        if (Vector3.Distance(transform.position, dadObj.transform.position) < 5)
        {
            dadObj.GetComponent<AudioSource>().PlayOneShot(dadScaredSFX);
            dadObj.SetTrigger("scared");
        }
    }

    public void normalScareSFx()
    {
        GetComponent<AudioSource>().PlayOneShot(normalScare);
    }
}
