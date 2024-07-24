using System.Collections;
using System.Collections.Generic;
using Core;
using Managers;
using UnityEngine;

namespace UI
{
    public abstract class BaseUI : MonoBehaviour
    {
        public abstract void Show();

        public abstract void Hide();

        public abstract void Interrupt();

        public abstract bool IsOnScreen();
    }

    public abstract class BaseUI<T> : BaseUI
    where T : Behaviour
    {
        // Friendly reminder to myself: Never forget again a SerializeField else you would curse the gods because the build won't run ʘ‿ʘ
        [SerializeField] protected T m_Behaviour = null;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            m_Behaviour = GetComponent<T>();

            if (m_Behaviour == null)
            {
                Debug.LogError(
                    $"{gameObject.name} doesn't have a {m_Behaviour.GetType().Name} attached. {this.GetType().Name} can't work properly.");
            }
        }
#endif
    }
    
    public abstract class BaseUICanvas : BaseUI<Canvas>
    {
        public override void Show()
        {
            m_Behaviour.enabled = true;

            this.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            m_Behaviour.enabled = false;

            this.gameObject.SetActive(false);
        }

        public override void Interrupt() => Hide();

        public override bool IsOnScreen() => m_Behaviour.enabled == true;

#if UNITY_EDITOR
        private void OnValidate()
        {
            m_Behaviour.enabled = false;

            this.gameObject.SetActive(false);
        }
#endif
    }

    public abstract class BaseUICanvasGroup : BaseUI<CanvasGroup>
    {
        [SerializeField] private bool b_FadeIn = true;
        [SerializeField] private bool b_FadeOut = true;

        private Queue<IEnumerator> m_QueuedOperations = new Queue<IEnumerator>(0);

        public virtual bool IsDone() => UICoroutinesHandler.Instance.IsAlive(this) == false && m_QueuedOperations.Count == 0;

        // TODO: Evaluate if it's really necessary to handle the canvas here, it should suffice to handle CanvasGroup state
        private Canvas m_Canvas = null;

        public virtual bool CanPause => true;
        
        public override bool IsOnScreen() => Mathf.Approximately(m_Behaviour.alpha, 1f) &&
                                             (m_Canvas == null || m_Canvas.enabled == true);

        protected virtual void Awake()
        {
            m_Canvas = GetComponent<Canvas>();
        }

        public sealed override void Show()
        {
            if (b_FadeIn)
            {
                Enqueue(Helpers.UI.COR_Fade(m_Behaviour, 0f, 1f, GameManager.Instance.DevSettings.BaseFadeDuration, OnShowCompleted, OnShowStart, CanPause));
            }
            else
            {
                Enqueue(COR_ShowImmediate());
                
                IEnumerator COR_ShowImmediate()
                {
                    m_Behaviour.alpha = 1f;
                
                    OnShowStart();
                    OnShowCompleted();

                    yield return null;
                }
            }
        }

        protected virtual void OnShowStart()
        {
            this.gameObject.SetActive(true);

            if (m_Canvas != null)
            {
                m_Canvas.enabled = true;
            }    
        }
        
        protected virtual void OnShowCompleted()
        {
            m_Behaviour.interactable = true;
            m_Behaviour.blocksRaycasts = true;
            
            var accessibleUIGroupRoot = this.GetComponent<AccessibleUIGroupRoot>();
            if (accessibleUIGroupRoot != null)
            {
                accessibleUIGroupRoot.enabled = true;
            }
        }

        public sealed override void Hide()
        {
            if (b_FadeOut)
            {
                Enqueue(Helpers.UI.COR_Fade(m_Behaviour, 1f, 0f, GameManager.Instance.DevSettings.BaseFadeDuration, OnHideCompleted, OnHideStart, CanPause));
            }
            else
            {
                Enqueue(COR_HideImmediate());
                
                IEnumerator COR_HideImmediate()
                {
                    m_Behaviour.alpha = 0f;
                
                    OnHideStart();
                    OnHideCompleted();

                    yield return null;
                }
            }
        }

        protected virtual void OnHideStart() { }

        protected virtual void OnHideCompleted()
        {
            if (m_Canvas != null)
            {
                m_Canvas.enabled = false;
            }

            var accessibleUIGroupRoot = this.GetComponent<AccessibleUIGroupRoot>();
            if (accessibleUIGroupRoot != null)
            {
                accessibleUIGroupRoot.enabled = false;
            }
            
            m_Behaviour.interactable = false;
            m_Behaviour.blocksRaycasts = false;

            this.gameObject.SetActive(false);
        }
        
        public override void Interrupt()
        {
            Hide();
        }
        
        private IEnumerator ConsumeQueuedOperations()
        {
            while (m_QueuedOperations.Count > 0)
            {
                yield return m_QueuedOperations.Dequeue();
            }
        }

        private void Enqueue(IEnumerator enumerator)
        {
            m_QueuedOperations.Enqueue(enumerator);

            UICoroutinesHandler.Instance.TryStartCoroutine(this, ConsumeQueuedOperations());
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // m_Behaviour.enabled = false;
            m_Behaviour.interactable = false;
            m_Behaviour.blocksRaycasts = false;

            this.gameObject.SetActive(false);
        }
#endif
    }

    // Beware that this class originally derived from BaseUICanvas.
    public abstract class PanelContainer<T> : BaseUICanvasGroup
    where T : BaseUI
    {
        [SerializeField] private T m_StartingPanel = null;
        
        private T m_Current = null;

        protected override void OnShowStart()
        {
            base.OnShowStart();
            
            m_Current = m_StartingPanel;
            
            m_Current.Show();
        }

        protected override void OnHideCompleted()
        {
            base.OnHideCompleted();
            
            m_Current?.Hide();

            m_Current = null;
        }

        public void ChangePanel(T newPanel)
        {
            m_Current.Hide();

            m_Current = newPanel;
            
            newPanel.Show();
        }
    }

    public abstract class PanelContainerUI<T> : BaseUICanvas
    where T : PanelContainerUI<T>.Panel
    {
        [SerializeField] private T m_StartingPanel = null;

        private T m_Current = null;

        protected virtual void Start()
        {
            foreach (var panel in GetComponentsInChildren<T>())
            {
                panel.SetOwner(this);  
            }
        }

        public override void Show()
        {
            base.Show();

            m_Current = m_StartingPanel;
            
            m_Current.Show();
        }

        public override void Hide()
        {
            base.Hide();
            
            m_Current.Hide();

            m_Current = null;
        }

        public virtual void ChangePanel(T newPanel)
        {
            m_Current.Hide();

            m_Current = newPanel;
            
            newPanel.Show();
        }

        public abstract class Panel : BaseUICanvas
        {
            protected PanelContainerUI<T> m_Owner = null;

            public void SetOwner(PanelContainerUI<T> owner)
            {
                m_Owner = owner;
            }
        }
    }
}