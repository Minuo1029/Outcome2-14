using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Teakisland.DialogueSystem
{

    public class DialogueManager : MonoBehaviour
    {

        private static DialogueManager instance;
        public static DialogueManager Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType<DialogueManager>();
                return instance;
            }
        }

        public static float CurrentVertical;
        public static List<TalkItem> TalkItemCache = new List<TalkItem>();
        public static List<QuestionView> QuestionItemCache = new List<QuestionView>();
        public float TextContentWidth => maxContentWidth - headHeight - headNearTalk - textMargin * 2;

        [Space(10)]
        public bool showBubbleRect;
        public QuestionView mainQuestion;
        public RectTransform scrollRect;
        public RectTransform content;
        public RectTransform viewport;

        [Header("气泡格式设置")]
        public float scrollForce = 4;
        public float textMargin = 6;
        public float nameHeight = 17;
        public float headHeight = 35;
        public float headNearTalk = 5;
        public float maxContentWidth = 280;

        [Header("布局设置")]
        public float padding_Top = 10;
        public float padding_Bottom = 10;
        public float padding_Left = 10;
        public float itemSpace = 10;
        public float validHoldQuestion = 400;

        [Header("所有角色")]
        public Sprite defaultTalker;
        [Space(5)]
        public Talker talker0;
        public Talker talker1;
        public Talker talker2;
        public Talker talker3;
        public Talker talker4;

        private bool isAutoScroll;
        private bool isHoldQuestion;
        private float defaultHeight;
        private Vector2 contentSize;
        private ScrollRect scrollView;
        private Coroutine coroutine;
        private Dictionary<string, Talker> allTalkers;

        private void Awake()
        {
            scrollView = viewport.parent.GetComponent<ScrollRect>();
            scrollRect = scrollView.GetComponent<RectTransform>();
            contentSize = content.sizeDelta;
            allTalkers = new Dictionary<string, Talker>();
            allTalkers.Add(talker0.name, talker0);
            allTalkers.Add(talker1.name, talker1);
            allTalkers.Add(talker2.name, talker2);
            allTalkers.Add(talker3.name, talker3);
            allTalkers.Add(talker4.name, talker4);
            defaultHeight = scrollRect.sizeDelta.y;
        }

        private void Update()
        {

            if (scrollRect.sizeDelta.y != defaultHeight)
            {
                defaultHeight = scrollRect.sizeDelta.y;
                isAutoScroll = true;
            }

            if (isAutoScroll)
            {
                if (scrollView.verticalNormalizedPosition != 0)
                {
                    scrollView.verticalNormalizedPosition = Mathf.Lerp(scrollView.verticalNormalizedPosition, 0, Time.deltaTime * scrollForce);
                }
                else
                {
                    isAutoScroll = false;
                }
            }
        }

        public TalkItem SpawnTalkItem()
        {
            TalkItem itemRes = Resources.Load<TalkItem>("TalkItem");
            TalkItem item = Instantiate(itemRes, content);
            TalkItemCache.Add(item);
            return item;
        }

        /// <summary>
        /// 创建或使用已有角色，发送指定消息
        /// </summary>
        /// <param name="talkerName">角色名称</param>
        /// <param name="message">消息内容</param>
        public void Talk(string talkerName, string message)
        {
            if (allTalkers.ContainsKey(talkerName))
            {
                allTalkers[talkerName].Say(message);
            }
            else
            {
                Talker talker = new Talker(talkerName, defaultTalker);
                allTalkers.Add(talkerName, talker);
                talker.Say(message);
            }
        }

        /// <summary>
        /// 弹出问题对话框
        /// </summary>
        public void ShowQuestion(QuestionSetting setting)
        {
            mainQuestion.SetQuestionData(setting);
            mainQuestion.gameObject.SetActive(true);
            HoldQuestion();
        }

        public void HoldQuestion()
        {
            return;
            if (scrollRect.sizeDelta.y >= validHoldQuestion)
            {
                isHoldQuestion = true;
                scrollRect.sizeDelta -= Vector2.up * (mainQuestion.ownRect.sizeDelta.y + itemSpace);
            }
        }

        public void ReleaseQuestion()
        {
            if (isHoldQuestion)
            {
                isHoldQuestion = false;
                scrollRect.sizeDelta += Vector2.up * (mainQuestion.ownRect.sizeDelta.y + itemSpace);
            }
        }

        public void RefreshContent()
        {
            float height = padding_Top + padding_Bottom;
            for (int i = 0; i < TalkItemCache.Count; i++) height += TalkItemCache[i].Height;
            for (int i = 0; i < QuestionItemCache.Count; i++) height += QuestionItemCache[i].Height;
            if (TalkItemCache.Count > 0) height += TalkItemCache.Count * itemSpace;
            if (QuestionItemCache.Count > 0) height += QuestionItemCache.Count * itemSpace;
            contentSize.y = height;
            content.sizeDelta = contentSize;
            if (contentSize.y > viewport.sizeDelta.y) ScrollToEnd();
        }

        public void ClearCache()
        {
            for (int i = 0; i < TalkItemCache.Count; i++) Destroy(TalkItemCache[i].gameObject);
            for (int i = 0; i < QuestionItemCache.Count; i++) Destroy(QuestionItemCache[i].gameObject);
            TalkItemCache.Clear();
            QuestionItemCache.Clear();
            CurrentVertical = 0;
            RefreshContent();
        }

        /// <summary>
        /// 自动滑至最下
        /// </summary>
        public void ScrollToEnd()
        {
            isAutoScroll = true;
        }

    }
}