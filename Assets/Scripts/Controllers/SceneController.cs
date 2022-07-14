using UnityEngine;

namespace Scenes
{
    public class SceneController : BaseSceneController
    {
        private void Start()
        {
            OnStart();

            HolaMUndo();
        }

        public override void HolaMUndo()
        {
            Debug.Log("Chao a todos");
        }
    }
}