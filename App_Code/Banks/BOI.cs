using System;
using System.Configuration;
using System.Net;

/// <summary>
/// Summary description for BOI
/// </summary>
public class BOI : Banks
{
    public BOI()
    {
        KeyName = "BOI";
    }

    public override string CallManualDataPushService(string cipherText)
    {
        throw new NotImplementedException();
    }

    public override string CallVerifyManualService(string cipherText)
    {
        throw new NotImplementedException();
    }

    public override string CallVerifyService(string cipherText)
    {
        try
        {
            using (BOIWebServ.RJSTVerify BOIService = new BOIWebServ.RJSTVerify())
            {
                IpAddress = Dns.GetHostByName(ConfigurationManager.AppSettings["BOIWebServ.RJSTVerify"].Split('/').GetValue(2).ToString().Replace("www.", "")).AddressList[0].ToString();
                return BOIService.UpdateGRN_RJST(cipherText);
            }
        }
        catch (Exception ex)
        {
            InsertErrorLog(ex.Message, "0220123", ConfigurationManager.AppSettings["BOIWebServ.RJSTVerify"].Split('/').GetValue(2).ToString().Replace("www.", ""), 1);
            return null;
        }
     }
}