using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public int totalObjectives = 0;
    public int currentObjectiveIndex = 0;
    public TextMeshProUGUI progressText;

    public TextMeshProUGUI currentObjectiveText;
    public List<LocalizedString> objectives;
    public LocalizedString controlReminder;
    public GameObject objectiveParent;
    private Animator animator;

    public float inactivityThreshold = 30f;
    public float autoCloseTime = 15f;
    private float objectiveChangeDelay = 0.51f;

    private float inactivityTimer = 0f;
    private float closeTimer = 0f;
    private bool reminderShown = false;
    private bool panelClosed = false;

    private int objectiveChangingHash = Animator.StringToHash("objectiveChanging");
    private int isClosingHash = Animator.StringToHash("isClosing");

    private bool isChanging = false;

    private void Start()
    {
        animator = objectiveParent.GetComponent<Animator>();
        UpdateObjectiveUI();
    }

    private void Update()
    {
        HandleInactivity();
        CheckAnimationState();
    }

    private void HandleInactivity()
    {
        if (Mouse.current.delta.ReadValue() != Vector2.zero || Keyboard.current.anyKey.wasPressedThisFrame || Gamepad.current?.leftStick.ReadValue() != Vector2.zero)
        {
            inactivityTimer = 0f;
            if (reminderShown)
            {
                reminderShown = false;
                UpdateObjectiveUI();
            }
        }
        else
        {
            inactivityTimer += Time.deltaTime;
        }

        if (inactivityTimer >= inactivityThreshold && !reminderShown)
        {
            reminderShown = true;
            ShowControlReminder();
        }

        closeTimer += Time.deltaTime;
        if (closeTimer >= autoCloseTime && !panelClosed)
        {
            panelClosed = true;
            animator.SetBool(isClosingHash, true);
        }
    }

    private void CheckAnimationState()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (animator.GetBool(objectiveChangingHash) && stateInfo.IsTag("ObjectiveChange") && stateInfo.normalizedTime >= 1f)
        {
            animator.SetBool(objectiveChangingHash, false);
        }

        if (animator.GetBool(isClosingHash) && stateInfo.IsTag("Close") && stateInfo.normalizedTime >= 1f)
        {
            animator.SetBool(isClosingHash, false);
        }
    }

    private void ShowControlReminder()
    {
        animator.SetBool(objectiveChangingHash, true);
        controlReminder.StringChanged += SetCurrentObjectiveText;
        controlReminder.RefreshString();
    }

    private void SetCurrentObjectiveText(string value)
    {
        currentObjectiveText.text = value;
        controlReminder.StringChanged -= SetCurrentObjectiveText;
    }

    private void SetObjectiveTextByIndex(int index)
    {
        if (index >= 0 && index < objectives.Count)
        {
            objectives[index].StringChanged += SetCurrentObjectiveText;
            objectives[index].RefreshString();
        }
    }

    public void AdvanceObjective()
    {
        currentObjectiveIndex++;
        if (currentObjectiveIndex > totalObjectives)
            currentObjectiveIndex = totalObjectives;

        UpdateObjectiveUI();
    }

    public void UpdateObjectiveUI()
    {
        progressText.text = $"{currentObjectiveIndex}/{totalObjectives}";

        if (currentObjectiveIndex >= 0 && currentObjectiveIndex < objectives.Count)
        {
            animator.SetBool(objectiveChangingHash, true);
            isChanging = true;
            StartCoroutine(ResetObjectiveChangingFlag());
        }

        inactivityTimer = 0f;
        closeTimer = 0f;
        panelClosed = false;
        animator.SetBool(isClosingHash, false);
    }

    public void NextObjective()
    {
        if (currentObjectiveIndex < totalObjectives)
        {
            currentObjectiveIndex++;
            UpdateObjectiveUI();
        }
    }

    public void PreviousObjective()
    {
        if (currentObjectiveIndex > 0)
        {
            currentObjectiveIndex--;
            UpdateObjectiveUI();
        }
    }

    private IEnumerator ResetObjectiveChangingFlag()
    {
        yield return new WaitForSeconds(objectiveChangeDelay);

        animator.SetBool(objectiveChangingHash, false);
        isChanging = false;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.IsTag("ObjectiveChange") && stateInfo.normalizedTime < 1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        yield return new WaitForSeconds(0.1f); 
        SetObjectiveTextByIndex(currentObjectiveIndex);
    }
}
