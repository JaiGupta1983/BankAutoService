using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BankIFSCName
/// </summary>
public  class BankIFSCName
{
    public  BankIFSCName()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    static Dictionary<string, string> bsrdictionary = new Dictionary<string, string>
    {
      {"0006326","SBIN"},
      {"0292861","UBIN"},
      {"0304017","PUNB"},
      {"6910213","IBKL"},
      {"0281065","CBIN"},
      {"0240539","CNRB"},
      {"0200113","BARB"},
      {"9920001","RTSB"},
      {"9910001","PAYU"},
      {"1000132","Epay"},
      {"0220123","BKID" },
      {"0000000","RBI" },
      {"6390013", "ICIC"},

     };
    public static string GetBankName(string code)
    {

       return  bsrdictionary[code];
       
    }
}