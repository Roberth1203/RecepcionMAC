using System;
using System.Data;
using System.Configuration;
using System.Windows.Forms;
using System.ServiceModel.Channels;
using Epicor.ServiceModel.StandardBindings;
using Ice.Proxy.BO;
using Ice.Lib;
using Erp.Proxy.BO;
using Erp.BO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adapters
{
    public class EpiFunctions
    {
        Credentials cred;
        protected string fileSys;
        protected string eCompany;
        public string RecycleBin;

        public EpiFunctions(string Username, string Password)
        {
            cred = new Credentials();
            eCompany = ConfigurationManager.AppSettings["epiCompany"].ToString();
            fileSys = String.Format(ConfigurationManager.AppSettings["epiConfig"].ToString(), "Epicor10");
            cred.username = Username;
            cred.pasword = Password;

            changeCompany(eCompany);
        }

        private void changeCompany(string currentCompany)
        {
            try
            {
                string appServerUrl = String.Empty;
                EnvironmentInformation.ConfigurationFileName = fileSys;
                appServerUrl = AppSettingsHandler.AppServerUrl;
                CustomBinding wcfBinding = NetTcp.UsernameWindowsChannel();
                Uri CustSvcUri = new Uri(String.Format("{0}/Ice/BO/{1}.svc", appServerUrl, "UserFile"));

                using (Ice.Proxy.BO.UserFileImpl US = new Ice.Proxy.BO.UserFileImpl(wcfBinding, CustSvcUri))
                {
                    US.ClientCredentials.UserName.UserName = cred.username;
                    US.ClientCredentials.UserName.Password = cred.pasword;
                    US.SaveSettings(cred.username, true, currentCompany, true, false, true, true, true, true, true, true, true,
                                               false, false, -2, 0, 1456, 886, 2, "MAINMENU", "", "", 0, -1, 0, "", false);
                    US.Close();
                    US.Dispose();
                }
            }
            catch (System.UnauthorizedAccessException e)
            {
                RecycleBin = "Error al cambiar de compañia: " + e.Message;
            }
            catch (Exception ee)
            {
                RecycleBin = "Error al cambiar de compañia: " + ee.Message;
            }
        }

        public void NewReceipt(int SupplierID)
        {
            try
            {
                string appServerUrl = String.Empty;
                EnvironmentInformation.ConfigurationFileName = fileSys;
                appServerUrl = AppSettingsHandler.AppServerUrl;
                CustomBinding wcfBinding = NetTcp.UsernameWindowsChannel();
                Uri CustSvcUri = new Uri(String.Format("{0}/Ice/BO/{1}.svc", appServerUrl, "Receipt"));

                using (ReceiptImpl BO = new ReceiptImpl(wcfBinding, CustSvcUri))
                {
                    ReceiptDataSet rpt = new ReceiptDataSet();
                    rpt.EnforceConstraints = false;

                    BO.GetNewRcvHead(rpt, SupplierID, "");
                    //rpt.Tables["RcvHead"].Rows[0][]
                    BO.Update(rpt);
                }
            }
            catch (Ice.Common.BusinessObjectException epi)
            {
                RecycleBin = "EpicorExcepción al crear nueva recepción: " + epi.Message;
            }
            catch (Exception ex)
            {
                RecycleBin = "SystemExcepción al crear nueva recepción: " + ex.Message;
            }
        }

        public void NewReceiptLine()
        {
            try
            {
                string appServerUrl = String.Empty;
                EnvironmentInformation.ConfigurationFileName = fileSys;
                appServerUrl = AppSettingsHandler.AppServerUrl;
                CustomBinding wcfBinding = NetTcp.UsernameWindowsChannel();
                Uri CustSvcUri = new Uri(String.Format("{0}/Ice/BO/{1}.svc", appServerUrl, "Receipt"));

                using (ReceiptImpl BO = new ReceiptImpl(wcfBinding, CustSvcUri))
                {

                }
            }
            catch (Ice.Common.BusinessObjectException epi)
            {
                RecycleBin = "EpicorExcepción al crear línea en la recepción: " + epi.Message;
            }
            catch (Exception ex)
            {
                RecycleBin = "SystemExcepción al crear línea en la recepción: " + ex.Message;
            }
        }
    }
}
