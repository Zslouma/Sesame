using MvvmCross.ViewModels;

namespace Syracuse.Mobitheque.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        public delegate void DisplayAlertDelegate(string title, string message, string button);
        public event DisplayAlertDelegate OnDisplayAlert;

        protected BaseViewModel()
        { }

        internal void DisplayAlert(string title, string message, string button)
        {
            this.OnDisplayAlert?.Invoke(title, message, button);
        }

    }

    public abstract class BaseViewModel<TParameter, TResult> : MvxViewModel<TParameter, TResult>
        where TParameter : class
        where TResult : class
    {
        public delegate void DisplayAlertDelegate(string title, string message, string button);
        public event DisplayAlertDelegate OnDisplayAlert;

        protected BaseViewModel()
        { }

        internal void DisplayAlert(string title, string message, string button)
        {
            this.OnDisplayAlert?.Invoke(title, message, button);
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            if (viewFinishing && CloseCompletionSource != null && !CloseCompletionSource.Task.IsCompleted && !CloseCompletionSource.Task.IsFaulted)
                CloseCompletionSource?.TrySetCanceled();

            base.ViewDestroy(viewFinishing);
        }
    }
}
