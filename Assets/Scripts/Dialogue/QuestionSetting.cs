using System.Collections;
using UnityEngine;

namespace Teakisland.DialogueSystem
{

    public enum AutoSelectType
    {
        SelectA,
        SelectB
    }

    public class QuestionSetting : MonoBehaviour
    {

        public float skipDelay = 2;
        [Space(5)]
        public float autoTime = 10;
        public AutoSelectType autoSelect;
        [Space(5)]
        public string ASkipto;
        public string BSkipto;
        [Space(5)]
        public string userSay_selectA;
        public string userSay_selectB;
        [Space(5)]
        public AudioClip soundEffect_SelectA;
        public AudioClip soundEffect_SelectB;
        [Space(5)]
        public QuestionData question;

        public Coroutine ShowQuestion() => StartCoroutine(WaitUserSelect());

        private IEnumerator WaitUserSelect()
        {
            DialogueManager.Instance.ShowQuestion(this);
            DialogueManager.Instance.mainQuestion.AddClick_SelectA(SelectA);
            DialogueManager.Instance.mainQuestion.AddClick_SelectB(SelectB);
            while (!DialogueManager.Instance.mainQuestion.isClick) yield return null;
        }

        private IEnumerator WaitForResult(string skip)
        {
            DialogueManager.Instance.mainQuestion.isClick = true;
            yield return new WaitForSeconds(skipDelay);
            yield return MainAutoRunner.SkipTo(skip);
        }

        public void SelectA()
        {
            if (!string.IsNullOrEmpty(userSay_selectA)) DialogueManager.Instance.Talk(DialogueManager.Instance.talker0.name, userSay_selectA);
            StartCoroutine(WaitForResult(ASkipto));
        }

        public void SelectB()
        {
            if (!string.IsNullOrEmpty(userSay_selectB)) DialogueManager.Instance.Talk(DialogueManager.Instance.talker0.name, userSay_selectB);
            StartCoroutine(WaitForResult(BSkipto));
        }

    }
}