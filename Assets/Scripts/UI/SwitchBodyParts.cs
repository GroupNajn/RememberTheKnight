using System.Collections.Generic;
using UnityEngine;


public class SwitchBodyParts : MonoBehaviour
{
    [SerializeField] private Gender currentGender = Gender.Male;
    [SerializeField] GameObject MaleObject;
    [SerializeField] GameObject FemaleObject;

    // ACTIVE PARENTS
    private Transform headParent;

    private Transform leftShoulderParent;
    private Transform rightShoulderParent;

    private Transform leftElbowParent;
    private Transform rightElbowParent;

    private Transform torsoParent;
    private Transform leftUppperArmParent;
    private Transform rightUppperArmParent;
    private Transform leftLowerArmParent;
    private Transform rightLowerArmParent;
    private Transform leftHandParent;
    private Transform rightHandParent;

    private Transform hipsParent;
    private Transform leftKneeParent;
    private Transform rightKneeParent;
    private Transform leftLegParent;
    private Transform rightLegParent;

    [Header("General Parents")]
    [SerializeField] private Transform generalLeftShoulderParent;
    [SerializeField] private Transform generalRightShoulderParent;

    [SerializeField] private Transform generalLeftElbowParent;
    [SerializeField] private Transform generalRightElbowParent;

    [SerializeField] private Transform generalLeftKneeParent;
    [SerializeField] private Transform generalRightKneeParent;

    [Header("Male Parents")]
    [SerializeField] private Transform maleHeadParent;

    [SerializeField] private Transform maleTorsoParent;
    [SerializeField] private Transform maleLeftUppperArmParent;
    [SerializeField] private Transform maleRightUppperArmParent;
    [SerializeField] private Transform maleLeftLowerArmParent;
    [SerializeField] private Transform maleRightLowerArmParent;
    [SerializeField] private Transform maleLeftHandParent;
    [SerializeField] private Transform maleRightHandParent;

    [SerializeField] private Transform maleHipsParent;
    [SerializeField] private Transform maleLeftLegParent;
    [SerializeField] private Transform maleRightLegParent;

    [Header("Female Parents")]
    [SerializeField] private Transform femaleHeadParent;

    [SerializeField] private Transform femaleTorsoParent;
    [SerializeField] private Transform femaleLeftUppperArmParent;
    [SerializeField] private Transform femaleRightUppperArmParent;
    [SerializeField] private Transform femaleLeftLowerArmParent;
    [SerializeField] private Transform femaleRightLowerArmParent;
    [SerializeField] private Transform femaleLeftHandParent;
    [SerializeField] private Transform femaleRightHandParent;

    [SerializeField] private Transform femaleHipsParent;
    [SerializeField] private Transform femaleLeftLegParent;
    [SerializeField] private Transform femaleRightLegParent;

    [Header("HEAD")]
    [SerializeField] List<GameObject> head = new List<GameObject>();

    [Header("PADS")]
    [SerializeField] List<GameObject> rightShoulder = new List<GameObject>();
    [SerializeField] List<GameObject> leftShoulder = new List<GameObject>();
    [SerializeField] List<GameObject> rightElbow = new List<GameObject>();
    [SerializeField] List<GameObject> leftElbow = new List<GameObject>();

    [Header("BODY")]
    [SerializeField] List<GameObject> torso = new List<GameObject>();
    [SerializeField] List<GameObject> leftUppperArm = new List<GameObject>();
    [SerializeField] List<GameObject> rightUppperArm = new List<GameObject>();
    [SerializeField] List<GameObject> leftLowerArm = new List<GameObject>();
    [SerializeField] List<GameObject> rightLowerArm = new List<GameObject>();
    [SerializeField] List<GameObject> leftHand = new List<GameObject>();
    [SerializeField] List<GameObject> rightHand = new List<GameObject>();

    [Header("LEGS")]
    [SerializeField] List<GameObject> hips = new List<GameObject>();
    [SerializeField] List<GameObject> leftKnee = new List<GameObject>();
    [SerializeField] List<GameObject> rightKnee = new List<GameObject>();
    [SerializeField] List<GameObject> leftLeg = new List<GameObject>();
    [SerializeField] List<GameObject> rightLeg = new List<GameObject>();


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

