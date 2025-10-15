using System.Collections;
using System.Collections.Generic;
using Core.Dialog;
using Framework.Action;
using Framework.Controller;
using JetBrains.Annotations;
using ScriptableObjects.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core.UserInterface
{
    public class DialogUserInterfaceController : BaseController<DialogUserInterfaceController>
    {
        [Header("Dialog Entity Information")]
        public List<DialogEntityData> DialogEntity = new List<DialogEntityData>();
        
        [Header("UI References")]
        public GameObject dialogPanel;
        public TextMeshProUGUI dialogText;
        public TextMeshProUGUI entityNameText;
        public Button skipButton;

        private DialogData currentDialogData;
        private int currentDialogIndex = 0;
        private Coroutine typingCoroutine;
        private bool isTyping = false;

        public UnityAction OnDialogStart;
        public UnityAction OnDialogEnd;

        public void Start()
        {
            dialogPanel.SetActive(false);
            
            if (skipButton != null)
            {
                skipButton.onClick.AddListener(OnSkipButtonClick);
            }
        }

        private void AddDialogEntityData(EntityData entityData)
        {
            if (DialogEntity.Find(d => d.entityData == entityData) != null) return;
            DialogEntity.Add(new DialogEntityData(entityData));
        }

        [CanBeNull]
        private DialogEntityData GetDialogEntityData(EntityData entityData)
        {
            return DialogEntity.Find(d => d.entityData == entityData);
        }
        
        public void LaunchDialogWithEntity(EntityData entityData)
        {
            AddDialogEntityData(entityData);
            DialogEntityData dialog = GetDialogEntityData(entityData);
            if (dialog == null)
            {
                Debug.LogError("Error: Dialog Entity not found");
                return;
            }
            DialogData dialogData = dialog.GetDialogToPlay();
            dialog.alreadyTalked = true;
            if (dialogData == null || dialogData.contents == null || dialogData.contents.Length == 0)
            {
                Debug.LogWarning("DialogData is null or empty!");
                return;
            }

            currentDialogData = dialogData;
            currentDialogIndex = 0;
            dialogPanel.SetActive(true);
            
            ShowCurrentDialog();
            foreach (Action dialogAction in currentDialogData.onStartDialogAction) 
                dialogAction.Execute();
            OnDialogStart?.Invoke();
        }

        private void ShowCurrentDialog()
        {
            if (currentDialogIndex >= currentDialogData.contents.Length)
            {
                EndDialog();
                return;
            }

            DialogContent content = currentDialogData.contents[currentDialogIndex];
            entityNameText.text = content.entity.entityName;
            
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            
            typingCoroutine = StartCoroutine(TypeText(content));
        }

        private IEnumerator TypeText(DialogContent dialogContent)
        {
            isTyping = true;
            dialogText.text = "";

            foreach (char letter in dialogContent.content)
            {
                dialogText.text += letter;
                yield return new WaitForSeconds(dialogContent.typingSpeed);
            }

            isTyping = false;
        }

        private void OnSkipButtonClick()
        {
            if (isTyping)
            {
                // Si le texte est en train de s'afficher, afficher tout de suite
                StopCoroutine(typingCoroutine);
                dialogText.text = currentDialogData.contents[currentDialogIndex].content;
                isTyping = false;
            }
            else
            {
                // Passer au dialogue suivant
                currentDialogIndex++;
                ShowCurrentDialog();
            }
        }

        private void EndDialog()
        {
            dialogPanel.SetActive(false);
            foreach (Action dialogAction in currentDialogData.onEndDialogAction) 
                dialogAction.Execute();
            currentDialogData = null;
            currentDialogIndex = 0;
            OnDialogEnd?.Invoke();
        }

        private void OnDestroy()
        {
            if (skipButton != null)
            {
                skipButton.onClick.RemoveListener(OnSkipButtonClick);
            }
        }
    }

    public class DialogEntityData
    {
        public EntityData entityData;
        public bool alreadyTalked;
        
        public DialogEntityData(EntityData entityData)
        {
            this.entityData = entityData;
            alreadyTalked = false;
        }

        public DialogData GetDialogToPlay()
        {
            if (alreadyTalked == false) return entityData.firstDialog;
            return entityData.normalDialog;
        }
    }
}