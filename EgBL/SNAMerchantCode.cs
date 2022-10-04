using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SNAMerchantCode
/// </summary>
public class SNAMerchantCode
{
    public SNAMerchantCode()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    static Dictionary<string, string> bsrdictionary = new Dictionary<string, string>
    {

       {"9930001","PayTMKey"},
       {"9910001","9910001"},
       {"6001","6001"},
       {"0200113","BOB1"},
       {"6002","6002"},
       {"6003","6003"},
       {"5018","5018"},

     };
    public  string GetBankName(string code)
    {

        return bsrdictionary[code];

    }
}