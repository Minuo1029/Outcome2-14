using UnityEngine;
using UnityEngine.UI;

namespace Teakisland.DialogueSystem
{
    public class TalkItem : MonoBehaviour
    {

        public Image talkerHead;
        public Image contentImage;
        public Text talkerName;
        public Text contentText;

        public float Height
        {
            get
            {
                return ownRect.sizeDelta.y;
            }
        }

        private RectTransform ownRect;
        private ContentSizeFitter sizeFitter;

        private void Awake()
        {
            ownRect = GetComponent<RectTransform>();
            sizeFitter = contentText.GetComponent<ContentSizeFitter>();
            Image ownBack = GetComponent<Image>();
            if (ownBack != null) ownBack.enabled = DialogueManager.Instance.showBubbleRect;
        }

        public void RefreshLayout(bool toRight = false)
        {

            contentImage.rectTransform.pivot = Vector2.up;
            contentText.rectTransform.pivot = Vector2.up;
            //talkerName.rectTransform.pivot = Vector2.zero;
            talkerName.rectTransform.pivot = new Vector2(0, 1);

            Vector2 imageSize = new Vector2(contentText.rectTransform.sizeDelta.x + DialogueManager.Instance.textMargin * 2, contentText.rectTransform.sizeDelta.y + DialogueManager.Instance.textMargin * 2);
            Vector2 imagePos = new Vector2(contentText.rectTransform.anchoredPosition.x - DialogueManager.Instance.textMargin, 0);
            if (imageSize.x > DialogueManager.Instance.maxContentWidth) imageSize.x = DialogueManager.Instance.maxContentWidth - DialogueManager.Instance.headNearTalk;

            Vector2 rectSize = new Vector2(DialogueManager.Instance.maxContentWidth, DialogueManager.Instance.nameHeight);
            if (imageSize.y <= DialogueManager.Instance.headHeight) rectSize.y += DialogueManager.Instance.headHeight;
            else rectSize.y += imageSize.y;
            ownRect.sizeDelta = rectSize;

            RectTransform headMask = talkerHead.transform.parent.GetComponent<RectTransform>();

            headMask.pivot = Vector2.up;
            headMask.sizeDelta = Vector3.one * DialogueManager.Instance.headHeight;
            headMask.anchorMin = Vector2.up;
            headMask.anchorMax = Vector2.up;
            
            //headMask.anchoredPosition = new Vector2(0, -DialogueManager.Instance.nameHeight);
            headMask.anchoredPosition = Vector2.zero;
            float nameWidth = DialogueManager.Instance.maxContentWidth - DialogueManager.Instance.headHeight - DialogueManager.Instance.headNearTalk;
            talkerName.rectTransform.sizeDelta = new Vector2(nameWidth, DialogueManager.Instance.nameHeight);
            //Vector2 anchor = new Vector2(DialogueManager.Instance.headHeight / DialogueManager.Instance.maxContentWidth, (ownRect.sizeDelta.y - DialogueManager.Instance.nameHeight) / ownRect.sizeDelta.y);
            Vector2 anchor = new Vector2(DialogueManager.Instance.headHeight / DialogueManager.Instance.maxContentWidth, 1);
            talkerName.rectTransform.anchorMin = anchor;
            talkerName.rectTransform.anchorMax = anchor;
            contentImage.rectTransform.anchorMin = anchor;
            contentImage.rectTransform.anchorMax = anchor;
            contentText.rectTransform.anchorMin = anchor;
            contentText.rectTransform.anchorMax = anchor;
            contentText.rectTransform.anchoredPosition = new Vector2(DialogueManager.Instance.headNearTalk + DialogueManager.Instance.textMargin, -DialogueManager.Instance.textMargin);
            contentImage.rectTransform.sizeDelta = imageSize;
            contentImage.rectTransform.anchoredPosition = imagePos;
            //talkerName.rectTransform.anchoredPosition = new Vector2(DialogueManager.Instance.headNearTalk, 0);
            talkerName.rectTransform.anchoredPosition = new Vector2(DialogueManager.Instance.headNearTalk, -imageSize.y);
            if (toRight)
            {
                talkerName.alignment = TextAnchor.MiddleRight;
                talkerName.rectTransform.anchoredPosition -= Vector2.right * (DialogueManager.Instance.headHeight + DialogueManager.Instance.headNearTalk);
                headMask.pivot = Vector2.one;
                headMask.anchorMin = Vector2.one;
                headMask.anchorMax = Vector2.one;
                headMask.anchoredPosition = Vector2.zero;

                //contentText.rectTransform.anchoredPosition -= Vector2.right * (DialogueManager.Instance.headHeight + DialogueManager.Instance.headNearTalk);
                //contentImage.rectTransform.anchoredPosition -= Vector2.right * (DialogueManager.Instance.headHeight + DialogueManager.Instance.headNearTalk);

                contentText.rectTransform.anchorMin = Vector2.one;
                contentText.rectTransform.anchorMax = Vector2.one;
                contentImage.rectTransform.anchorMin = Vector2.one;
                contentImage.rectTransform.anchorMax = Vector2.one;
                contentText.rectTransform.pivot = Vector2.one;
                contentImage.rectTransform.pivot = Vector2.one;

                Vector2 imgPos = contentImage.rectTransform.anchoredPosition;
                Vector2 txtPos = contentText.rectTransform.anchoredPosition;
                imgPos.x = -(DialogueManager.Instance.headHeight + DialogueManager.Instance.headNearTalk);
                txtPos.x = imgPos.x - DialogueManager.Instance.textMargin;
                contentImage.rectTransform.anchoredPosition = imgPos;
                contentText.rectTransform.anchoredPosition = txtPos;
            }
        }

