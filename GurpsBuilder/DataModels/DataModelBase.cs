using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GurpsBuilder.DataModels
{
    public class DataModelBase : INotifyPropertyChanged
    {
        protected string mName = "";
        public virtual string Name
        {
            get { return mName; }
            set
            {
                mName = value;
                OnPropertyChanged("Name");
            }
        }

        #region Interface Implementations

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void BubblePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
