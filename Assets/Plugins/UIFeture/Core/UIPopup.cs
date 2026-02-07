
#if VCONTAINER_AVAILABLE

using UnityEngine;

namespace UIFeture.Core
{
    public class UIPopup : WindowBinder
    {
        [SerializeField] private IUIProvider _btnClose;
        [SerializeField] private IUIProvider _btnAlternativeClose;

        public override void Open()
        {
            AddListeners();
            base.Open();
        }

        public override void Close()
        {
            RemoveListeners();
            base.Close();
        }

        private void AddListeners()
        {
            _btnClose?.AddListener(OnCloseClicked);
            _btnAlternativeClose?.AddListener(OnCloseClicked);
        }

        private void RemoveListeners()
        {
            _btnClose?.RemoveListener(OnCloseClicked);
            _btnAlternativeClose?.RemoveListener(OnCloseClicked);
        }

        private void OnCloseClicked() => Close();
    }
}
#endif