        public void SetTalkerData(Talker talker, string message)
        {
            int width = GetTextWidth(message);
            contentText.text = message;
            if (width <= DialogueManager.Instance.TextContentWidth)
            {
                contentText.rectTransform.sizeDelta = new Vector2(width, DialogueManager.Instance.headHeight - DialogueManager.Instance.textMargin * 2);
            }
            else
            {
                contentText.rectTransform.sizeDelta = new Vector2(DialogueManager.Instance.TextContentWidth, DialogueManager.Instance.headHeight - DialogueManager.Instance.textMargin * 2);
                sizeFitter.SetLayoutHorizontal();
                sizeFitter.SetLayoutVertical();
                float minHeight = DialogueManager.Instance.headHeight - DialogueManager.Instance.textMargin * 2;
                if (contentText.rectTransform.sizeDelta.y <= minHeight)
                {
                    contentText.rectTransform.sizeDelta = new Vector2(contentText.rectTransform.sizeDelta.x, minHeight);
                }
            }
            talkerHead.sprite = talker.head;
            talkerName.text = talker.name;
            talkerName.color = talker.nameColor;
            contentText.color = talker.contentColor;
            contentImage.color = talker.bubbleColor;
            RefreshLayout(talker.isMainTalker);
            ownRect.anchoredPosition = new Vector2(DialogueManager.Instance.padding_Left, -DialogueManager.Instance.padding_Top - DialogueManager.CurrentVertical);
            DialogueManager.CurrentVertical += ownRect.sizeDelta.y + DialogueManager.Instance.itemSpace;
            DialogueManager.Instance.RefreshContent();
        }

        public int GetTextWidth(string message)
        {
            int totalLength = 0;
            Font myFont = contentText.font;
            myFont.RequestCharactersInTexture(message, contentText.fontSize, contentText.fontStyle);
            CharacterInfo characterInfo = new CharacterInfo();
            char[] arr = message.ToCharArray();
            foreach (char c in arr)
            {
                myFont.GetCharacterInfo(c, out characterInfo, contentText.fontSize);
                totalLength += characterInfo.advance;
            }
            return totalLength;
        }

    }
}