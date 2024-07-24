using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace DialogueSystem
{
    public class CharacterCamera : MonoBehaviour
    {
        [SerializeField] private GameObject m_CloseUpCamera = null;

        [SerializeField] private GameObject m_HalfBodyCamera = null;

        [SerializeField] private GameObject m_ShoulderLevelCamera = null;

        [SerializeField] private GameObject m_ShoulderLevelCamera_Scenario01_RaviJake = null;

        [SerializeField] private GameObject m_Scenario02_Triangle_AishaMiguel = null;

        [SerializeField] private GameObject m_Scenario02_Triangle_MiguelAisha = null;

        [SerializeField] private GameObject m_Scenario02_Triangle_AishaMira = null;

        [SerializeField] private GameObject m_Scenario02_Triangle_MiraAisha = null;

        [SerializeField] private GameObject m_Scenario02_Triangle_MiraMiguel = null;

        [SerializeField] private GameObject m_Scenario02_Triangle_MiguelMira = null;

        [SerializeField] private GameObject m_Scenario02_EstablishingShot = null;

        [SerializeField] private GameObject m_Scenario02_Report_AishaAnna = null;

        [SerializeField] private GameObject m_Scenario02_Report_AnnaAisha = null;

        [SerializeField] private GameObject m_Scenario02_Report_EstablishingShot = null;

        [SerializeField] private GameObject m_Scenario02_Report_AishaAnna_Sitting = null;

        [SerializeField] private GameObject m_Scenario02_Report_AnnaAisha_Sitting = null;

        [SerializeField] private GameObject m_Scenario02_Report_AishaAnna_Sitting_CloseUp = null;

        [SerializeField] private GameObject m_Scenario02_Report_AnnaAisha_Sitting_CloseUp = null;

        [SerializeField] private GameObject m_ShoulderLevelCamera_Scenario03_KaiSmartphoneShot = null;

        [SerializeField] private GameObject m_Scenario04_MariaJake = null;

        [SerializeField] private GameObject m_Scenario04_JakeMaria = null;

        [SerializeField] private GameObject m_Scenario04_SamuelMaria = null;

        [SerializeField] private GameObject m_Scenario04_EstablishingShot = null;

        [SerializeField] private GameObject m_Scenario04_MariaJake_CloseUp = null;

        [SerializeField] private GameObject m_Scenario04_DanJake = null;

        [SerializeField] private GameObject m_Scenario04_EstablishingShot_SamuelMaria = null;

        [SerializeField] private GameObject m_ShoulderLevelCamera_Scenario05_AdamLeansForward = null;

        [SerializeField] private GameObject m_ShoulderLevelCamera_Scenario05_AdamLooksSophia = null;

        [SerializeField] private GameObject m_Scenario05_EstablishingShot = null;

        [SerializeField] private GameObject m_Scenario05_AishaListens = null;

        [SerializeField] private GameObject m_Scenario05_AishaIntervention = null;

        [SerializeField] private GameObject m_Scenario05_SofiaSmartphone = null;

        [SerializeField] private GameObject m_Scenario03_EstablishingShot = null;

        [SerializeField] private GameObject m_Scenario03_KaiEva_Sitting_CloseUp = null;

        [SerializeField] private GameObject m_Scenario03_Dialogue01_FinalShot = null;

        [SerializeField] private GameObject m_Scenario03_Dialogue02_MiraEva = null;

        [SerializeField] private GameObject m_Scenario03_EvaSelfie = null;

        [SerializeField] private GameObject m_Scenario03_KaiLookAtSmartphone = null;

        [SerializeField] private GameObject m_Scenario03_KaiNotification = null;

        [SerializeField] private GameObject m_Scenario03_Dialogue02_JakeSamuel = null;

        [SerializeField] private GameObject m_Scenario03_Dialogue02_MiguelMira = null;

        private Dictionary<CameraPositions, GameObject> m_CameraMap = null;

        private GameObject m_CurrentActive = null;
        
        public enum CameraPositions
        {
            None = 0,
            CloseUpCamera,
            HalfBodyCamera,
            ShoulderLevelCamera,
            ShoulderLevelCamera_Scenario01_RaviJake,
            ShoulderLevelCamera_Scenario03_KaiSmartphoneShot,
            ShoulderLevelCamera_Scenario05_AdamLeansForward,
            Scenario05_EstablishingShot,
            Scenario05_AishaListens,
            Scenario05_AishaIntervention,
            Scenario02_Triangle_AishaMiguel,
            Scenario02_Triangle_MiguelAisha,
            Scenario02_Triangle_AishaMira,
            Scenario02_Triangle_MiraAisha,
            Scenario02_Triangle_MiraMiguel,
            Scenario02_Triangle_MiguelMira,
            Scenario02_EstablishingShot,
            Scenario02_Report_AishaAnna,
            Scenario02_Report_AnnaAisha,
            Scenario02_Report_EstablishingShot,
            Scenario02_Report_AishaAnna_Sitting,
            Scenario02_Report_AnnaAisha_Sitting,
            Scenario02_Report_AishaAnna_Sitting_CloseUp,
            Scenario02_Report_AnnaAisha_Sitting_CloseUp,
            Scenario04_MariaJake,
            Scenario04_JakeMaria,
            Scenario04_SamuelMaria,
            Scenario04_EstablishingShot,
            Scenario04_MariaJake_CloseUp,
            Scenario04_DanJake,
            Scenario04_EstablishingShot_SamuelMaria,
            Scenario05_SofiaSmartphone,
            Scenario03_EstablishingShot,
            Scenario03_KaiEva_Sitting_CloseUp,
            Scenario03_Dialogue01_FinalShot,
            Scenario03_Dialogue02_MiraEva,
            Scenario03_EvaSelfie,
            Scenario03_KaiLookAtSmartphone,
            Scenario03_KaiNotification,
            Scenario03_Dialogue2_JakeSamuel,
            Scenario03_Dialogue2_MiguelMira,
            ShoulderLevelCamera_Scenario05_AdamLooksSophia
        }

        private void Start()
        {
            m_CameraMap = new Dictionary<CameraPositions, GameObject>()
            {
                { CameraPositions.None, null },
                { CameraPositions.CloseUpCamera, m_CloseUpCamera },
                { CameraPositions.HalfBodyCamera, m_HalfBodyCamera },
                { CameraPositions.ShoulderLevelCamera, m_ShoulderLevelCamera },
                { CameraPositions.ShoulderLevelCamera_Scenario01_RaviJake, m_ShoulderLevelCamera_Scenario01_RaviJake },
                { CameraPositions.ShoulderLevelCamera_Scenario03_KaiSmartphoneShot, m_ShoulderLevelCamera_Scenario03_KaiSmartphoneShot },
                { CameraPositions.ShoulderLevelCamera_Scenario05_AdamLeansForward, m_ShoulderLevelCamera_Scenario05_AdamLeansForward },
                { CameraPositions.ShoulderLevelCamera_Scenario05_AdamLooksSophia, m_ShoulderLevelCamera_Scenario05_AdamLooksSophia },
                { CameraPositions.Scenario05_EstablishingShot, m_Scenario05_EstablishingShot },
                { CameraPositions.Scenario05_AishaListens, m_Scenario05_AishaListens },
                { CameraPositions.Scenario05_AishaIntervention, m_Scenario05_AishaIntervention },
                { CameraPositions.Scenario02_Triangle_AishaMiguel, m_Scenario02_Triangle_AishaMiguel },
                { CameraPositions.Scenario02_Triangle_MiguelAisha, m_Scenario02_Triangle_MiguelAisha },
                { CameraPositions.Scenario02_Triangle_AishaMira, m_Scenario02_Triangle_AishaMira },
                { CameraPositions.Scenario02_Triangle_MiraAisha, m_Scenario02_Triangle_MiraAisha },
                { CameraPositions.Scenario02_Triangle_MiraMiguel, m_Scenario02_Triangle_MiraMiguel },
                { CameraPositions.Scenario02_Triangle_MiguelMira, m_Scenario02_Triangle_MiguelMira },
                { CameraPositions.Scenario02_EstablishingShot, m_Scenario02_EstablishingShot },
                { CameraPositions.Scenario02_Report_AishaAnna, m_Scenario02_Report_AishaAnna },
                { CameraPositions.Scenario02_Report_AnnaAisha, m_Scenario02_Report_AnnaAisha },
                { CameraPositions.Scenario02_Report_EstablishingShot, m_Scenario02_Report_EstablishingShot },
                { CameraPositions.Scenario02_Report_AishaAnna_Sitting, m_Scenario02_Report_AishaAnna_Sitting },
                { CameraPositions.Scenario02_Report_AnnaAisha_Sitting, m_Scenario02_Report_AnnaAisha_Sitting },
                { CameraPositions.Scenario02_Report_AishaAnna_Sitting_CloseUp, m_Scenario02_Report_AishaAnna_Sitting_CloseUp },
                { CameraPositions.Scenario02_Report_AnnaAisha_Sitting_CloseUp, m_Scenario02_Report_AnnaAisha_Sitting_CloseUp },
                { CameraPositions.Scenario04_MariaJake, m_Scenario04_MariaJake },
                { CameraPositions.Scenario04_JakeMaria, m_Scenario04_JakeMaria },
                { CameraPositions.Scenario04_SamuelMaria, m_Scenario04_SamuelMaria },
                { CameraPositions.Scenario04_EstablishingShot, m_Scenario04_EstablishingShot },
                { CameraPositions.Scenario04_MariaJake_CloseUp, m_Scenario04_MariaJake_CloseUp },
                { CameraPositions.Scenario04_DanJake, m_Scenario04_DanJake },
                { CameraPositions.Scenario04_EstablishingShot_SamuelMaria, m_Scenario04_EstablishingShot_SamuelMaria },
                { CameraPositions.Scenario05_SofiaSmartphone, m_Scenario05_SofiaSmartphone },
                { CameraPositions.Scenario03_EstablishingShot, m_Scenario03_EstablishingShot },
                { CameraPositions.Scenario03_KaiEva_Sitting_CloseUp, m_Scenario03_KaiEva_Sitting_CloseUp },
                { CameraPositions.Scenario03_Dialogue01_FinalShot, m_Scenario03_Dialogue01_FinalShot },
                { CameraPositions.Scenario03_Dialogue02_MiraEva, m_Scenario03_Dialogue02_MiraEva },
                { CameraPositions.Scenario03_EvaSelfie, m_Scenario03_EvaSelfie },
                { CameraPositions.Scenario03_KaiLookAtSmartphone, m_Scenario03_KaiLookAtSmartphone },
                { CameraPositions.Scenario03_KaiNotification, m_Scenario03_KaiNotification },
                { CameraPositions.Scenario03_Dialogue2_JakeSamuel, m_Scenario03_Dialogue02_JakeSamuel},
                { CameraPositions.Scenario03_Dialogue2_MiguelMira, m_Scenario03_Dialogue02_MiguelMira }
            };
        }

        public void ActivateCamera(CameraPositions position)
        {
            if (m_CurrentActive != null)
            {
                Debug.LogError($"");
                
                return;
            }

            m_CurrentActive = m_CameraMap[position];
            
            m_CameraMap[position].SetActive(true);
        }
        
        public void DeactivateCamera()
        {
            if (m_CurrentActive == null)
            {
                Debug.LogError($"");
                
                return;
            }
            
            m_CurrentActive.SetActive(false);
            m_CurrentActive = null;
        }
    }
}