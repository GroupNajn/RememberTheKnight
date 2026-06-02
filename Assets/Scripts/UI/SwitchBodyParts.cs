using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
// This script manages the character customization.
// This is done throuh methods that changes, save or load the current index of a list of body parts. 
// Each body part has its own list of gameobjects, that are active or not dependig on the current index.
// All methods with "Next" or "Prev" in their name, changes the current index of a list and when at the end of the list, it goes back to the start and vice versa.
// The other methods are helper methods to find refrences, activate/deactivete body parts in a safe maner or save/load indexes for playerPrefs.
/// </summary>
public class SwitchBodyParts : MonoBehaviour
{
    /// <summary>
    /// Changed by Anton 2026-06-01
    /// Added logic to save and load character customization using PlayerPrefs, 
    /// allowing players to retain their chosen appearance across game sessions.
    /// </summary>

    UIManager uiManager;
    private GameData gameData;

    [SerializeField] private Gender currentGender = Gender.Male;
    public enum Gender
    {
        Male,
        Female
    }

    #region Body Parts
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
    [SerializeField] List<GameObject> leftUpperArm = new();
    [SerializeField] List<GameObject> rightUpperArm = new();
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
    [HideInInspector] public int currentLeftUpperArm;
    [HideInInspector] public int currentRightUpperArm;
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
    [SerializeField] private int currentSavedLeftUpperArm;
    [SerializeField] private int currentSavedRightUpperArm;
    [SerializeField] private int currentSavedLeftLowerArm;
    [SerializeField] private int currentSavedRightLowerArm;
    [SerializeField] private int currentSavedLeftHand;
    [SerializeField] private int currentSavedRightHand;

    [SerializeField] private int currentSavedHips;
    [SerializeField] private int currentSavedLeftKnee;
    [SerializeField] private int currentSavedRightKnee;
    [SerializeField] private int currentSavedLeftLeg;
    [SerializeField] private int currentSavedRightLeg;
    #endregion

    [HideInInspector] public bool hasSaved = false;

    bool canStartGame = false;
    bool startGame = false;

    private void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        gameData = FindFirstObjectByType<GameData>();

