using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // TODO: It would be better to get the model from the CharacterData instead of authoring it into some prefab variants.
    // TODO: Find a proper folder for this file.
    public class CharacterShowcase : MonoBehaviour
    {
        public enum CameraPositions
        {
            None = 0,
            FullBody = 1,
            CloseUp = 2,
            HalfBody = 3,
            Right = 4,
            Left = 5
        }
        
        [SerializeField] private Transform m_FullBodyCameraPosition = null;

        [SerializeField] private Transform m_CloseUpCameraPosition = null;
        
        [SerializeField] private Transform m_HalfBodyCameraPosition = null;

        [SerializeField] private Transform m_RightCameraPosition = null;
        
        [SerializeField] private Transform m_LeftCameraPosition = null;
        
        [SerializeField] private Camera m_Camera = null;
        
        [SerializeField] private RenderTexture m_RenderTextureTemplate = null;
        
        private RenderTexture m_RenderTexture = null;

        // TODO: Make a CharacterShowCaseInstance class to avoid giving the chance to access this field from CharacterData.
        public RenderTexture ImageTexture => m_RenderTexture;

        private static List<ShowcasePosition> m_Positions = new List<ShowcasePosition>()
        {
            new ShowcasePosition()
        };

        private static Dictionary<object, List<CharacterShowcase>> m_Instances =
            new Dictionary<object, List<CharacterShowcase>>(0);

        private Dictionary<CameraPositions, Transform> m_PostitonTransformMap = null;
        
        private void Awake()
        {
            m_RenderTexture = new RenderTexture(m_RenderTextureTemplate);

            m_Camera.targetTexture = m_RenderTexture;

            m_PostitonTransformMap = new Dictionary<CameraPositions, Transform>()
            {
                { CameraPositions.None, null },
                { CameraPositions.FullBody, m_FullBodyCameraPosition },
                { CameraPositions.CloseUp, m_CloseUpCameraPosition },
                { CameraPositions.HalfBody, m_HalfBodyCameraPosition },
                { CameraPositions.Right, m_RightCameraPosition },
                { CameraPositions.Left, m_LeftCameraPosition },
            };
        }
        
        // TODO: This is confusing. This method should return an Instance nested class else we would be able to call GetInstance on instances.
        public CharacterShowcase GetInstance(object owner, CameraPositions cameraPosition)
        {
            CharacterShowcase characterShowcase = null;
            
            foreach (var position in m_Positions)
            {
                if (position.user == null)
                {
                    return SpawnAtPosition(position);
                }
            }

            ShowcasePosition newPosition = new ShowcasePosition();
            newPosition.position = m_Positions[m_Positions.Count-1].position + Vector3.one * 10f;
            
            m_Positions.Add(newPosition);
            
            return SpawnAtPosition(newPosition);

            CharacterShowcase SpawnAtPosition(ShowcasePosition position)
            {
                characterShowcase = GameObject.Instantiate(this, position.position, Quaternion.identity)
                    .GetComponent<CharacterShowcase>();
                position.user = characterShowcase;

                characterShowcase.ChangeCameraPosition(cameraPosition);

                if (m_Instances.ContainsKey(owner) == false)
                {
                    m_Instances.Add(owner, new List<CharacterShowcase>(0));
                }
                
                m_Instances[owner].Add(characterShowcase);

                return characterShowcase;
            }
        }

        private void ChangeCameraPosition(CameraPositions newCameraPosition)
        {
            m_Camera.transform.position = m_PostitonTransformMap[newCameraPosition].transform.position;
            m_Camera.transform.rotation = m_PostitonTransformMap[newCameraPosition].transform.rotation;
        }

        public static void ClearByOwner(object owner)
        {
            if (m_Instances.ContainsKey(owner) == false)
            {
                return;
            }

            foreach (var characterShowcase in m_Instances[owner])
            {
                FreePosition(characterShowcase);   
            }

            m_Instances.Remove(owner);
        }

        private static void FreePosition(CharacterShowcase characterShowcase)
        {
            foreach (var position in m_Positions)
            {
                if (position.user == characterShowcase)
                {
                    // TODO: Test as this can be useless, if I destroy a GameObject I expect this variable to become null.
                    position.user = null;
                    
                    Destroy(characterShowcase.gameObject);
                }
            }
        }

        private class ShowcasePosition
        {
            public Vector3 position = Vector3.one * 100f;
            public CharacterShowcase user = null;
        }
    }

    // public class CharacterImage : MonoBehaviour
    // {
    //     [SerializeField] private RawImage m_Image = null;
    //
    //     [SerializeField] private AspectRatioFitter m_AspectRatioFitter = null;
    //
    //     private CharacterShowcase m_Showcase = null;
    //
    //     public CharacterShowcase Showcase
    //     {
    //         set
    //         {
    //             value.GetInstance()    
    //         }
    //     }
    // }
}