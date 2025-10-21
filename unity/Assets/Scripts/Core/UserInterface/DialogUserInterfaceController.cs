using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIEffects;
using Core.Dialog;
using Core.SFX;
using Framework.Controller;
using JetBrains.Annotations;
using ScriptableObjects.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Action = Framework.Action.Action;

namespace Core.UserInterface
{
    public class DialogUserInterfaceController : InterfaceController<DialogUserInterfaceController>
    {
        [Header("Dialog Entity Information")]
        public List<DialogEntityData> DialogEntity = new List<DialogEntityData>();
        
        [Header("UI References")]
        public TextMeshProUGUI dialogText;
        public TextMeshProUGUI entityNameText;
        public Image entityImage;
        public Button skipButton;

        private DialogData currentDialogData;
        private int currentDialogIndex = 0;
        private Coroutine typingCoroutine;
        private bool isTyping = false;
        private SFXController sfxController;

        public new void Start()
        {
            base.Start();
            sfxController = SFXController.Instance;
            if (skipButton != null)
                skipButton.onClick.AddListener(OnSkipButtonClick);
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
            if (dialog == null) return;
            if(dialog.alreadyTalked && entityData.summonableOnce) return;
            
            panel.GetComponent<UIEffect>().transitionRate = 1f;
            OpenPanel();
            
            DialogData dialogData = dialog.GetDialogToPlay();
            dialog.alreadyTalked = true;
            if (dialogData == null || dialogData.contents == null || dialogData.contents.Length == 0) return;

            currentDialogData = dialogData;
            currentDialogIndex = 0;
            
            ShowCurrentDialog();
            foreach (Action dialogAction in currentDialogData.onStartDialogAction) 
                dialogAction.Execute();
        }

        private void ShowCurrentDialog()
        {
            if (currentDialogIndex > currentDialogData.contents.Length) return;
            if (currentDialogIndex == currentDialogData.contents.Length)
            {
                EndDialog();
                return;
            }

            DialogContent content = currentDialogData.contents[currentDialogIndex];
            entityNameText.text = content.entity.entityName;
            entityImage.sprite = content.entity.entitySprite;

            if (currentDialogData.talkAudioClip != null && sfxController != null)
            {
                sfxController.PlayTalkAudioClip(currentDialogData.talkAudioClip);
            }
            
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            
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
            
            if (sfxController != null)
                sfxController.StopTalkAudioClip();
        }

        private void OnSkipButtonClick()
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogText.text = currentDialogData.contents[currentDialogIndex].content;
                isTyping = false;
                
                if (sfxController != null)
                    sfxController.StopTalkAudioClip();
            }
            else
            {
                currentDialogIndex++;
                ShowCurrentDialog();
            }
        }

        private void EndDialog()
        {
            if (sfxController != null)
                sfxController.StopTalkAudioClip();

            foreach (Action dialogAction in currentDialogData.onEndDialogAction)
            {
                try { dialogAction.Execute(); }
                catch (Exception e) { Debug.LogError(e); }
            }

            currentDialogData = null;
            currentDialogIndex = 0;
            ClosePanel();
        }

        private void OnDestroy()
        {
            if (skipButton != null)
                skipButton.onClick.RemoveListener(OnSkipButtonClick);
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
            return entityData.normalDialog;
        }
    }
}