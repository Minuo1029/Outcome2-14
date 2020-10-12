using UnityEngine;

namespace Teakisland.DialogueSystem
{
    [System.Serializable]
    public class Talker
    {

        public bool isMainTalker;
        public string name;
        public Sprite head;
        public Color nameColor;
        public Color contentColor;
        public Color bubbleColor;

        public Talker(string name)
        {
            this.name = name;
            head = DialogueManager.Instance?.defaultTalker;
            nameColor = new Color(0.7F, 0, 1, 1);
            contentColor = new Color(1, 1, 1, 1);
            bubbleColor = new Color(0.4F, 0.6F, 0.9F, 1);
        }

        public Talker(string name, Sprite head)
        {
            this.name = name;
            this.head = head;
            nameColor = new Color(0.7F, 0, 1, 1);
            contentColor = new Color(1, 1, 1, 1);
            bubbleColor = new Color(0.4F, 0.6F, 0.9F, 1);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public void Say(string message)
        {
            TalkItem talkItem = DialogueManager.Instance.SpawnTalkItem();
            talkItem.SetTalkerData(this, message);
        }

    }
}