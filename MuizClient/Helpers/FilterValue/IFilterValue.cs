using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MuizClient.Helpers.FilterValue
{
    public interface IFilterValue : INotifyPropertyChanged
    {
        Dictionary<string, object> GetPropertyFilterValues(string property); 
    }

    public interface IFilterValueDefault<T> : IFilterValue
    {
        T Value { get; set; }
        bool Compare(IFilterValueDefault<T> filterValue);
    }

    public abstract class FilterValueDefault<T> : IFilterValueDefault<T>
    {
        public T Value { get; set; }

        public bool Compare(IFilterValueDefault<T> filterValue) => (object)Value == (object)filterValue.Value;

        public Dictionary<string, object> GetPropertyFilterValues(string property)
            => new Dictionary<string, object>() { { property, Value } };


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public interface IFilterValueMinMax<T> : IFilterValue
    {
        T FromValue { get; set; }
        T ToValue { get; set; }

        bool Compare(IFilterValueMinMax<T> filterValue);
    }

    public abstract class FilterValueMinMax<T> : IFilterValueMinMax<T>
    {
        private T fromValue;
        private T toValue;

        public T FromValue 
        {
            get => fromValue;
            set
            {
                fromValue = value;
                OnPropertyChanged();
            }
        }

        public T ToValue
        {
            get => toValue;
            set
            {
                toValue = value;
                OnPropertyChanged();
            }
        }

        public bool Compare(IFilterValueMinMax<T> filterValue) 
            => (object)FromValue == (object)filterValue.FromValue
                && (object)ToValue == (object)filterValue.ToValue;

        public Dictionary<string, object> GetPropertyFilterValues(string property)
            => new Dictionary<string, object>() { { property + "From", FromValue }, { property + "To", ToValue }};


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
