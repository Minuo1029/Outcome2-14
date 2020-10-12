using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Teakisland.DialogueSystem
{

    public class QuestionView : MonoBehaviour
    {

        public float Height
        {
            get
            {
                return ownRect.sizeDelta.y;
            }
        }

        public Image autoProgress;
        public Text titleText;
        public Text hintText;
        public Text contentText;

        public Text selectTextA, selectTextB;
        public Button selectBtnA, selectBtnB;
        public AudioSource audioSourceA, audioSourceB;

        [HideInInspector] public bool isClick;

        private float autoTick = -1;
        private float maxAutoTick = -1;
        private AutoSelectType autoSelect;
        private UnityAction selectA, selectB;

        [HideInInspector] public RectTransform ownRect;

        private void Awake()
        {
            ownRect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (autoTick > -1)
            {
                if (autoTick > 0)
                {
                    autoTick -= Time.deltaTime;
                    autoProgress.fillAmount = autoTick / maxAutoTick;
                }
                else
                {
                    isClick = true;
                    autoTick = -1;
                    autoProgress.fillAmount = 0;
                    if (autoSelect == AutoSelectType.SelectA) selectA();
                    else selectB();
                }
            }
        }

        public void Clear()
        {
            titleText.text = null;
            hintText.text = null;
            contentText.text = null;
            selectTextA.text = "SelectA";
            selectTextB.text = "SelectB";
            autoProgress.fillAmount = 1;
            autoTick = -1;
        }

        public void SetInteractable(bool value)
        {
            selectBtnA.interactable = value;
            selectBtnB.interactable = value;
            if (!value)
            {
                autoTick = -1;
                autoProgress.fillAmount = 0;
            }
        }

        public void SetQuestionData(QuestionSetting setting)
        {
            isClick = false;
            SetInteractable(true);
            audioSourceA.clip = setting.soundEffect_SelectA;
            audioSourceB.clip = setting.soundEffect_SelectB;
            autoSelect = setting.autoSelect;
            autoTick = setting.autoTime;
            maxAutoTick = setting.autoTime;
            titleText.text = setting.question.title;
            hintText.text = setting.question.hint;
            contentText.text = setting.question.content;
            selectTextA.text = setting.question.selectA;
            selectTextB.text = setting.question.selectB;
        }

        public void AnyClick()
        {
            //Clear();
            //gameObject.SetActive(false);
            isClick = true;
            SetInteractable(false);
            DialogueManager.Instance.ReleaseQuestion();
        }
        
        public void AddClick_SelectA(UnityAction action)
        {
            if (action == null) return;
            selectA = action;
            selectA += AnyClick;
            selectA += audioSourceA.Play;
            selectBtnA.onClick.RemoveAllListeners();
            selectBtnA.onClick.AddListener(selectA);
        }
        
        public void AddClick_SelectB(UnityAction action)
        {
            if (action == null) return;
            selectB = action;
            selectB += AnyClick;
            selectB += audioSourceB.Play;
            selectBtnB.onClick.RemoveAllListeners();
            selectBtnB.onClick.AddListener(selectB);
        }
        

    }
}