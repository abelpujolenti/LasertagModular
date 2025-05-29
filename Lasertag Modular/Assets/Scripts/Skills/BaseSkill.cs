using UnityEngine;

namespace Agent
{
    public abstract class BaseSkill
    {

        private string _name = "";
        private bool _canUse = true;

        public virtual void TryUseAbility() {

            if (CanUseAbility())
            {
                UseAbility();
                AbilityUsed(true);
            }
            else
            {
                AbilityUsed(false);
            } 
        }

        public abstract void UseAbility();

        protected virtual bool CanUseAbility() { 
            return _canUse;
        }

        protected void AbilityUsed(bool used)
        {
            if (used)
            {
                _canUse = false;
                OnAbilityUsed();
            }
            else
            {
                OnAbilityFailed();
            }
        }

        //Feedback
        protected abstract void OnAbilityUsed();
        protected abstract void OnAbilityFailed();

    }

}
