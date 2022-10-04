using EgBL;

/// <summary>
/// Summary description for BanksEncryptionDecryption
/// </summary>
public class BanksEncryptionDecryption
{
    public static string GetEncryptedString(string PlainText, string KeyName,string Version = null)
    {
        if (KeyName == "BwmHPemeQsQhpwEGWmyQtQ==")
        {
            ePayEncryptionDecryptionBL objEncry = new ePayEncryptionDecryptionBL();
            return objEncry.Encrypt(PlainText, KeyName, 128);
        }
        else
        {
            return SelectEncryption(KeyName, PlainText, Version);
        }
    }
    public static string GetDecryptedString(string EncData, string KeyName, string Version = null)
    {
        if (KeyName == "BwmHPemeQsQhpwEGWmyQtQ==")
        {
            ePayEncryptionDecryptionBL objEpay = new ePayEncryptionDecryptionBL();
            return objEpay.Decrypt(EncData, KeyName, 128);
        }
        else
        {
            //SbiEncryptionDecryption objEncry = new SbiEncryptionDecryption();
            //return objEncry.Decrypt(EncData, System.Web.HttpContext.Current.Server.MapPath("~/WebPages/Key/" + KeyName + ".key"));
            return SelectDecryption(KeyName, EncData, Version);
        }
    }
    private static string SelectEncryption(string KeyName, string PlainText, string Version)
    {
        SbiEncryptionDecryption objEncry = new SbiEncryptionDecryption();
        switch (Version)
        {
            case "2.0":
                //    //return objEncry.Encrypt256(PlainText, System.Web.HttpContext.Current.Server.MapPath("~/WebPages/Key/" + KeyName + ".key"));
                //    return objEncry.EncryptSBIWithKey256(PlainText, System.Web.Configuration.WebConfigurationManager.AppSettings["Securekey"] + KeyName + ".key");
                //case "OBC":
                //    //return objEncry.Encrypt256(PlainText, System.Web.HttpContext.Current.Server.MapPath("~/WebPages/Key/" + KeyName + ".key"));
                //    return objEncry.EncryptSBIWithKey256(PlainText, System.Web.Configuration.WebConfigurationManager.AppSettings["Securekey"] + KeyName + ".key");
                //case "BOB1":
                //    //return objEncry.Encrypt256(PlainText, System.Web.HttpContext.Current.Server.MapPath("~/WebPages/Key/" + KeyName + ".key"));
                //    return objEncry.EncryptSBIWithKey256(PlainText, System.Web.Configuration.WebConfigurationManager.AppSettings["Securekey"] + "BOB1" + ".key");
                //case "PayTMKey":
                //    //return objEncry.Encrypt256(PlainText, System.Web.HttpContext.Current.Server.MapPath("~/WebPages/Key/" + KeyName + ".key"));
                //    return objEncry.EncryptSBIWithKey256(PlainText, System.Web.Configuration.WebConfigurationManager.AppSettings["Securekey"] + "PayTMKey" + ".key");
                return objEncry.EncryptAES256(PlainText, System.Web.Configuration.WebConfigurationManager.AppSettings["Securekey"] + KeyName + ".key");
            default:
                return objEncry.Encrypt(PlainText, System.Web.Configuration.WebConfigurationManager.AppSettings["Securekey"] + KeyName + ".key");
        }
    }
    private static string SelectDecryption(string KeyName, string EncData, string Version)
    {
        SbiEncryptionDecryption objEncry = new SbiEncryptionDecryption();
        switch (Version)
        {
            case "2.0":
                //    //return objEncry.Decrypt256(EncData, System.Web.HttpContext.Current.Server.MapPath("~/WebPages/Key/" + KeyName + ".key"));
                //    return objEncry.DecryptSBIWithKey256(EncData, System.Web.Configuration.WebConfigurationManager.AppSettings["Securekey"] + KeyName + ".key", null);
                //case "OBC":
                //    //return objEncry.Decrypt256(EncData, System.Web.HttpContext.Current.Server.MapPath("~/WebPages/Key/" + KeyName + ".key"));
                //    return objEncry.DecryptSBIWithKey256(EncData, System.Web.Configuration.WebConfigurationManager.AppSettings["Securekey"] + KeyName + ".key", null);
                //case "BOB1":
                //    //return objEncry.Encrypt256(PlainText, System.Web.HttpContext.Current.Server.MapPath("~/WebPages/Key/" + KeyName + ".key"));
                //    return objEncry.DecryptSBIWithKey256(EncData, System.Web.Configuration.WebConfigurationManager.AppSettings["Securekey"] + "BOB1" + ".key", null);
                //case "PayTMKey":
                //    //return objEncry.Decrypt256(EncData, System.Web.HttpContext.Current.Server.MapPath("~/WebPages/Key/" + KeyName + ".key"));
                return objEncry.DecryptAES256(EncData, System.Web.Configuration.WebConfigurationManager.AppSettings["Securekey"] + KeyName + ".key", null);
            default:
                return objEncry.Decrypt(EncData, System.Web.Configuration.WebConfigurationManager.AppSettings["Securekey"] + KeyName + ".key");
        }
       }
    }