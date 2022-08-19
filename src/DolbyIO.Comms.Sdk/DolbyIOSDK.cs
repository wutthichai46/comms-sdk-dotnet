using System;
using System.Threading.Tasks;
using DolbyIO.Comms.Services;

namespace DolbyIO.Comms 
{
    public class DolbyIOException : Exception
    {
        public DolbyIOException(String msg)
            : base(msg)
        {
        }
    }

    /// <summary>
    /// The DolbyIOSDK Class is a starting point that allows initializing the
    /// C# SDK and accessing the underlying services.
    /// </summary>
    public class DolbyIOSDK : IDisposable
    {   
        private volatile bool _initialised = false;

        private Session _session = new Session();
        private Conference _conference = new Conference();
        private MediaDevice _mediaDevice = new MediaDevice();

        public SignalingChannelErrorEventHandler SignalingChannelError
        {
            set 
            { 
                if (!_initialised)
                {
                    throw new DolbyIOException("DolbyIOSDK is not initialized!");
                }
                Native.SetOnSignalingChannelExceptionHandler(value);
            }
        }

        public InvalidTokenErrorEventHandler InvalidTokenError
        {
            set
            { 
                if (!_initialised)
                {
                    throw new DolbyIOException("DolbyIOSDK is not initialized!");
                }
                Native.SetOnInvalidTokenExceptionHandler(value);
            }
        }

        ~DolbyIOSDK()
        {
            Dispose(false);
        }

        /// <summary>
        /// The Session service accessor.
        /// </summary>
        public Session Session 
        {
            get 
            { 
                if (!_initialised)
                {
                    throw new DolbyIOException("DolbyIOSDK is not initialized!");
                }
                return _session; 
            } 
        }
        
        /// <summary>
        /// The Conference service accessor.
        /// </summary>
        public Conference Conference 
        {
            get 
            { 
                if (!_initialised)
                {
                    throw new DolbyIOException("DolbyIOSDK is not initialized!");
                }
                return _conference; 
            } 
        }

        /// <summary>
        /// The MediaDevice service accessor.
        /// </summary>
        public MediaDevice MediaDevice
        {
            get
            {
                if (!_initialised)
                {
                    throw new DolbyIOException("DolbyIOSDK is not initialized!");
                }
                return _mediaDevice;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appKey">Application secret key</param>
        public async Task Init(String appKey)
        {
            if (_initialised)
            {
                throw new DolbyIOException("Already initialized, call Dispose first.");
            }
            
            await Task.Run(() =>
            {
                Native.CheckException(Native.Init(appKey));
                _initialised = true;
            });
        }

        public async Task SetLogLevel(LogLevel level)
        {
            if (!_initialised)
            {
                throw new DolbyIOException("DolbyIOSDK is not initialized!");
            }
            await Task.Run(() =>  Native.CheckException(Native.SetLogLevel(level)));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (_initialised)
            {
                Native.CheckException(Native.Release());
                _initialised = false;
            }
        }
    }
}
