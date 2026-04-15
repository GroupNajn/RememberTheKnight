using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SwitchBodyParts : MonoBehaviour
{
    UIManager uiManager;

    [SerializeField] private Gender currentGender = Gender.Male;
    public enum Gender
    {
        Male,
        Female
    }

    [SerializeField] Transform allGenderParts;
    [SerializeField] Transform maleParts;
    [SerializeField] Transform femaleParts;

    [Header("HEAD")]
    [SerializeField] List<GameObject> head = new();

    [Header("PADS")]
    [SerializeField] List<GameObject> rightShoulder = new();
    [SerializeField] List<GameObject> leftShoulder = new();
    [SerializeField] List<GameObject> rightElbow = new();
    [SerializeField] List<GameObject> leftElbow = new();

    [Header("BODY")]
    [SerializeField] List<GameObject> torso = new();
    [SerializeField] List<GameObject> leftUppperArm = new();
    [SerializeField] List<GameObject> rightUppperArm = new();
    [SerializeField] List<GameObject> leftLowerArm = new();
    [SerializeField] List<GameObject> rightLowerArm = new();
    [SerializeField] List<GameObject> leftHand = new();
    [SerializeField] List<GameObject> rightHand = new();

    [Header("LEGS")]
    [SerializeField] List<GameObject> hips = new();
    [SerializeField] List<GameObject> leftKnee = new();
    [SerializeField] List<GameObject> rightKnee = new();
    [SerializeField] List<GameObject> leftLeg = new();
    [SerializeField] List<GameObject> rightLeg = new();

    [HideInInspector] public int currentHead;

    [HideInInspector] public int currentLeftShoulder;
    [HideInInspector] public int currentRightShoulder;

    [HideInInspector] public int currentLeftElbow;
    [HideInInspector] public int currentRightElbow;

    [HideInInspector] public int currentTorso;
    [HideInInspector] public int currentLeftUppperArm;
    [HideInInspector] public int currentRightUppperArm;
    [HideInInspector] public int currentLeftLowerArm;
    [HideInInspector] public int currentRightLowerArm;
    [HideInInspector] public int currentLeftHand;
    [HideInInspector] public int currentRightHand;

    [HideInInspector] public int currentHips;
    [HideInInspector] public int currentLeftKnee;
    [HideInInspector] public int currentRightKnee;
    [HideInInspector] public int currentLeftLeg;
    [HideInInspector] public int currentRightLeg;

    [Header("SAVED")]
    [SerializeField] private int currentSavedHead;

    [SerializeField] private int currentSavedLeftShoulder;
    [SerializeField] private int currentSavedRightShoulder;

    [SerializeField] public int currentSavedLeftElbow;
    [SerializeField] public int currentSavedRightElbow;

    [SerializeField] private int currentSavedTorso;
    [SerializeField] private int currentSavedLeftUppperArm;
    [SerializeField] private int currentSavedRightUppperArm;
    [SerializeField] private int currentSavedLeftLowerArm;
    [SerializeField] private int currentSavedRightLowerArm;
    [SerializeField] private int currentSavedLeftHand;
    [SerializeField] private int currentSavedRightHand;

    [SerializeField] private int currentSavedHips;
    [SerializeField] private int currentSavedLeftKnee;
    [SerializeField] private int currentSavedRightKnee;
    [SerializeField] private int currentSavedLeftLeg;
    [SerializeField] private int currentSavedRightLeg;

    [HideInInspector] public bool hasSaved = false;

    private void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        FindRoots();
        RebuildParts();
    }

    private void FindRoots()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        foreach (Transform child in player.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == "All_Gender_Parts") allGenderParts = child;
            if (child.name == "Male_Parts") maleParts = child;
            if (child.name == "Female_Parts") femaleParts = child;
        }
    }

    Transform FindDeep(Transform root, string name)
    {
        if (root.name == name) return root;

        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
            if (child.name == name) return child;

        return null;
    }

    List<GameObject> GetChildren(Transform parent)
    {
        var list = new List<GameObject>();

        if (parent == null)
            return list;

        foreach (Transform child in parent)
        {
            list.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
        return list;
    }

    void RebuildParts()
    {
        Transform gender = currentGender == Gender.Male ? maleParts : femaleParts;
        string genderPrefix = currentGender == Gender.Male ? "Male" : "Female";

        head = GetChildren(gender.Find($"{genderPrefix}_00_Head/{genderPrefix}_Head_No_Elements"));
        torso = GetChildren(gender.Find($"{genderPrefix}_03_Torso"));
        rightUppperArm = GetChildren(gender.Find($"{genderPrefix}_04_Arm_Upper_Right"));
        leftUppperArm = GetChildren(gender.Find($"{genderPrefix}_05_Arm_Upper_Left"));
        rightLowerArm = GetChildren(gender.Find($"{genderPrefix}_06_Arm_Lower_Right"));
        leftLowerArm = GetChildren(gender.Find($"{genderPrefix}_07_Arm_Lower_Left"));
        rightHand = GetChildren(gender.Find($"{genderPrefix}_08_Hand_Right"));
        leftHand = GetChildren(gender.Find($"{genderPrefix}_09_Hand_Left"));
        hips = GetChildren(gender.Find($"{genderPrefix}_10_Hips"));
        rightLeg = GetChildren(gender.Find($"{genderPrefix}_11_Leg_Right"));
        leftLeg = GetChildren(gender.Find($"{genderPrefix}_12_Leg_Left"));

        rightShoulder = GetChildren(allGenderParts.Find("All_05_Shoulder_Attachment_Right"));
        leftShoulder = GetChildren(allGenderParts.Find("All_06_Shoulder_Attachment_Left"));
        rightElbow = GetChildren(allGenderParts.Find("All_07_Elbow_Attachment_Right"));
        leftElbow = GetChildren(allGenderParts.Find("All_08_Elbow_Attachment_Left"));
        rightKnee = GetChildren(allGenderParts.Find("All_10_Knee_Attachement_Right"));
        leftKnee = GetChildren(allGenderParts.Find("All_11_Knee_Attachement_Left"));

        ResetIndex();
        ActivateStartingBody();
    }

    void ResetIndex()
    {
        currentHead = 0;
        currentLeftShoulder = 0;
        currentRightShoulder = 0;
        currentLeftElbow = 0;
        currentRightElbow = 0;
        currentTorso = 0;
        currentLeftUppperArm = 0;
        currentRightUppperArm = 0;
        currentLeftLowerArm = 0;
        currentRightLowerArm = 0;
        currentLeftHand = 0;
        currentRightHand = 0;
        currentHips = 0;
        currentLeftKnee = 0;
        currentRightKnee = 0;
        currentLeftLeg = 0;
        currentRightLeg = 0;
    }

    void ActivateStartingBody()
    {
        TryActivate(head, 0);
        TryActivate(rightShoulder, 0);
        TryActivate(leftShoulder, 0);
        TryActivate(rightElbow, 0);
        TryActivate(leftElbow, 0);
        TryActivate(torso, 0);
        TryActivate(rightUppperArm, 0);
        TryActivate(leftUppperArm, 0);
        TryActivate(rightLowerArm, 0);
        TryActivate(leftLowerArm, 0);
        TryActivate(rightHand, 0);
        TryActivate(leftHand, 0);
        TryActivate(hips, 0);
        TryActivate(rightKnee, 0);
        TryActivate(leftKnee, 0);
        TryActivate(rightLeg, 0);
        TryActivate(leftLeg, 0);
    }

    void ActivatCurrentBody()
    {
        TryActivate(head, currentHead);
        TryActivate(rightShoulder, currentRightShoulder);
        TryActivate(leftShoulder, currentLeftShoulder);
        TryActivate(rightElbow, currentRightElbow);
        TryActivate(leftElbow, currentLeftElbow);
        TryActivate(torso, currentTorso);
        TryActivate(rightUppperArm, currentRightUppperArm);
        TryActivate(leftUppperArm, currentLeftUppperArm);
        TryActivate(rightLowerArm, currentRightLowerArm);
        TryActivate(leftLowerArm, currentLeftLowerArm);
        TryActivate(rightHand, currentRightHand);
        TryActivate(leftHand, currentLeftHand);
        TryActivate(hips, currentHips);
        TryActivate(rightKnee, currentRightKnee);
        TryActivate(leftKnee, currentLeftKnee);
        TryActivate(rightLeg, currentRightLeg);
        TryActivate(leftLeg, currentLeftLeg);
    }

    void ActivateSavedBody()
    {
        TryActivate(head, currentSavedHead);
        TryActivate(rightShoulder, currentSavedRightShoulder);
        TryActivate(leftShoulder, currentSavedLeftShoulder);
        TryActivate(rightElbow, currentSavedRightElbow);
        TryActivate(leftElbow, currentSavedLeftElbow);
        TryActivate(torso, currentSavedTorso);
        TryActivate(rightUppperArm, currentSavedRightUppperArm);
        TryActivate(leftUppperArm, currentSavedLeftUppperArm);
        TryActivate(rightLowerArm, currentSavedRightLowerArm);
        TryActivate(leftLowerArm, currentSavedLeftLowerArm);
        TryActivate(rightHand, currentSavedRightHand);
        TryActivate(leftHand, currentSavedLeftHand);
        TryActivate(hips, currentSavedHips);
        TryActivate(rightKnee, currentSavedRightKnee);
        TryActivate(leftKnee, currentSavedLeftKnee);
        TryActivate(rightLeg, currentSavedRightLeg);
        TryActivate(leftLeg, currentSavedLeftLeg);

    }

    void TryActivate(List<GameObject> list, int index)
    {
        if (list.Count > index)
            list[index].SetActive(true);
    }

    void SwitchPart(List<GameObject> list, ref int index, int direction)
    {
        if (list.Count == 0) return;

        list[index].SetActive(false);

        index = (index + direction + list.Count) % list.Count;

        list[index].SetActive(true);
    }

    public void SwitchGender()
    {
        currentGender = currentGender == Gender.Male ? Gender.Female : Gender.Male;

        maleParts.gameObject.SetActive(currentGender == Gender.Male);
        femaleParts.gameObject.SetActive(currentGender == Gender.Female);
        RebuildParts();

    }

    public void SaveBody()
    {
        currentSavedHead = currentHead;
        currentSavedLeftShoulder = currentLeftShoulder;
        currentSavedRightShoulder = currentRightShoulder;
        currentSavedLeftElbow = currentLeftElbow;
        currentSavedRightElbow = currentRightElbow;
        currentSavedTorso = currentTorso;
        currentSavedLeftUppperArm = currentLeftUppperArm;
        currentSavedRightUppperArm = currentRightUppperArm;
        currentSavedLeftLowerArm = currentLeftLowerArm;
        currentSavedRightLowerArm = currentRightLowerArm;
        currentSavedLeftHand = currentLeftHand;
        currentSavedRightHand = currentRightHand;
        currentSavedHips = currentHips;
        currentSavedLeftKnee = currentLeftKnee;
        currentSavedRightKnee = currentRightKnee;
        currentSavedLeftLeg = currentLeftLeg;
        currentSavedRightLeg = currentRightLeg;

        hasSaved = true;

    }

    public void LoadBody()
    {
        TryDeactivateCurrentBody();

        currentHead = currentSavedHead;
        currentRightShoulder = currentSavedRightShoulder;
        currentLeftShoulder = currentSavedLeftShoulder;
        currentRightElbow = currentSavedRightElbow;
        currentLeftElbow = currentSavedLeftElbow;
        currentTorso = currentSavedTorso;
        currentRightUppperArm = currentSavedRightUppperArm;
        currentLeftUppperArm = currentSavedLeftUppperArm;
        currentRightLowerArm = currentSavedRightLowerArm;
        currentLeftLowerArm = currentSavedLeftLowerArm;
        currentRightHand = currentSavedRightHand;
        currentLeftHand = currentSavedLeftHand;
        currentHips = currentSavedHips;
        currentRightKnee = currentSavedRightKnee;
        currentLeftKnee = currentSavedLeftKnee;
        currentRightLeg = currentSavedRightLeg;
        currentLeftLeg = currentSavedLeftLeg;

        ActivateSavedBody();

    }

    public void RandomizeBody()
    {
        TryDeactivateCurrentBody();

        currentHead = UnityEngine.Random.Range(0, head.Count);

        int shoulderIndex = UnityEngine.Random.Range(0, rightShoulder.Count);
        currentRightShoulder = shoulderIndex;
        currentLeftShoulder = shoulderIndex;

        int elbowIndex = UnityEngine.Random.Range(0, rightElbow.Count);
        currentRightElbow = elbowIndex;
        currentLeftElbow = elbowIndex;

        currentTorso = UnityEngine.Random.Range(0, torso.Count);

        int upperArmIndex = UnityEngine.Random.Range(0, rightUppperArm.Count);
        currentRightUppperArm = upperArmIndex;
        currentLeftUppperArm = upperArmIndex;

        int lowerArmIndex = UnityEngine.Random.Range(0, rightLowerArm.Count);
        currentRightLowerArm = lowerArmIndex;
        currentLeftLowerArm = lowerArmIndex;

        int handIndex = UnityEngine.Random.Range(0, rightHand.Count);
        currentRightHand = handIndex;
        currentLeftHand = handIndex;

        currentHips = UnityEngine.Random.Range(0, hips.Count);

        int kneeIndex = UnityEngine.Random.Range(0, rightKnee.Count);
        currentRightKnee = kneeIndex;
        currentLeftKnee = kneeIndex;

        int legIndex = UnityEngine.Random.Range(0, rightLeg.Count);
        currentRightLeg = legIndex;
        currentLeftLeg = legIndex;


        ActivatCurrentBody();
    }

    void TryDeactivateCurrentBody()
    {
        TryDeactivate(head, currentHead);
        TryDeactivate(leftShoulder, currentLeftShoulder);
        TryDeactivate(rightShoulder, currentRightShoulder);
        TryDeactivate(leftElbow, currentLeftElbow);
        TryDeactivate(rightElbow, currentRightElbow);
        TryDeactivate(torso, currentTorso);
        TryDeactivate(leftUppperArm, currentLeftUppperArm);
        TryDeactivate(rightUppperArm, currentRightUppperArm);
        TryDeactivate(leftLowerArm, currentLeftLowerArm);
        TryDeactivate(rightLowerArm, currentRightLowerArm);
        TryDeactivate(leftHand, currentLeftHand);
        TryDeactivate(rightHand, currentRightHand);
        TryDeactivate(hips, currentHips);
        TryDeactivate(leftKnee, currentLeftKnee);
        TryDeactivate(rightKnee, currentRightKnee);
        TryDeactivate(leftLeg, currentLeftLeg);
        TryDeactivate(rightLeg, currentRightLeg);
    }

    void TryDeactivate(List<GameObject> list, int index)
    {
        if (list.Count > index)
            list[index].SetActive(false);
    }

    //NEXT
    public void NextHead() => SwitchPart(head, ref currentHead, 1);
    public void NextTorso() => SwitchPart(torso, ref currentTorso, 1);
    public void NextHips() => SwitchPart(hips, ref currentHips, 1);
    public void NextShoulders()
    {
        SwitchPart(rightShoulder, ref currentRightShoulder, 1);
        SwitchPart(leftShoulder, ref currentLeftShoulder, 1);
    }
    public void NextElbows()
    {
        SwitchPart(rightElbow, ref currentRightElbow, 1);
        SwitchPart(leftElbow, ref currentLeftElbow, 1);
    }
    public void NextUpperArms()
    {
        SwitchPart(rightUppperArm, ref currentRightUppperArm, 1); SwitchPart(leftUppperArm, ref currentLeftUppperArm, 1);
    }
    public void NextLowerArms()
    {
        SwitchPart(rightLowerArm, ref currentRightLowerArm, 1);
        SwitchPart(leftLowerArm, ref currentLeftLowerArm, 1);
    }
    public void NextHands()
    {
        SwitchPart(rightHand, ref currentRightHand, 1);
        SwitchPart(leftHand, ref currentLeftHand, 1);
    }
    public void NextKnees()
    {
        SwitchPart(rightKnee, ref currentRightKnee, 1);
        SwitchPart(leftKnee, ref currentLeftKnee, 1);
    }
    public void NextLegs()
    {
        SwitchPart(rightLeg, ref currentRightLeg, 1);
        SwitchPart(leftLeg, ref currentLeftLeg, 1);
    }

    // PREVIOUS
    public void PrevHead() => SwitchPart(head, ref currentHead, -1);
    public void PrevTorso() => SwitchPart(torso, ref currentTorso, -1);
    public void PrevHips() => SwitchPart(hips, ref currentHips, -1);
    public void PrevShoulders()
    {
        SwitchPart(rightShoulder, ref currentRightShoulder, -1);
        SwitchPart(leftShoulder, ref currentLeftShoulder, -1);
    }
    public void PrevElbows()
    {
        SwitchPart(rightElbow, ref currentRightElbow, -1);
        SwitchPart(leftElbow, ref currentLeftElbow, -1);
    }
    public void PrevUpperArms()
    {
        SwitchPart(rightUppperArm, ref currentRightUppperArm, -1);
        SwitchPart(leftUppperArm, ref currentLeftUppperArm, -1);
    }
    public void PrevLowerArms()
    {
        SwitchPart(rightLowerArm, ref currentRightLowerArm, -1);
        SwitchPart(leftLowerArm, ref currentLeftLowerArm, -1);
    }
    public void PrevHands()
    {
        SwitchPart(rightHand, ref currentRightHand, -1);
        SwitchPart(leftHand, ref currentLeftHand, -1);
    }
    public void PrevKnees()
    {
        SwitchPart(rightKnee, ref currentRightKnee, -1);
        SwitchPart(leftKnee, ref currentLeftKnee, -1);
    }
    public void PrevLegs()
    {
        SwitchPart(rightLeg, ref currentRightLeg, -1);
        SwitchPart(leftLeg, ref currentLeftLeg, -1);
    }

    public void StartGame()
    {
        uiManager.CloseCharacterSelectUI();
        uiManager.UIMenuActive = false;
        uiManager.CheckUIState();

        gameObject.SetActive(false);

        // OPENS ALL UI THAT NEED TO SHOW DURING GAMEPLAY
        uiManager.OpenSoulUI();
        uiManager.ShowPlayerBars();

        // LOAD NEXT SCENE
        GlobalSceneManager.Instance.ActivateSceneTransition(SceneData.Instance[2]);


    }

    public void OnClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}


