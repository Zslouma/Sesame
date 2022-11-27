
using System;

namespace Syracuse.Mobitheque.Core.Models
{
    public class ApplicationState
    {
        private bool networkConnection;
        public bool NetworkConnection
        {
            get => this.networkConnection;
            set
            {
                if (this.networkConnection == value) return;
                else { this.networkConnection = value; }
                if (OnVariableChange != null)
                {
                    OnVariableChange(this.networkConnection);
                }
            }
        }

        public delegate void OnVariableChangeDelegate(bool networkConnection);
        public event OnVariableChangeDelegate OnVariableChange;

        public ApplicationState(bool networkConnection)
        {
            this.NetworkConnection = networkConnection;
        }

    }
}
