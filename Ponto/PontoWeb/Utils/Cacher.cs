using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PontoWeb.Utils
{
    public class Cacher<TValue>
    where TValue : class
    {
        #region Properties
        private Func<TValue> _init;
        public string Key { get; private set; }
        public TValue Value
        {
            get
            {
                return GetSetCache();
            }
        }

        private TValue GetSetCache()
        {

            var item = HttpContext.Current.Session[Key] as TValue;
            if (item == null)
            {
                item = _init();
                HttpContext.Current.Session.Add(Key,item);
            }
            return item;
        }
        #endregion

        #region Constructor
        public Cacher(string key, Func<TValue> init)
        {
            Key = key;
            _init = init;
        }
        #endregion

        #region Methods
        public void AlteraCache(Func<TValue> init)
        {
            _init = init;
            Refresh();
        }
        public void Refresh()
        {
            HttpContext.Current.Session.Remove(Key);
            GetSetCache();
        }

        public void Clear()
        {
            HttpContext.Current.Session.Remove(Key);
        }
        #endregion
    }
}