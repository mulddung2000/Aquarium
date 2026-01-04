using UnityEngine;

namespace Aquarium
{
    [System.Serializable]
    public class DialogueLine
    {
        [Header("Speaker")]
        [Tooltip("이 대사를 말하는 캐릭터의 이미지. 나레이션일 경우 비워둔다.")]
        public Sprite speakerSprite;

        [Header("Text")]
        [TextArea(2, 4)]
        public string text;
    }
}
