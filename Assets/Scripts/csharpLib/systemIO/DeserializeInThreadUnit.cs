using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using thread;

namespace systemIO
{
    public class DeserializeInThreadUnit<T>
    {
        private Action<T> callBack;
        private byte[] bytes;
        private T result;

        public void Init(byte[] _bytes, Action<T> _callBack)
        {
            bytes = _bytes;

            callBack = _callBack;

            ThreadScript.Instance.Add(GetData, LoadOver);
        }

        private void GetData()
        {
            using (Stream stream = new MemoryStream(bytes))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                result = (T)formatter.Deserialize(stream);
            }
        }

        private void LoadOver()
        {
            callBack(result);
        }
    }
}