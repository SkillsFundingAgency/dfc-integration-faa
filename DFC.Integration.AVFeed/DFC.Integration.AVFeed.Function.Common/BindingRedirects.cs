namespace DFC.Integration.AVFeed.Function.Common
{
    using System;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// Congiure binding redirects for dependant assemblies
    /// /// </summary>
    public static class BindingRedirects
    {
        /// <summary>
        /// Set the version redirects for dependant assemblies
        /// </summary>
        public static void Configure()
        {
            RedirectAssembly("Autofac", new Version("4.6.1.0"), "17863af14b0044da");
            RedirectAssembly("NLog", new Version("4.0.0.0"), "5120e14c03d0593c");
            RedirectAssembly("Castle.Core", new Version("4.0.0.0"), "407dd0808d44fbdc");
            RedirectAssembly("Autofac.Extras.DynamicProxy", new Version("4.2.1.0"), "17863af14b0044da");
            RedirectAssembly("System.Diagnostics.DiagnosticSource", new Version("4.0.2.1"), "cc7b13ffcd2ddd51");
        }

        private static void RedirectAssembly(string shortName, Version targetVersion, string publicKeyToken)
        {
            ResolveEventHandler handler = null;

            handler = (sender, args) =>
            {
                var requestedAssembly = new AssemblyName(args.Name)
                {
                    Version = targetVersion,
                    CultureInfo = CultureInfo.InstalledUICulture,
                };

                if (requestedAssembly.Name != shortName)
                {
                    return null;
                }

                var targetPublicKeyToken = new AssemblyName($"x, PublicKeyToken={publicKeyToken}").GetPublicKeyToken();
                requestedAssembly.SetPublicKeyToken(targetPublicKeyToken);

                AppDomain.CurrentDomain.AssemblyResolve -= handler;

                return Assembly.Load(requestedAssembly);
            };

            AppDomain.CurrentDomain.AssemblyResolve += handler;
        }
    }
}