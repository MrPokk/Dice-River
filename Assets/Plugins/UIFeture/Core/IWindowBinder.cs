#if VCONTAINER_AVAILABLE

using VContainer;

namespace UIFeture.Core
{
    public interface IWindowBinder
    {
        public IWindowBinder Bind(IObjectResolver viewModel);
        public void Open();
        public void Close();
    }
}
#endif
