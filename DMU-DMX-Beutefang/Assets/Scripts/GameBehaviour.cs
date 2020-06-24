using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBehaviour : MonoBehaviour
{

    [Header("Ringe")]
    [SerializeField] private GameObject basaltRing;
    [SerializeField] private GameObject kupferZinkRing;
    [SerializeField] private GameObject methanHydratRing;
    [SerializeField] private GameObject korallenRing;
    [SerializeField] private GameObject korallenSchwammRing;
    [SerializeField] private GameObject manganKnollenRing;

    [Header("Sounds")]
    [SerializeField] private AudioSource catchSound;
    [SerializeField] private AudioSource gameOverSound;
    [SerializeField] private AudioSource gameStartSound;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource missSound;
    [SerializeField] private AudioSource moveSound;

    [Header("Anim")]
    [SerializeField] private Animator anim;

    private Text timerText;
    private Text scoreText;
    private Text caughtText;
    private Text triesText;

    private List<GameObject> ringe;
    private bool newObject;
    private int counter = 2;
    private int currentIndex = -1;
    private bool running;
    private int score;
    private int caught;
    private int tries;

    private IEnumerator countdownCorountine;
    private IEnumerator newObjectCorountine;

    private void Start()
    {
        var temp = transform.Find("SpielUI").GetChild(0);
        timerText = temp.Find("Timer").GetComponent<Text>();
        scoreText = temp.Find("Score").GetComponent<Text>();
        caughtText = temp.Find("Caught").GetComponent<Text>();
        triesText = temp.Find("Tries").GetComponent<Text>();

        ringe = new List<GameObject>(new[] { basaltRing, kupferZinkRing, methanHydratRing, korallenRing, korallenSchwammRing, manganKnollenRing });
        countdownCorountine = Countdown();
        StartCoroutine(countdownCorountine);
    }

    public void NextObject()
    {
        if (counter == ringe.Count - 1 || running) return;

        moveSound.Play();

        anim.Play(counter + "to" + (counter + 1));

        StartCoroutine(NextObjectDelay());
    }

    private IEnumerator NextObjectDelay()
    {
        running = true;

        if (counter + 1 == currentIndex)
        {
            ringe[counter++].transform.GetChild(0).gameObject.SetActive(false);

            ringe[counter].transform.GetChild(2).gameObject.SetActive(false);

            ringe[counter].transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (counter == currentIndex)
        {
            ringe[counter].transform.GetChild(1).gameObject.SetActive(false);

            ringe[counter++].transform.GetChild(2).gameObject.SetActive(true);

            ringe[counter].transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            ringe[counter++].transform.GetChild(0).gameObject.SetActive(false);

            ringe[counter].transform.GetChild(0).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.1f);

        running = false;
    }

    public void PreviousObject()
    {
        if (counter == 0 || running) return;

        moveSound.Play();

        anim.Play(counter + "to" + (counter - 1));

        StartCoroutine(PreviousObjectDelay());
    }

    private IEnumerator PreviousObjectDelay()
    {
        running = true;

        if (counter - 1 == currentIndex)
        {
            ringe[counter--].transform.GetChild(0).gameObject.SetActive(false);

            ringe[counter].transform.GetChild(2).gameObject.SetActive(false);

            ringe[counter].transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (counter == currentIndex)
        {
            ringe[counter].transform.GetChild(1).gameObject.SetActive(false);

            ringe[counter--].transform.GetChild(2).gameObject.SetActive(true);

            ringe[counter].transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            ringe[counter--].transform.GetChild(0).gameObject.SetActive(false);

            ringe[counter].transform.GetChild(0).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.1f);

        running = false;
    }

    public void GetObject()
    {
        if (newObject && currentIndex == counter)
        {
            hitSound.Play();

            StopCoroutine(newObjectCorountine);
            caughtText.text = "" + ++caught;
            scoreText.text = "" + (score += 100);
            ringe[counter].transform.GetChild(2).gameObject.SetActive(false);
            ringe[counter].transform.GetChild(1).gameObject.SetActive(false);
            ringe[counter].transform.GetChild(0).gameObject.SetActive(true);
            newObject = false;
        }
        else
        {
            catchSound.Play();
        }
    }

    private IEnumerator Countdown()
    {

        for (int i = 10; i >= 0; i--) 
        {
            timerText.text = "" + i;
            yield return new WaitForSeconds(1);
        }

        gameStartSound.Play();

        int countDown = 60;

        while (countDown != 0)
        {

            if (!newObject)
            {
                triesText.text = "" + ++tries;
                newObjectCorountine = NewObject();
                StartCoroutine(newObjectCorountine);
            }

            yield return new WaitForSeconds(1);
            timerText.text = "" + --countDown;
        }

        gameOverSound.Play();
    }

    private IEnumerator NewObject()
    {
        newObject = true;

        do
        {
            currentIndex = Random.Range(0, ringe.Count);
        } while (currentIndex == counter);

        ringe[currentIndex].transform.GetChild(2).gameObject.SetActive(true);

        yield return new WaitForSeconds(10);

        newObject = false;

        missSound.Play();

        ringe[currentIndex].transform.GetChild(2).gameObject.SetActive(false);

        if (counter == currentIndex)
        {
            ringe[currentIndex].transform.GetChild(1).gameObject.SetActive(false);
            ringe[currentIndex].transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}
