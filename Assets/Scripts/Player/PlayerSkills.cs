using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{
    [Space(10)]
    [Header("Turn Into Box Skill")]

    [SerializeField]
    float turnIntoBoxCD = 8f;

    [SerializeField]
    GameObject boxModel;

    [SerializeField]
    GameObject[] playerModels;

    GameObject instantiatedBoxModel;

    [SerializeField]
    ParticleSystem puffEffect;

    //UI
    [SerializeField]
    TextMeshProUGUI turnIntoBoxCDText;

    [SerializeField]
    Image turnIntoBoxSkillIcon;
    float turnIntoBoxCountDownUI = 0;

    [Space(10)]
    [Header("Show Behind Walls Skill")]

    [SerializeField]
    float showHiddenWallCD = 6f;

    [SerializeField]
    Material hiddenWallMaterial;

    //UI
    [SerializeField]
    TextMeshProUGUI hiddenWallCDText;

    [SerializeField]
    Image hiddenWallSkillIcon;
    float hiddenWallCountDownUI = 0;

    Material solidWallMaterial;


    [Space(10)]
    [Header("Hide Into Straw Skill")]

    [SerializeField]
    ParticleSystem strawMoveEffect;
    [SerializeField]
    Transform moveEffectPos;

    [SerializeField]
    AudioClip onEnterLeaveStrawSFX;


    [Space(10)]
    [Header("Whistle Skill")]

    [SerializeField]
    GameObject lastKnowPosition;

    [SerializeField]
    ParticleSystem whistleEffect;

    [SerializeField]
    float whistleCD = 6f;


    [SerializeField]
    AudioClip whistleSound;
    //UI
    [SerializeField]
    TextMeshProUGUI whistleCDText;

    [SerializeField]
    Image whistleSkillIcon;
    float whistleCountDownUI = 0;



    //Image SKill Icon CD Collor
    Color IconCDColor = new Color32(217, 217, 217, 80);

    //Image SKill Icon Able To use Collor
    Color IconNormalColor = new Color32(217, 217, 217, 255);


    //Skills sounds
    AudioSource audioSource;
    PlayerAnimController playerAnimController;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimController = GetComponent<PlayerAnimController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Hidden Wall
        if (hiddenWallCountDownUI >= 1)
        {
            hiddenWallCountDownUI -= Time.deltaTime;
            TimeSpan CDtime = TimeSpan.FromSeconds(hiddenWallCountDownUI);
            hiddenWallCDText.text = CDtime.ToString(@"ss");
        }
        else
        {
            hiddenWallSkillIcon.color = IconNormalColor;
            hiddenWallCDText.text = "";
            hiddenWallCountDownUI = 0;
        }

        //Turn Into Box
        if (turnIntoBoxCountDownUI >= 1)
        {
            turnIntoBoxCountDownUI -= Time.deltaTime;
            TimeSpan CDtime = TimeSpan.FromSeconds(turnIntoBoxCountDownUI);
            turnIntoBoxCDText.text = CDtime.ToString(@"ss");
        }
        else
        {
            turnIntoBoxSkillIcon.color = IconNormalColor;
            turnIntoBoxCDText.text = "";
            turnIntoBoxCountDownUI = 0;
        }


        //Whistle
        if (whistleCountDownUI >= 1)
        {
            whistleCountDownUI -= Time.deltaTime;
            TimeSpan CDtime = TimeSpan.FromSeconds(whistleCountDownUI);
            whistleCDText.text = CDtime.ToString(@"ss");
        }
        else
        {
            whistleSkillIcon.color = IconNormalColor;
            whistleCDText.text = "";
            whistleCountDownUI = 0;
        }

    }

    #region Wall Hack
    public void ShowBehindWalls()
    {
        if (hiddenWallCountDownUI > 0)
            return;

        var walls = GameObject.FindGameObjectsWithTag("HiddenWalls");

        foreach (GameObject wall in walls)
        {
            solidWallMaterial = wall.GetComponent<Renderer>().material;
            wall.GetComponent<Renderer>().material = hiddenWallMaterial;
        }
        StartCoroutine(HideWallsAgain(5));
    }
    IEnumerator HideWallsAgain(int secs)
    {
        yield return new WaitForSeconds(secs);

        var walls = GameObject.FindGameObjectsWithTag("HiddenWalls");
        foreach (GameObject wall in walls)
        {
            wall.GetComponent<Renderer>().material = solidWallMaterial;
        }

        hiddenWallSkillIcon.color = IconCDColor;
        hiddenWallCountDownUI = showHiddenWallCD;

    }

    #endregion

    #region Transform Into Box
    public void onTransformIntoBox(Collider collider, Rigidbody rb)
    {
        if (turnIntoBoxCountDownUI > 0)
            return;


        collider.enabled = false;
        rb.isKinematic = true;
        foreach (GameObject model in playerModels)
            model.SetActive(false);

        Instantiate(puffEffect, transform);
        instantiatedBoxModel = Instantiate(boxModel, transform);
    }
    public void GoBackToHumanForm(Collider collider, Rigidbody rb)
    {
        if (turnIntoBoxCountDownUI > 0)
            return;

        Destroy(instantiatedBoxModel);

        collider.enabled = true;
        rb.isKinematic = false;

        Instantiate(puffEffect, transform);

        foreach (GameObject model in playerModels)
            model.SetActive(true);


        turnIntoBoxSkillIcon.color = IconCDColor;
        turnIntoBoxCountDownUI = turnIntoBoxCD;
    }
    #endregion

    #region Hide
    public void OnInteractWithStraw(GameObject straw)
    {
        var StrawVFXPosition = straw.gameObject.transform.GetChild(0);
        Instantiate(strawMoveEffect, StrawVFXPosition.transform);
        audioSource.PlayOneShot(onEnterLeaveStrawSFX);
    }
    #endregion

    #region Whistle

    public void OnWhistle()
    {
        if (whistleCountDownUI > 0)
            return;
        Instantiate(whistleEffect, transform);

        GameObject soundPosition = Instantiate(lastKnowPosition, transform.position, Quaternion.identity);
        Instantiate(soundPosition, soundPosition.transform, true);

        audioSource.PlayOneShot(whistleSound);
        playerAnimController.Whistle();

        whistleSkillIcon.color = IconCDColor;
        whistleCountDownUI = whistleCD; ;

    }


    #endregion
}
