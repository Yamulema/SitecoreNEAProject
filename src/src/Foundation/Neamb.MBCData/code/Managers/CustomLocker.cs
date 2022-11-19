using Neambc.Neamb.Foundation.DependencyInjection;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    [Service(typeof(ICustomLocker))]
    public class CustomLocker: ICustomLocker
    {
        private readonly object _syncObject;
        private const int TimeoutMaxWait = 500; // milliseconds

        public CustomLocker() {
            _syncObject = new object();
        }
        public KeyLocker Lock() {

            if (System.Threading.Monitor.TryEnter(this._syncObject, TimeoutMaxWait))
                return new KeyLocker(this._syncObject);
            else
                // throw exception, log message, etc.
                throw new System.TimeoutException("Process already locked. It is not possible to start a new one");
        }
    }
}