using UnityEngine;

namespace Aquarium
{
    public class InteractiveObject : MonoBehaviour
    {
        #region Variables
        [Header("Interact")]
        public string interactionText = "Name Of Object";

        public Renderer TargetRenderer => targetRenderer;
        private Renderer targetRenderer;
        #endregion

        #region Properties
        //public Renderer[] TargetRenderers { get; private set; }
        #endregion

        #region Unity Event Methods
        void Awake()
        {
            //targetRenderer = GetComponentInChildren<Renderer>();
        }

        private void Update()
        {

        }
        #endregion

        #region Custom Methods
        public virtual void OnClick()
        {
            Debug.Log(interactionText + " 클릭됨");
        }
        #endregion
    }
}
