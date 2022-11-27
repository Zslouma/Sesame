using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        public delegate void DisplayAlertDelegate(string title, string message, string button);
        public delegate Task<bool> DisplayAlertDelegateMult(string title, string message, string buttonYes, string buttonNo);
        public event DisplayAlertDelegate OnDisplayAlert;
        public event DisplayAlertDelegateMult OnDisplayAlertMult;

        protected BaseViewModel()
        { }

        internal void DisplayAlert(string title, string message, string button)
        {
            this.OnDisplayAlert?.Invoke(title, message, button);
        }
        internal Task<bool> DisplayAlert(string title, string message, string buttonYes, string buttonNo)
        {
            return this.OnDisplayAlertMult?.Invoke(title, message, buttonYes, buttonNo);
        }

    }

    public abstract class BaseViewModel<TParameter, TResult> : MvxViewModel<TParameter, TResult>
        where TParameter : class
        where TResult : class
    {
        public delegate void DisplayAlertDelegate(string title, string message, string button);
        public delegate Task<bool> DisplayAlertDelegateMult(string title, string message, string buttonYes, string buttonNo);
        public event DisplayAlertDelegate OnDisplayAlert;
        public event DisplayAlertDelegateMult OnDisplayAlertMult;

        protected BaseViewModel()
        { }

        internal void DisplayAlert(string title, string message, string button)
        {
            this.OnDisplayAlert?.Invoke(title, message, button);
        }
        internal Task<bool> DisplayAlert(string title, string message, string buttonYes, string buttonNo)
        {
            return this.OnDisplayAlertMult?.Invoke(title, message, buttonYes, buttonNo);
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource?.TrySetCanceled();

            base.ViewDestroy(viewFinishing);
        }
    }
}
