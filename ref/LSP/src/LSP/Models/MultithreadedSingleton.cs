using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models
{
    public abstract class MultithreadedSingleton<TClass, TInterface> where TClass : class, TInterface, new()
    {
        private static volatile TClass instance = new TClass();
        private static object syncRoot = new Object();

        public static TInterface Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new TClass();
                    }
                }
                return instance;
            }
        }
    }
}