        Event_System.instance.OnLoadScenes += OnLoadScenes;
    }

    private void Start()
    {
        FindRoots();

        if (PlayerPrefsSaveSystem.HasPlayedGame())
            currentGender = (Gender)PlayerPrefsSaveSystem.GetSavedGender();

        maleParts.gameObject.SetActive(currentGender == Gender.Male);
        femaleParts.gameObject.SetActive(currentGender == Gender.Female);

        RebuildParts();

        if (PlayerPrefsSaveSystem.HasPlayedGame())
            LoadSavedCustomization();
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
        rightUpperArm = GetChildren(gender.Find($"{genderPrefix}_04_Arm_Upper_Right"));
        leftUpperArm = GetChildren(gender.Find($"{genderPrefix}_05_Arm_Upper_Left"));
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
        currentLeftUpperArm = 0;
        currentRightUpperArm = 0;
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
        TryActivate(rightUpperArm, 0);
        TryActivate(leftUpperArm, 0);
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

    void ActivateCurrentBody()
    {
        TryActivate(head, currentHead);
        TryActivate(rightShoulder, currentRightShoulder);
        TryActivate(leftShoulder, currentLeftShoulder);
        TryActivate(rightElbow, currentRightElbow);
        TryActivate(leftElbow, currentLeftElbow);
        TryActivate(torso, currentTorso);
        TryActivate(rightUpperArm, currentRightUpperArm);
        TryActivate(leftUpperArm, currentLeftUpperArm);
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
        TryActivate(rightUpperArm, currentSavedRightUpperArm);
        TryActivate(leftUpperArm, currentSavedLeftUpperArm);
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

        SaveBody();
        RebuildParts();
        LoadSavedCustomization();
    }

    public void SaveBody()
    {
        //currentSavedHead = currentHead;
        //currentSavedLeftShoulder = currentLeftShoulder;
        //currentSavedRightShoulder = currentRightShoulder;
        //currentSavedLeftElbow = currentLeftElbow;
        //currentSavedRightElbow = currentRightElbow;
        //currentSavedTorso = currentTorso;
        //currentSavedLeftUppperArm = currentLeftUppperArm;
        //currentSavedRightUppperArm = currentRightUppperArm;
        //currentSavedLeftLowerArm = currentLeftLowerArm;
        //currentSavedRightLowerArm = currentRightLowerArm;
        //currentSavedLeftHand = currentLeftHand;
        //currentSavedRightHand = currentRightHand;
        //currentSavedHips = currentHips;
        //currentSavedLeftKnee = currentLeftKnee;
        //currentSavedRightKnee = currentRightKnee;
        //currentSavedLeftLeg = currentLeftLeg;
        //currentSavedRightLeg = currentRightLeg;

        PlayerPrefsSaveSystem.SaveCharactedCustomization("Head", currentHead);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("RightShoulder", currentRightShoulder);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("LeftShoulder", currentLeftShoulder);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("RightElbow", currentRightElbow);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("LeftElbow", currentLeftElbow);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("Torso", currentTorso);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("RightUpperArm", currentRightUpperArm);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("LeftUpperArm", currentLeftUpperArm);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("RightLowerArm", currentRightLowerArm);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("LeftLowerArm", currentLeftLowerArm);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("RightHand", currentRightHand);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("LeftHand", currentLeftHand);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("Hips", currentHips);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("RightKnee", currentRightKnee);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("LeftKnee", currentLeftKnee);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("RightLeg", currentRightLeg);
        PlayerPrefsSaveSystem.SaveCharactedCustomization("LeftLeg", currentLeftLeg);

        hasSaved = true;
    }

    public void LoadSavedCustomization()
    {
        TryDeactivateCurrentBody();

        currentHead = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("Head");
        currentRightShoulder = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("RightShoulder");
        currentLeftShoulder = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("LeftShoulder");
        currentRightElbow = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("RightElbow");
        currentLeftElbow = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("LeftElbow");
        currentTorso = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("Torso");
        currentRightUpperArm = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("RightUpperArm");
        currentLeftUpperArm = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("LeftUpperArm");
        currentRightLowerArm = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("RightLowerArm");
        currentLeftLowerArm = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("LeftLowerArm");
        currentRightHand = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("RightHand"    );
        currentLeftHand = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("LeftHand");
        currentHips = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("Hips");
        currentRightKnee = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("RightKnee");
        currentLeftKnee = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("LeftKnee");
        currentRightLeg = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("RightLeg");
        currentLeftLeg = PlayerPrefsSaveSystem.GetSavedCharacterCustomization("LeftLeg");

        ActivateCurrentBody();
    }

    //public void LoadBody()
    //{
    //    TryDeactivateCurrentBody();

    //    currentHead = currentSavedHead;
    //    currentRightShoulder = currentSavedRightShoulder;
    //    currentLeftShoulder = currentSavedLeftShoulder;
    //    currentRightElbow = currentSavedRightElbow;
    //    currentLeftElbow = currentSavedLeftElbow;
    //    currentTorso = currentSavedTorso;
    //    currentRightUpperArm = currentSavedRightUpperArm;
    //    currentLeftUpperArm = currentSavedLeftUpperArm;
    //    currentRightLowerArm = currentSavedRightLowerArm;
    //    currentLeftLowerArm = currentSavedLeftLowerArm;
    //    currentRightHand = currentSavedRightHand;
    //    currentLeftHand = currentSavedLeftHand;
    //    currentHips = currentSavedHips;
    //    currentRightKnee = currentSavedRightKnee;
    //    currentLeftKnee = currentSavedLeftKnee;
    //    currentRightLeg = currentSavedRightLeg;
    //    currentLeftLeg = currentSavedLeftLeg;

    //    ActivateSavedBody();
    //}

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

        int upperArmIndex = UnityEngine.Random.Range(0, rightUpperArm.Count);
        currentRightUpperArm = upperArmIndex;
        currentLeftUpperArm = upperArmIndex;

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


        ActivateCurrentBody();
    }

    void TryDeactivateCurrentBody()
    {
        TryDeactivate(head, currentHead);
        TryDeactivate(leftShoulder, currentLeftShoulder);
        TryDeactivate(rightShoulder, currentRightShoulder);
        TryDeactivate(leftElbow, currentLeftElbow);
        TryDeactivate(rightElbow, currentRightElbow);
        TryDeactivate(torso, currentTorso);
        TryDeactivate(leftUpperArm, currentLeftUpperArm);
        TryDeactivate(rightUpperArm, currentRightUpperArm);
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
        SwitchPart(rightUpperArm, ref currentRightUpperArm, 1); SwitchPart(leftUpperArm, ref currentLeftUpperArm, 1);
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
        SwitchPart(rightUpperArm, ref currentRightUpperArm, -1);
        SwitchPart(leftUpperArm, ref currentLeftUpperArm, -1);
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

    private void OnLoadScenes()
    {
        if (SceneManager.GetActiveScene().name == SceneData.Instance[1])
        {
            canStartGame = false;
            startGame = false;
            Event_System.instance.OnSceneTransitionDone += OnBlackFadeDone;
        }
        //else if (SceneManager.GetActiveScene().name == SceneData.Instance[2])
        //{
        //    uiManager.UIMenuActive = false;
        //    uiManager.CheckUIState();

        //    // OPENS ALL UI THAT NEED TO SHOW DURING GAMEPLAY
        //    uiManager.OpenUIOnMenuClose();
        //}
    }

    private void OnBlackFadeDone()
    {
        canStartGame = true;

        if (startGame)
        {
            if (!PlayerPrefsSaveSystem.HasPlayedGame())
            {
                gameData.FirstTimePlaying = false;
                PlayerPrefsSaveSystem.SetSaveState("HasPlayedGame");

                GlobalSceneManager.Instance.ActivateSceneTransition(SceneData.Instance[7]);
            }
            else
                GlobalSceneManager.Instance.ActivateSceneTransition(SceneData.Instance[2]);
        }

        Event_System.instance.OnSceneTransitionDone -= OnBlackFadeDone;
    }

    public void StartGame()
    {
        uiManager.CloseCharacterSelectUI();
        SaveBody();
        PlayerPrefsSaveSystem.SaveGender(currentGender);

        // LOAD NEXT SCENE
        if (canStartGame)
        {
            if (!PlayerPrefsSaveSystem.HasPlayedGame())
            {
                gameData.FirstTimePlaying = false;
                PlayerPrefsSaveSystem.SetSaveState("HasPlayedGame");

                GlobalSceneManager.Instance.ActivateSceneTransition(SceneData.Instance[7]);
            }
            else
                GlobalSceneManager.Instance.ActivateSceneTransition(SceneData.Instance[2]);
        }
        else
        {
            startGame = true;
        }

    }

    public void OnClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}