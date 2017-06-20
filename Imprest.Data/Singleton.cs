using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imprest.Data.Facct;
using Imprest.Data;

namespace Imprest.Data
{
    public  class Singleton<T> where T : new()
    {
       
         static T singleObject;
        public static  T getInstance()
        {
           if(singleObject != null)
            {
                return singleObject;
            }
            else
            {
                return singleObject = new T();
            }
        }
    }
}
