using UnityEngine;

namespace Guppy
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
            targetRenderer = GetComponentInChildren<Renderer>();
        }
        #endregion
    }
}