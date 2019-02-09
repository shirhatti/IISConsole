using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISConsole.IISConfiguration
{
    public static class IISConfig
    {
        public static Configuration CreateDefaultConfig()
        {
            var config = new Configuration()
            {
                ApplicationHost = new ApplicationHost
                {
                    ApplicationPools = new ApplicationPoolsSection
                    {
                        ApplicationPools = new ApplicationPool[]
                        {
                            new ApplicationPool
                            {
                                Name = "DefaultAppPool"
                            }
                        }
                    }
                },
                WebServer = new WebServer
                {
                    globalModules = new GlobalModule[]
                    {
                        new GlobalModule
                        {
                            Name = "AnonymousAuthenticationModule",
                            Image = @"%windir%\System32\inetsrv\authanon.dll"
                        },
                        new GlobalModule
                        {
                            Name = "UriCacheModule",
                            Image = @"%windir%\System32\inetsrv\cachuri.dll"
                        },
                        new GlobalModule
                        {
                            Name = "StaticFileModule",
                            Image = @"%windir%\System32\inetsrv\static.dll"
                        }
                    },
                    HandlersSection = new HandlersSection
                    {
                         AccessPolicy = "Read",
                         Handlers = new Handler[]
                         {
                             new Handler
                             {
                                 name = "StaticFile",
                                 path = "*",
                                 verb = "*",
                                 modules = "StaticFileModule",
                                 resourceType = "Either",
                                 requireAccess = "Read"
                             }
                         }
                    },
                    Modules = new Module[]
                    {
                        new Module
                        {
                            Name = "AnonymousAuthenticationModule",
                            LockItem = true
                        },
                        new Module
                        {
                            Name = "StaticFileModule",
                            LockItem = true
                        }
                    },
                    Security = new Security
                    {
                        Access = new SecurityAccess
                        {
                            SslFlags = "None"
                        },
                        Authentication = new SecurityAuthentication
                        {
                            AnonymousAuthentication = new AnonymousAuthentication
                            {
                                Enabled = true,
                                UserName = "IUSR"
                            }
                        }
                    }
                }
            };

            return config;
        }
    }
}
