using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DataStoreCollections
{
    public class DataStoreCollectionsEvent : UnityEvent<string>
    {

    }

    public abstract class ASaveAbstractClass<T>  
    {
        protected string IndChange = "";
        public DataStoreCollectionsEvent callUpdate;
        public UnityEvent Change = new UnityEvent();

        public void TestingX() { }
        public virtual void Set(T _val) {
            Val = _val;
            Change.Invoke();
            callUpdate.Invoke(IndChange);
        }
        public T Get() { return Val; }
        protected T Val { get; set; }

        public string GetJson()
        {
            return "Data";
        }

        public void FromJson(string val)  
        {
            
        }

        protected ASaveAbstractClass() {

        }
        public ASaveAbstractClass(T val, string MyChangeEventInd)
        {
            Val = val;
            IndChange = MyChangeEventInd;
        }
    }



    public class number<T> : ASaveAbstractClass<T>  
    {


        public  void Set(T val)
        {
            
            base.Set(val);
        }
         

        public void FromJson(string val)  
        {
            if (typeof(T) == typeof(int))
            {
                int x = int.Parse(val);
            //    Val = int.Parse(val);
            }
                //Val = T.Parse(val);
         }

        public string GetJson()
        {
            return Val.ToString();
        }

        public number(T val, string MyChangeEventInd = "")
            : base(val, MyChangeEventInd)
        {

        }
    }

    public class intData: ASaveAbstractClass<int>
    {

        public bool Take(int val)
        {
            if (Val < val) return false;
            Increment(-val);
            return true;
        }

        public void Increment(int val)
        {
            Val += val;
            base.Set(Val);
        }

        public void Set(int val)
        {   
            base.Set(val);
        }

        public void FromJson(string val)
        {
              Val = int.Parse(val);
        }

        public string GetJson()
        {
            return Val.ToString();
        }
       
        public intData(int val, string MyChangeEventInd = "") 
            : base(val, MyChangeEventInd)
        {
            
        }
    }
}