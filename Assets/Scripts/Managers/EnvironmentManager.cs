using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class EnvironmentManager : SingletonBehaviour<EnvironmentManager>
    {
        private SceneReference m_Current = null;

        private AsyncOperation m_SceneLoading = null;

        public bool IsSceneLoading => m_SceneLoading != null && m_SceneLoading.isDone == false;
        
        public void LoadEnvironment(SceneReference sceneReference)
        {
            if (m_Current != null)
            {
                SceneManager.UnloadSceneAsync(m_Current.SceneName);
            }

            m_Current = sceneReference;

            m_SceneLoading = SceneManager.LoadSceneAsync(m_Current.SceneName, LoadSceneMode.Additive);
        }

        public void UnloadEnvironment()
        {
            if (m_Current == null)
            {
                Debug.LogError($"Trying to unload a scene but none is present.");
                
                return;
            }

            SceneManager.UnloadSceneAsync(m_Current.SceneName);

            m_Current = null;
        }
		
		public void RecalcLightProbes()
		{
			LightProbes.Tetrahedralize();
		
			Debug.Log("LightProbes recalculation started");
		}
    }
}