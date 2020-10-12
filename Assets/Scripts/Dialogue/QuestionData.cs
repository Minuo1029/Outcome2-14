using System;
using UnityEngine;

namespace Teakisland.DialogueSystem
{
    [Serializable]
    public class QuestionData
    {
        [TextArea]
        public string title;
        public string hint;
        [TextArea]
        public string content;
        public string selectA;
        public string selectB;

        public QuestionData(string title, string hint, string content, string selectA, string selectB)
        {
            this.title = title;
            this.hint = hint;
            this.content = content;
            this.selectA = selectA;
            this.selectB = selectB;
        }

    }
}
