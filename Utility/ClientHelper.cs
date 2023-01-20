using Algenta.Colectica.Model.Repository;
using Algenta.Colectica.Repository.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloserSdk.Models;
using Algenta.Colectica.Model;
using Algenta.Colectica.Model.Ddi;
using Algenta.Colectica.Model.Utility;
using System.Configuration;

namespace CloserSdk.Utility
{
	public static class ClientHelper
	{
       
        public static WcfRepositoryClient GetClient()
		{
            //var Url = ConfigurationManager.AppSettings["DevelopmentURL"].ToString();
            //var UserName = ConfigurationManager.AppSettings["DevelopmentUserName"].ToString();
            //var UserPassword = System.Configuration.ConfigurationManager.AppSettings["DevelopmentPassword"].ToString();
            var Url = ConfigurationManager.AppSettings["StagingURL"].ToString();
            var UserName = ConfigurationManager.AppSettings["StagingUserName"].ToString();
            var UserPassword = System.Configuration.ConfigurationManager.AppSettings["StagingPassword"].ToString();
            RepositoryConnectionInfo info = new RepositoryConnectionInfo()
            {
                // TODO Update the hostname as appropriate.
                // Url = "localhost",
                //Url = "https://clsr-clrdp2w01p.ad.ucl.ac.uk",
                //AuthenticationMethod = RepositoryAuthenticationMethod.Windows,
                //TransportMethod = RepositoryTransportMethod.NetTcp

                Url = Url,
                AuthenticationMethod = RepositoryAuthenticationMethod.UserName,
                UserName = UserName,
                Password = UserPassword,     
                TransportMethod = RepositoryTransportMethod.NetTcp
            };
            
			var client = new WcfRepositoryClient(info);
            // var test = new Algenta.Colectica.Model.Ddi.Utility.MetadataUpdateBuilder();
            // test.UpdateProperty
			return client;
		}

        public static RepositoryClientBase GetClient1()
        {
            // Get a client that can be used to interact with Colectica Repository.
            RepositoryConnectionInfo connectionInfo = new RepositoryConnectionInfo()
            {
                Url = "localhost",
                AuthenticationMethod = RepositoryAuthenticationMethod.Windows,
                TransportMethod = RepositoryTransportMethod.NetTcp
            };

            WcfRepositoryClient client = new WcfRepositoryClient(connectionInfo);
            return client;
        }   


    }
}