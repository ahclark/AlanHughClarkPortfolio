using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{


    [SerializeField]
    Light mainLight;
    //int timeofday = 0;
    //public Color[] colors;
    //enum DayTimes { Morning = 0, Noon, Afternoon, Evening, Sunset, Night, Midnight, Sunrise}
    //public float TimeTick;
    //Color ColorA, ColorB;
    //float lerpRatio = 0;
    public delegate void SwitchNightTime(bool nightTime);
    public static event SwitchNightTime NightTimeSwitch;

    public enum ColorLerp
    {
        Smooth,
        Linear
    }

    [SerializeField]
    Color NightStartColor;

    public bool NightTime = false;

    [SerializeField]
    Color beginning;

    [SerializeField]
    Color end;

    bool fadeDown = true;

    [SerializeField]
    float duration = 1;
    [SerializeField]
    float starDuration = 1;
    [SerializeField]
    float tutorialDuration = 180.0f;

    [SerializeField]
    ColorLerp lerp;

    [SerializeField]
    GameObject sun;
    [SerializeField]
    GameObject stars;
    [SerializeField]
    GameObject skyObject;
    Material sky;
    Material StarsMat;

    Coroutine starTwinkle;

    float timer;
    float dayPercentage;
    float startTime;

    public bool starting = true;


    public static DayNightCycle dayInstance = null;
    Vector3 sunTransformLocal;

    private void Awake()
    {
        if (dayInstance == null)
            dayInstance = this;
        else if (dayInstance != this)
            Destroy(gameObject);
    }
        // Use this for initialization
        void Start()
    {
        startTime = Time.time;
        timer = 0.0f;
        sky = skyObject.GetComponent<MeshRenderer>().material;
        StarsMat = stars.GetComponent<MeshRenderer>().material;
        //if (sun.transform.parent)
        //    sunTransformLocal = sun.transform.InverseTransformDirection(sun.transform.forward);
    }

    //private void NextColor()
    //{
    //
    //    if (timeofday < 7)
    //    {
    //        timeofday++;
    //    }
    //    else
    //    {
    //        timeofday = 0;
    //    }
    //    ColorA = ColorB;
    //    ColorB = colors[timeofday];
    //
    //}
    private void FixedUpdate()
    {
        /*timer += Time.deltaTime;
        
        if(timer >= TimeTick)
        {
            timer = 0;
            lerpRatio += Time.deltaTime;
            mainLight.color = Color.Lerp(ColorA, ColorB, lerpRatio);
        
            if(lerpRatio >= 1)
            {
                lerpRatio = 0;
                NextColor();
            }
        
        }*/
        if(tutorialDuration >= 0.0f)
            tutorialDuration -= Time.deltaTime;
        if (tutorialDuration >= 0.0f)
        {
            if (!starting)
                return;
        }
        if(tutorialDuration <= 0.0f && !starting)
        {
            StartDay();
        }
        float t = 0.0f;
        if (lerp == ColorLerp.Smooth)
        {
            t = (Mathf.Sin(Mathf.PI * (Time.time - startTime) / duration) + 1) / 2.0f;
        }
        if (lerp == ColorLerp.Linear)
        {
            t = Mathf.PingPong((Time.time - startTime), duration) / duration;
        }

        mainLight.color = Color.Lerp(beginning, end, t);
        float angle = (((Time.time - startTime) / duration) / 2.0f) % 1.0f;
        //Debug.Log(angle);
        sun.transform.RotateAround(transform.position, -sun.transform.forward, (360.0f/duration*Time.deltaTime)/2.0f);
        sky.mainTextureOffset = new Vector2((((Time.time - startTime)/duration)/2.0f + 0.5f) % 1.0f, sky.mainTextureOffset.y);

        if (!NightTime && mainLight.color.b <= NightStartColor.b)
        {
            NightTime = true;
            stars.SetActive(true);
            starTwinkle = StartCoroutine(TwinkleStars());
            //NightTimeSwitch(NightTime);
        }
        if (NightTime && mainLight.color.b >= NightStartColor.b)
        {
            NightTime = false;
            stars.SetActive(false);
            StopCoroutine(starTwinkle);
            // NightTimeSwitch(NightTime);
        }
        //timer = Time.time - startTime;
    }

    public void StartDay()
    {
        if (!starting)
        {
            starting = true;
            startTime = Time.time;
        }
    }

    //void TimeofDay()
    //{
    //    timer += Time.deltaTime;
    //    dayPercentage = timer / (realminutesinDay * 60.0f);
    //    if (timer > realminutesinDay * 60.0f)
    //    {
    //        timer = 0.0f;
    //    }
    //}

    IEnumerator TwinkleStars()
    {
        float startingTime = Time.time;
        Color beginningStar = StarsMat.color;
        Color endStar = StarsMat.color;
        beginningStar.a = 0.0f;
        endStar.a = 1.0f;
        while(true)
        {
            float t = 0.0f;
            t = Mathf.PingPong((Time.time - startingTime), starDuration) / starDuration;
            StarsMat.color = Color.Lerp(beginningStar, endStar, t);
            yield return null;
        }
    }





}