    public enum Gender
    {
        Male,
        Female
    }


    private void Awake()
    {
        ApplyGenderParents();
        RebuildParts();
    }

    void ApplyGenderParents()
    {
        leftShoulderParent = generalLeftShoulderParent;
        rightShoulderParent = generalRightShoulderParent;

        leftElbowParent = generalLeftElbowParent;
        rightElbowParent = generalRightElbowParent;

        leftKneeParent = generalLeftKneeParent;
        rightKneeParent = generalRightKneeParent;

        if (currentGender == Gender.Male)
        {
            headParent = maleHeadParent;

            torsoParent = maleTorsoParent;
            leftUppperArmParent = maleLeftUppperArmParent;
            rightUppperArmParent = maleRightUppperArmParent;
            leftLowerArmParent = maleLeftLowerArmParent;
            rightLowerArmParent = maleRightLowerArmParent;
            leftHandParent = maleLeftHandParent;
            rightHandParent = maleRightHandParent;

            hipsParent = maleHipsParent;
            leftLegParent = maleLeftLegParent;
            rightLegParent = maleRightLegParent;
        }
        else
        {
            headParent = femaleHeadParent;

            torsoParent = femaleTorsoParent;
            leftUppperArmParent = femaleLeftUppperArmParent;
            rightUppperArmParent = femaleRightUppperArmParent;
            leftLowerArmParent = femaleLeftLowerArmParent;
            rightLowerArmParent = femaleRightLowerArmParent;
            leftHandParent = femaleLeftHandParent;
            rightHandParent = femaleRightHandParent;

            hipsParent = femaleHipsParent;
            leftLegParent = femaleLeftLegParent;
            rightLegParent = femaleRightLegParent;

        }
    }

    public void SwitchGender()
    {
        currentGender = currentGender == Gender.Male ? Gender.Female : Gender.Male;

        ApplyGenderParents();
        SetRootActive();
        RebuildParts();

    }

