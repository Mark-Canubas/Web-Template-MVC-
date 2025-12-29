using System;
using System.Web;

namespace LoginMVC.Services
{
    public interface ISessionService
    {
        void Set<T>(string key, T value);
        T Get<T>(string key);
        bool TryGet<T>(string key, out T value);
        void Remove(string key);
        void Clear();           
    }

    public sealed class SessionService : ISessionService
    {
        private readonly HttpSessionStateBase _session;

        public SessionService(HttpSessionStateBase session)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public void Set<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key is required.", nameof(key));
            _session[key] = value;
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key is required.", nameof(key));

            var obj = _session[key];
            if (obj == null) return default;

            if (obj is T t) return t;

            throw new InvalidCastException($"Session key '{key}' is '{obj.GetType().FullName}', not '{typeof(T).FullName}'.");
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default;

            if (string.IsNullOrWhiteSpace(key))
                return false;

            var obj = _session[key];
            if (obj == null)
                return false;

            if (obj is T t)
            {
                value = t;
                return true;
            }

            return false;
        }

        public void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return;
            _session.Remove(key);
        }

        public void Clear()
        {
            _session.Clear();
            _session.Abandon();
        }
    }
}