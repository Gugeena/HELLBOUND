using System.Collections;
using UnityEngine;

public class TenthLayerOfHellScript : MonoBehaviour
{
    public GameObject fireball;
    Animator TLOHanim;
    public bool alreadyin = false;
    AnimationClip[] animations;
    int last = -1;
    public Transform logspawninglocation;
    public GameObject log;
    public SpriteRenderer vinigreti;
    public static bool stoned = false;
    public static bool shouldturnoffforawhile = false;
    public AudioClip logaudio, snakeaudio, acidrain;
    AudioSource lastplayedaudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
        alreadyin = false;
        last = -1;
        stoned = false;
        shouldturnoffforawhile = false;
        //StartCoroutine(EventManager());
        */
        shouldturnoffforawhile = false;
    }

    private void OnEnable()
    {
        if (TLOHanim == null) TLOHanim = GetComponent<Animator>();
        if (animations == null) animations = TLOHanim.runtimeAnimatorController.animationClips;
        alreadyin = false;
        last = -1;
        stoned = false;
        //shouldturnoffforawhile = false;
        StartCoroutine(EventManager());
    }

    private void OnDisable()
    {
        if (lastplayedaudio != null) lastplayedaudio.Stop();
        TLOHanim.Play("idletlohl");
    }

    public IEnumerator EventManager()
    {
        while (true)
        {
            print("alreadyin: " + alreadyin + ", hasdied: " + PlayerMovement.hasdiedforeverybody + ", shouldturnoff: " + shouldturnoffforawhile);
            if (!alreadyin && !PlayerMovement.hasdiedforeverybody && !shouldturnoffforawhile)
            {
                int random;
                do { random = UnityEngine.Random.Range(0, 4); }
                while (random == last || random == 3 && LilithScript.bossfightstarted);
                last = random;
                alreadyin = true;
                switch (random)
                {
                    case 0: StartCoroutine(snakeattack()); break;
                    case 1: StartCoroutine(Fireballrain()); break;
                    case 2: StartCoroutine(logs()); break;
                    case 3: StartCoroutine(gettingstoned()); break;
                }
            }
            yield return null;
        }
    }

    public IEnumerator Fireballrain()
    {
        StartCoroutine(ColorFade(vinigreti, Color.green, 0.5f));
        lastplayedaudio = audioManager.instance.playAudio(acidrain, 1, 1, this.transform, audioManager.instance.sfx);
        for (int i = 0; i < 40; i++)
        {
            if (shouldturnoffforawhile)
            {
                if (lastplayedaudio != null && lastplayedaudio.isPlaying) lastplayedaudio.Stop();
                alreadyin = false;
                yield break;
            }
            yield return new WaitForSeconds(0.25f);
            float x1 = UnityEngine.Random.Range(14.07f, 36.05f);
            Instantiate(fireball, new Vector2(x1, 6.86f), Quaternion.identity);
        }
        yield return new WaitForSeconds(1f);
        alreadyin = false;
    }

    public IEnumerator snakeattack()
    { 
        if (shouldturnoffforawhile)
        {
            if (lastplayedaudio != null && lastplayedaudio.isPlaying) lastplayedaudio.Stop();
            alreadyin = false;
            yield break;
        }
        lastplayedaudio = audioManager.instance.playAudio(snakeaudio, 1, 1, this.transform, audioManager.instance.sfx);
        StartCoroutine(ColorFade(vinigreti, Color.red, 0.5f));
        string animation;
        int random;
        do { random = UnityEngine.Random.Range(0, animations.Length); }
        while (animations[random].name == "idletlohl");
        animation = animations[random].name;
        float waittime = 3.5f;
        if (animation.ToLower().Contains("vertical")) waittime = 3f;
        TLOHanim.Play(animations[random].name);
        yield return new WaitForSeconds(waittime);
        alreadyin = false;
        yield break;
    }

    public IEnumerator logs()
    {
        StartCoroutine(ColorFade(vinigreti, Color.sandyBrown, 0.5f));
        lastplayedaudio = audioManager.instance.playAudio(logaudio, 1, 1, this.transform, audioManager.instance.sfx);
        for (int i = 0; i < 10; i++)
        {
            if (shouldturnoffforawhile)
            {
                if(lastplayedaudio != null && lastplayedaudio.isPlaying) lastplayedaudio.Stop();
                alreadyin = false;
                yield break;
            }
            yield return new WaitForSeconds(1f);
            Instantiate(log, logspawninglocation.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(5f);
        alreadyin = false;
    }

    public IEnumerator ColorFade(SpriteRenderer sprite, Color target, float duration)
    {
        Color start = sprite.color;
        float t = 0f;
        target.a = 0.2f;
        while (t < duration)
        {
            t += Time.deltaTime;
            sprite.color = Color.Lerp(start, target, t / duration);
            yield return null;
        }
        sprite.color = target;
    }

    public IEnumerator gettingstoned()
    {
        StartCoroutine(ColorFade(vinigreti, Color.cyan, 0.5f));
        stoned = true;
        yield return new WaitForSeconds(5f);
        stoned = false;
        yield return new WaitForSeconds(1f);
        alreadyin = false;
    }

    public void onPlayerDeath(float time)
    {
        StartCoroutine(ColorFade(vinigreti, Color.black, 0.1f));
        StartCoroutine(SoundTurnOff(time));
    }

    private IEnumerator SoundTurnOff(float time)
    {
        yield return new WaitForSeconds(time);
        if (lastplayedaudio != null && lastplayedaudio.isPlaying) lastplayedaudio.Stop(); lastplayedaudio.Stop();
    }
}
