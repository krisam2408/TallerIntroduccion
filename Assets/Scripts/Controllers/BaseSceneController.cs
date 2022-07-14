using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public abstract class BaseSceneController : MonoBehaviour
    {
        [SerializeField] private PlayerBehaviour m_playerPrefab;
        protected PlayerBehaviour PlayerPrefab { get { return m_playerPrefab; } }
        [SerializeField] private CinemachineVirtualCamera m_camera;

        private Coroutine m_currentCoroutine;

        protected void OnStart()
        {
            m_currentCoroutine = StartCoroutine(SpawnPlayer());
        }

        private IEnumerator SpawnPlayer()
        {
            yield return new WaitForEndOfFrame();

            PlayerBehaviour player = Instantiate(m_playerPrefab, Vector3.up, Quaternion.identity);
            player.name = "Player";

            ConfigureCamera(player);
        }

        private void ConfigureCamera(PlayerBehaviour player)
        {
            m_camera.Follow = player.transform;
            m_camera.LookAt = player.transform;

            CinemachineTransposer transposer = m_camera.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
            transposer.m_FollowOffset = new Vector3(0f, 0f, -7f);
            transposer.m_XDamping = 0.7f;
            transposer.m_YDamping = 1f;
            transposer.m_ZDamping = 1f;

            CinemachineComposer composer = m_camera.GetCinemachineComponent<CinemachineComposer>();
            composer.m_TrackedObjectOffset = new Vector3(0f, 1f, 0f);
            composer.m_LookaheadTime = 0.1f;
            composer.m_LookaheadSmoothing = 3f;
            composer.m_LookaheadIgnoreY = false;
            composer.m_HorizontalDamping = 3.1f;
            composer.m_VerticalDamping = 0.5f;
            composer.m_ScreenX = 0.5f;
            composer.m_ScreenY = 0.5f;
            composer.m_DeadZoneWidth = 0.28f;
            composer.m_DeadZoneHeight = 0.49f;
            composer.m_SoftZoneWidth = 0.8f;
            composer.m_SoftZoneHeight = 0.8f;
            composer.m_BiasX = 0f;
            composer.m_BiasY = 0f;
            composer.m_CenterOnActivate = true;
        }

        protected IEnumerator ChangeScene(string num)
        {
            yield return SceneManager.LoadSceneAsync(num);
        }

        public virtual void HolaMUndo()
        {
            Debug.Log("Hola Mundo");
        }
    }
}