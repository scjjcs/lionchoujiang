using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public abstract class Validation : INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly Dictionary<string, PropertyInfo> _propertyGetters = new Dictionary<string, PropertyInfo>();

        private readonly Dictionary<string, ValidationAttribute[]> _validators = new Dictionary<string, ValidationAttribute[]>();

        private readonly Type _type;

        protected Validation()
        {
            _type = GetType();
            LoadData();
        }
        #region 私有方法
        private void LoadData()
        {
            PropertyInfo[] properties = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in properties)
            {
                //拥有的验证特性
                object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(ValidationAttribute), true);

                if (customAttributes.Length > 0)
                {
                    _validators.Add(propertyInfo.Name, customAttributes as ValidationAttribute[]);
                    _propertyGetters.Add(propertyInfo.Name, propertyInfo);
                }
            }
        }

        /// 
        /// 属性更改通知
        /// 
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region IDataErrorInfo Members

        /// 
        /// 实现IDataErrorInfo接口(获取校验未通过的错误提示)
        /// 

        public string Error
        {
            get
            {
                IEnumerable<string> errors = from d in _validators
                                             from v in d.Value
                                             where !v.IsValid(_propertyGetters[d.Key].GetValue(this, null))
                                             select v.ErrorMessage;
                return string.Join(Environment.NewLine, errors.ToArray());
            }
        }

        /// 
        /// 实现IDataErrorInfo接口
        /// 
        public string this[string columnName]
        {
           get
            {
                if (_propertyGetters.ContainsKey(columnName))
                {
                    object value = _propertyGetters[columnName].GetValue(this, null);
                    string[] errors = _validators[columnName].Where(v => !v.IsValid(value))
                        .Select(v => v.ErrorMessage).ToArray();
                    OnPropertyChanged("Error");
                    return string.Join(Environment.NewLine, errors);
                }
                OnPropertyChanged("Error");
                return string.Empty;
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }



}