    List<GameObject> GetChildren(Transform parent)
    {
        List<GameObject> list = new List<GameObject>();

        foreach (Transform child in parent)
        {
            list.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        return list;
    }

    void SwitchPart(List<GameObject> list, ref int index, int direction)
    {
        if (list.Count == 0) return;

        list[index].SetActive(false);

        index += direction;
        if (index < 0) index = list.Count - 1;
        if (index >= list.Count) index = 0;

        list[index].SetActive(true);
    }

    //next item
    public void NextHead() => SwitchPart(head, ref currentHead, 1);
    public void NextTorso()
    {
        SwitchPart(torso, ref currentTorso, 1);
    }

    public void NextShoulders()
    {
        SwitchPart(leftShoulder, ref currentLeftShoulder, 1);
        SwitchPart(rightShoulder, ref currentRightShoulder, 1);
    }
    public void NextUpperArms()
    {

        SwitchPart(leftUppperArm, ref currentLeftUppperArm, 1);
        SwitchPart(rightUppperArm, ref currentRightUppperArm, 1);
    }

    public void NextElbows()
    {
        SwitchPart(leftElbow, ref currentLeftElbow, 1);
        SwitchPart(rightElbow, ref currentRightElbow, 1);

    }

    public void NextLowerArms()
    {
        SwitchPart(leftLowerArm, ref currentLeftLowerArm, 1);
        SwitchPart(rightLowerArm, ref currentRightLowerArm, 1);
    }
    public void NextHands()
    {
        SwitchPart(leftHand, ref currentLeftHand, 1);
        SwitchPart(rightHand, ref currentRightHand, 1);
    }
    public void NextHips() => SwitchPart(hips, ref currentHips, 1);
    public void NextKnee()
    {
        SwitchPart(leftKnee, ref currentLeftKnee, 1);
        SwitchPart(rightKnee, ref currentRightKnee, 1);
    }
    public void NextLegs()
    {
        SwitchPart(leftLeg, ref currentLeftLeg, 1);
        SwitchPart(rightLeg, ref currentRightLeg, 1);
    }

    // Previous item
    public void PrevHead() => SwitchPart(head, ref currentHead, -1);
    public void PrevTorso()
    {
        SwitchPart(torso, ref currentTorso, -1);
    }

    public void PrevShoulders()
    {
        SwitchPart(leftShoulder, ref currentLeftShoulder, -1);
        SwitchPart(rightShoulder, ref currentRightShoulder, -1);
    }
    public void PrevUpperArms()
    {

        SwitchPart(leftUppperArm, ref currentLeftUppperArm, -1);
        SwitchPart(rightUppperArm, ref currentRightUppperArm, -1);
    }
    public void PrevElbows()
    {
        SwitchPart(leftElbow, ref currentLeftElbow, -1);
        SwitchPart(rightElbow, ref currentRightElbow, -1);


    }
    public void PrevLowerArms()
    {
        SwitchPart(leftLowerArm, ref currentLeftLowerArm, -1);
        SwitchPart(rightLowerArm, ref currentRightLowerArm, -1);

    }
    public void PrevHands()
    {
        SwitchPart(leftHand, ref currentLeftHand, -1);
        SwitchPart(rightHand, ref currentRightHand, -1);
    }
    public void PrevHips() => SwitchPart(hips, ref currentHips, -1);

    public void PrevKnee()
    {
        SwitchPart(leftKnee, ref currentLeftKnee, -1);
        SwitchPart(rightKnee, ref currentRightKnee, -1);
    }
    public void PrevLegs()
    {
        SwitchPart(leftLeg, ref currentLeftLeg, -1);
        SwitchPart(rightLeg, ref currentRightLeg, -1);
    }

    void SetRootActive()
    {
        bool male = currentGender == Gender.Male;

        MaleObject.SetActive(male);
        FemaleObject.SetActive(!male);
    }

    void RebuildParts()
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

        head = GetChildren(headParent);

        rightShoulder = GetChildren(rightShoulderParent);
        leftShoulder = GetChildren(leftShoulderParent);

        rightElbow = GetChildren(rightElbowParent);
        leftElbow = GetChildren(leftElbowParent);

        torso = GetChildren(torsoParent);
        leftUppperArm = GetChildren(leftUppperArmParent);
        rightUppperArm = GetChildren(rightUppperArmParent);
        leftLowerArm = GetChildren(leftLowerArmParent);
        rightLowerArm = GetChildren(rightLowerArmParent);
        leftHand = GetChildren(leftHandParent);
        rightHand = GetChildren(rightHandParent);

        hips = GetChildren(hipsParent);
        leftKnee = GetChildren(leftKneeParent);
        rightKnee = GetChildren(rightKneeParent);
        leftLeg = GetChildren(leftLegParent);
        rightLeg = GetChildren(rightLegParent);


        if (head.Count > 0) head[0].SetActive(true);

        if (leftShoulder.Count > 0) leftShoulder[0].SetActive(true);
        if (rightShoulder.Count > 0) rightShoulder[0].SetActive(true);

        if (leftElbow.Count > 0) leftElbow[0].SetActive(true);
        if (rightElbow.Count > 0) rightElbow[0].SetActive(true);

        if (torso.Count > 0) torso[0].SetActive(true);
        if (leftUppperArm.Count > 0) leftUppperArm[0].SetActive(true);
        if (rightUppperArm.Count > 0) rightUppperArm[0].SetActive(true);
        if (leftLowerArm.Count > 0) leftLowerArm[0].SetActive(true);
        if (rightLowerArm.Count > 0) rightLowerArm[0].SetActive(true);
        if (leftHand.Count > 0) leftHand[0].SetActive(true);
        if (rightHand.Count > 0) rightHand[0].SetActive(true);

        if (hips.Count > 0) hips[0].SetActive(true);
        if (leftKnee.Count > 0) leftKnee[0].SetActive(true);
        if (rightKnee.Count > 0) rightKnee[0].SetActive(true);
        if (leftLeg.Count > 0) leftLeg[0].SetActive(true);
        if (rightLeg.Count > 0) rightLeg[0].SetActive(true);
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

        if (head.Count > 0) head[currentHead].SetActive(false);

        if (leftShoulder.Count > 0) leftShoulder[currentLeftShoulder].SetActive(false);
        if (rightShoulder.Count > 0) rightShoulder[currentRightShoulder].SetActive(false);

        if (leftElbow.Count > 0) leftElbow[currentLeftElbow].SetActive(false);
        if (rightElbow.Count > 0) rightElbow[currentRightElbow].SetActive(false);


        if (torso.Count > 0) torso[currentTorso].SetActive(false);
        if (leftUppperArm.Count > 0) leftUppperArm[currentLeftUppperArm].SetActive(false);
        if (rightUppperArm.Count > 0) rightUppperArm[currentRightUppperArm].SetActive(false);
        if (leftLowerArm.Count > 0) leftLowerArm[currentLeftLowerArm].SetActive(false);
        if (rightLowerArm.Count > 0) rightLowerArm[currentRightLowerArm].SetActive(false);
        if (leftHand.Count > 0) leftHand[currentLeftHand].SetActive(false);
        if (rightHand.Count > 0) rightHand[currentRightHand].SetActive(false);

        if (hips.Count > 0) hips[currentHips].SetActive(false);
        if (leftKnee.Count > 0) leftKnee[currentLeftKnee].SetActive(false);
        if (rightKnee.Count > 0) rightKnee[currentRightKnee].SetActive(false);
        if (leftLeg.Count > 0) leftLeg[currentLeftLeg].SetActive(false);
        if (rightLeg.Count > 0) rightLeg[currentRightLeg].SetActive(false);

        currentHead = currentSavedHead;

        currentLeftShoulder = currentSavedLeftShoulder;
        currentRightShoulder = currentSavedRightShoulder;

        currentLeftElbow = currentSavedLeftElbow;
        currentRightElbow = currentSavedRightElbow;

        currentTorso = currentSavedTorso;
        currentLeftUppperArm = currentSavedLeftUppperArm;
        currentRightUppperArm = currentSavedRightUppperArm;
        currentLeftLowerArm = currentSavedLeftLowerArm;
        currentRightLowerArm = currentSavedRightLowerArm;
        currentLeftHand = currentSavedLeftHand;
        currentRightHand = currentSavedRightHand;

        currentHips = currentSavedHips;
        currentLeftLeg = currentSavedLeftLeg;
        currentRightLeg = currentSavedRightLeg;


        if (head.Count > 0) head[currentHead].SetActive(true);

        if (leftShoulder.Count > 0) leftShoulder[currentLeftShoulder].SetActive(true);
        if (rightShoulder.Count > 0) rightShoulder[currentRightShoulder].SetActive(true);

        if (leftElbow.Count > 0) leftElbow[currentLeftElbow].SetActive(true);
        if (rightElbow.Count > 0) rightElbow[currentRightElbow].SetActive(true);

        if (torso.Count > 0) torso[currentTorso].SetActive(true);
        if (leftUppperArm.Count > 0) leftUppperArm[currentLeftUppperArm].SetActive(true);
        if (rightUppperArm.Count > 0) rightUppperArm[currentRightUppperArm].SetActive(true);
        if (leftLowerArm.Count > 0) leftLowerArm[currentLeftLowerArm].SetActive(true);
        if (rightLowerArm.Count > 0) rightLowerArm[currentRightLowerArm].SetActive(true);
        if (leftHand.Count > 0) leftHand[currentLeftHand].SetActive(true);
        if (rightHand.Count > 0) rightHand[currentRightHand].SetActive(true);

        if (hips.Count > 0) hips[currentHips].SetActive(true);
        if (leftKnee.Count > 0) leftKnee[currentLeftKnee].SetActive(true);
        if (rightKnee.Count > 0) rightKnee[currentRightKnee].SetActive(true);
        if (leftLeg.Count > 0) leftLeg[currentLeftLeg].SetActive(true);
        if (rightLeg.Count > 0) rightLeg[currentRightLeg].SetActive(true);
    }
}


