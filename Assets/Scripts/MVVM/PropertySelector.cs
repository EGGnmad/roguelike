using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using UniRx;
using Component = UnityEngine.Component;

namespace MVVM
{
    [Serializable]
    public class PropertySelector
    {
        public Component obj;
        private PropertyInfo _property;

        public void Select()
        {
            _property.GetValue(obj).ObserveEveryValueChanged(x => x).Subscribe((x) =>
            {
                OnPropertyChanged(_property.Name);
            });
        }

        public void AddListener(PropertyChangedEventHandler func)
        {
            Select();
            PropertyChanged += func;
        }

        #region Methods:Notify

        protected event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(obj, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}