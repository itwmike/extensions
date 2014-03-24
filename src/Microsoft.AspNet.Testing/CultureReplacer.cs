using System;
using System.Globalization;
using System.Threading;
using Xunit;

namespace Microsoft.AspNet.Testing
{
    public class CultureReplacer : IDisposable
    {
        private const string _defaultCultureName = "en-GB";
        private const string _defaultUICultureName = "en-US";
        private static readonly CultureInfo _defaultCulture = new CultureInfo(_defaultCultureName);
        private readonly CultureInfo _originalCulture;
        private readonly CultureInfo _originalUICulture;
        private readonly long _threadId;

        // Culture => Formatting of dates/times/money/etc, defaults to en-GB because en-US is the same as InvariantCulture
        // We want to be able to find issues where the InvariantCulture is used, but a specific culture should be.
        //
        // UICulture => Language
        public CultureReplacer(string culture = _defaultCultureName, string uiCulture = _defaultUICultureName)
        {
            _originalCulture = CultureInfo.DefaultThreadCurrentCulture;
            _originalUICulture = CultureInfo.DefaultThreadCurrentUICulture;
            _threadId = Thread.CurrentThread.ManagedThreadId;
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(uiCulture);
        }

        /// <summary>
        /// The name of the culture that is used as the default value for CultureInfo.DefaultThreadCurrentCulture when CultureReplacer is used.
        /// </summary>
        public static string DefaultCultureName
        {
            get { return _defaultCultureName; }
        }

        /// <summary>
        /// The name of the culture that is used as the default value for CultureInfo.DefaultThreadCurrentUICulture when CultureReplacer is used.
        /// </summary>
        public static string DefaultUICultureName
        {
            get { return _defaultUICultureName; }
        }

        /// <summary>
        /// The culture that is used as the default value for CultureInfo.DefaultThreadCurrentCulture when CultureReplacer is used.
        /// </summary>
        public static CultureInfo DefaultCulture
        {
            get { return _defaultCulture; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Assert.True(Thread.CurrentThread.ManagedThreadId == _threadId,
                    "The current thread is not the same as the thread invoking the constructor. This should never happen.");
                CultureInfo.DefaultThreadCurrentCulture = _originalCulture;
                CultureInfo.DefaultThreadCurrentUICulture = _originalUICulture;
            }
        }
    }
}